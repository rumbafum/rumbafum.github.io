using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using SAC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAC.Services.Import
{
    public static class PdfImporter
    {
        public static string ImportPdf(string path, DateTime? raceDate, SACServiceContext db)
        {
            List<string> teams;
            using (TeamService teamService = new TeamService(db))
                teams = teamService.GetTeams().Select(t => t.Name).ToList();

            string pdfText = ExtractTextFromPdf(path, teams, false);

            if(string.IsNullOrWhiteSpace(pdfText))
                return "No data to import.";
            return ProcessText(pdfText, raceDate, db);
        }

        public static string ImportInitialData(string path, SACServiceContext db)
        {
            string pdfText = ExtractTextFromPdf(path, new List<string>(), true);

            if (string.IsNullOrWhiteSpace(pdfText))
                return "No data to import.";
            return ProcessInitialData(pdfText, db);
        }

        private static string ExtractTextFromPdf(string path, List<string> teams, bool initialData)
        {
            using (PdfReader reader = new PdfReader(path))
            {
                SACTextExtractionStrategy strategy = new SACTextExtractionStrategy(reader.NumberOfPages, teams, initialData);

                StringBuilder text = new StringBuilder();
                for (int i = 1; i <= reader.NumberOfPages; i++)
                {
                    text.Append(PdfTextExtractor.GetTextFromPage(reader, i, strategy));
                }
                return text.ToString();
            }
        }

        private static string ProcessText(string pdfText, DateTime? raceDate, SACServiceContext db)
        {
            string[] lines = pdfText.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

            int currentAgeRankId = -1;
            using (RaceService raceService = new RaceService(db))
            using (AgeRankService ageRankService = new AgeRankService(db))
            using(TeamService teamService = new TeamService(db))
            using(AthleteService athleteService = new AthleteService(db))
            using(RaceResultService raceResultService = new RaceResultService(db))
            {
                int raceId = -1;
                for (int i = 0; i < lines.Length; i++)
                {
                    if (i < 2) continue;
                    if (string.IsNullOrEmpty(lines[i])) continue;
                    if (i == 2)
                    {
                        if ((raceId = raceService.RaceExists(lines[i])) != -1)
                            return "A Race with this name already exists!";
                        raceId = raceService.AddRace(lines[i], raceDate).Id;
                        continue;
                    }
                    if (lines[i].StartsWith("Class."))
                    {
                        string currentAgeRank = lines[i - 1];
                        if (currentAgeRank.ToLower().Contains("(cont.)"))
                            continue;
                        if ((currentAgeRankId = ageRankService.AgeRankExists(currentAgeRank)) == -1)
                            currentAgeRankId = ageRankService.AddAgeRank(SAC.Models.AgeRankMappings.GetAgeRankNameByCode(currentAgeRank)).Id;
                        continue;
                    }
                    string[] result = lines[i].Split(new string[] { "\t" }, StringSplitOptions.RemoveEmptyEntries);
                    if (result.Length < 5) continue;

                    int test;
                    int position;
                    int bibNumber;
                    string athleteName;
                    string teamName;
                    int points;
                    if (int.TryParse(result[1].Trim(), out test))
                    {
                        position = Convert.ToInt16(result[0].Replace("º", "").Trim());
                        bibNumber = Convert.ToInt16(result[1].Trim());
                        athleteName = result[2];
                        teamName = result[3];
                        points = 0;
                        Int32.TryParse(result[4].Trim(), out points);
                    }
                    else
                    {
                        position = Convert.ToInt16(result[0].Replace("º", "").Trim());
                        bibNumber = Convert.ToInt16(result[2].Trim());
                        athleteName = result[1];
                        teamName = result[3];
                        points = 0;
                        Int32.TryParse(result[4].Trim(), out points);
                    }

                    int athleteId;
                    if ((athleteId = athleteService.AthleteExists(bibNumber)) == -1)
                    {
                        int teamId;
                        if ((teamId = teamService.TeamExists(teamName)) == -1)
                            teamId = teamService.AddTeam(teamName).Id;
                        athleteId = athleteService.AddAthlete(athleteName, bibNumber, currentAgeRankId, teamId).Id;
                    }
                    else
                    {
                        Athlete athlete = athleteService.GetAthlete(athleteId);
                        if (athlete.AgeRankId != currentAgeRankId)
                            athleteService.UpdateAthlete(athleteId, currentAgeRankId);
                    }

                    if(raceId != -1)
                        raceResultService.AddRaceResult(position, points, athleteId, currentAgeRankId, raceId);
                }
            }

            return string.Empty;
        }

        private static string ProcessInitialData(string pdfText, SACServiceContext db)
        {
            string[] lines = pdfText.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

            using (TeamService teamService = new TeamService(db))
            using (AthleteService athleteService = new AthleteService(db))
            using (AgeRankService ageRankService = new AgeRankService(db))
            {
                foreach (var line in lines)
                {
                    int bibNumber;
                    string firstName;
                    string lastName;
                    string teamName;
                    string fullName;
                    string ageRank;
                    string[] fields = line.Split(new string[] { "\t" }, StringSplitOptions.RemoveEmptyEntries);
                    if (fields.Length < 5) continue;

                    if (!Int32.TryParse(fields[0], out bibNumber))
                        continue;

                    firstName = fields[1];
                    lastName = fields[2];
                    teamName = fields[3];
                    ageRank = fields[4];

                    int teamId = teamService.TeamExists(teamName);
                    if (teamId == -1)
                        teamId = teamService.AddTeam(teamName).Id;

                    int ageRankId = ageRankService.AgeRankExists(AgeRankMappings.GetAgeRankNameByCode(ageRank));
                    if (ageRankId == -1)
                        ageRankId = ageRankService.AddAgeRank(AgeRankMappings.GetAgeRankNameByCode(ageRank)).Id;

                    fullName = firstName.Trim() + " " + lastName.Trim();
                    Athlete athlete;
                    int athleteId = athleteService.AthleteExists(fullName);
                    if (athleteId == -1)
                        athlete = athleteService.AddAthlete(fullName, bibNumber, ageRankId, teamId);
                }
            }

            return "";
        }
    }
}
