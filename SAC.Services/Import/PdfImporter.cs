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

            string pdfText = ExtractTextFromPdf(path, teams);

            if(string.IsNullOrWhiteSpace(pdfText))
                return "No data to import.";
            return ProcessText(pdfText, raceDate, db);
        }

        private static string ExtractTextFromPdf(string path, List<string> teams)
        {
            using (PdfReader reader = new PdfReader(path))
            {
                SACTextExtractionStrategy strategy = new SACTextExtractionStrategy(reader.NumberOfPages, teams);

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
                            currentAgeRankId = ageRankService.AddAgeRank(currentAgeRank).Id;
                        continue;
                    }
                    string[] result = lines[i].Split(new string[] { "\t" }, StringSplitOptions.RemoveEmptyEntries);
                    if (result.Length < 5) continue;

                    int position = Convert.ToInt16(result[0].Replace("º", "").Trim());
                    int bibNumber = Convert.ToInt16(result[1].Trim());
                    string athleteName = result[2];
                    string teamName = result[3];
                    int points = Convert.ToInt16(result[4].Trim());

                    int athleteId;
                    if ((athleteId = athleteService.AthleteExists(athleteName)) == -1)
                    {
                        int teamId;
                        if ((teamId = teamService.TeamExists(teamName)) == -1)
                            teamId = teamService.AddTeam(teamName).Id;
                        athleteId = athleteService.AddAthlete(athleteName, bibNumber, currentAgeRankId, teamId).Id;
                    }

                    if(raceId != -1)
                        raceResultService.AddRaceResult(position, points, athleteId, currentAgeRankId, raceId);
                }
            }

            return string.Empty;
        }
    }
}
