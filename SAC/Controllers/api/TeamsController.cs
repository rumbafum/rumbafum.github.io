using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using SAC.Models;

namespace App.SAC.Controllers.api
{
    public class TeamsController : ApiController
    {
        private SACServiceContext db = new SACServiceContext();

        // GET: api/Teams
        public IQueryable<Team> GetTeams()
        {
            return db.Teams;
        }

        // GET: api/Teams/5
        [ResponseType(typeof(Team))]
        public async Task<IHttpActionResult> GetTeam(int id)
        {
            Team team = await db.Teams.FindAsync(id);
            if (team == null)
            {
                return NotFound();
            }

            return Ok(team);
        }

        public List<TeamClassificationDto> GetTeamClassificationByRace(int raceId)
        {
            int position = 1;
            IQueryable<TeamClassificationDto> collection = 
                db.RaceResults.Where(rr => rr.RaceId == raceId).GroupBy(rr => rr.Athlete.TeamId).OrderByDescending(tc => tc.Sum(x => x.Points)).
                    Select(rr => new TeamClassificationDto
                    {
                        Id = rr.FirstOrDefault().Athlete.TeamId,
                        Name = rr.FirstOrDefault().Athlete.Team.Name,
                        Points = rr.Sum(x => x.Points),
                        Position = 0
                    });
            var list = collection.ToList();
            foreach (TeamClassificationDto item in list)
            {
                item.Position = position;
                position++;
            }

            return list;
        }

        public List<TeamClassificationDto> GetTeamClassificationByRaceAndAgeRank(int raceId, int ageRankId)
        {
            int position = 1;
            IQueryable<TeamClassificationDto> collection =
                db.RaceResults.Where(rr => rr.RaceId == raceId && rr.AgeRankId == ageRankId).GroupBy(rr => rr.Athlete.TeamId).
                    OrderByDescending(tc => tc.Sum(x => x.Points)).
                    Select(rr => new TeamClassificationDto
                    {
                        Id = rr.FirstOrDefault().Athlete.TeamId,
                        Name = rr.FirstOrDefault().Athlete.Team.Name,
                        Points = rr.Sum(x => x.Points),
                        Position = 0
                    });
            var list = collection.ToList();
            foreach (TeamClassificationDto item in list)
            {
                item.Position = position;
                position++;
            }

            return list;
        }

        public List<TeamClassificationDto> GetTeamClassification(int ageRankId)
        {
            int position = 1;
            IQueryable<TeamClassificationDto> collection;
            if(ageRankId == -1)
                collection = db.RaceResults.GroupBy(rr => rr.Athlete.TeamId).OrderByDescending(tc => tc.Sum(x => x.Points)).Select(rr => new TeamClassificationDto
                { 
                    Id = rr.FirstOrDefault().Athlete.TeamId,
                    Name = rr.FirstOrDefault().Athlete.Team.Name,
                    Points = rr.Sum(x => x.Points),
                    Position = 0
                });
            else
                collection = db.RaceResults.Where(rr => rr.AgeRankId == ageRankId).GroupBy(rr => rr.Athlete.TeamId).OrderByDescending(tc => tc.Sum(x => x.Points)).
                    Select(rr => new TeamClassificationDto
                {
                    Id = rr.FirstOrDefault().Athlete.TeamId,
                    Name = rr.FirstOrDefault().Athlete.Team.Name,
                    Points = rr.Sum(x => x.Points),
                    Position = 0
                });
            var list = collection.ToList();
            foreach (TeamClassificationDto item in list)
            {
                item.Position = position;
                position++;
            }
                
            return list;
        }

        // PUT: api/Teams/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutTeam(int id, Team team)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != team.Id)
            {
                return BadRequest();
            }

            db.Entry(team).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TeamExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Teams
        [ResponseType(typeof(Team))]
        public async Task<IHttpActionResult> PostTeam(Team team)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Teams.Add(team);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = team.Id }, team);
        }

        // DELETE: api/Teams/5
        [ResponseType(typeof(Team))]
        public async Task<IHttpActionResult> DeleteTeam(int id)
        {
            Team team = await db.Teams.FindAsync(id);
            if (team == null)
            {
                return NotFound();
            }

            db.Teams.Remove(team);
            await db.SaveChangesAsync();

            return Ok(team);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TeamExists(int id)
        {
            return db.Teams.Count(e => e.Id == id) > 0;
        }
    }
}