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
    public class AthletesController : ApiController
    {
        private SACServiceContext db = new SACServiceContext();

        // GET: api/Athletes
        public IQueryable<Athlete> GetAthletes()
        {
            return db.Athletes;
        }

        [ResponseType(typeof(Athlete))]
        public IHttpActionResult GetAthleteWithData(int id)
        {
            Athlete athlete = db.Athletes.Where(a => a.Id == id).Include(a => a.AgeRank).Include(a => a.Team).FirstOrDefault();
            if (athlete == null)
            {
                return NotFound();
            }

            return Ok(athlete);
        }

        public IQueryable<AthleteDto> GetAthletesByTeam(int teamId)
        {
            var athletes = db.Athletes.Where(a => a.TeamId == teamId).OrderBy(a => a.AgeRankId).Include(a => a.AgeRank);
            return athletes.Select(a => new AthleteDto { 
                Id = a.Id,
                Name = a.Name,
                AgeRank = a.AgeRank.Name,
                Number = a.Number,
                Team = a.Team.Name,
                TotalPoints = db.RaceResults.Where(rr => rr.AthleteId == a.Id).Sum(rr => rr.Points)
            });
        }

        public List<AthleteClassificationDto> GetAthletesClassification(int ageRankId)
        {
            int position = 1;
            IQueryable<AthleteClassificationDto> collection = 
                db.RaceResults.Where(rr => rr.AgeRankId == ageRankId).GroupBy(rr => rr.AthleteId).OrderByDescending(tc => tc.Sum(x => x.Points)).
                    Select(rr => new AthleteClassificationDto
                    {
                        Id = rr.FirstOrDefault().AthleteId,
                        Name = rr.FirstOrDefault().Athlete.Name,
                        Number = rr.FirstOrDefault().Athlete.Number,
                        Team = rr.FirstOrDefault().Athlete.Team.Name,
                        TeamId = rr.FirstOrDefault().Athlete.TeamId,
                        Points = rr.Sum(x => x.Points),
                        Position = 0,
                        Races = rr.Count()
                    });
            var list = collection.ToList();
            foreach (AthleteClassificationDto item in list)
            {
                item.Position = position;
                position++;
            }

            return list;
        }

        // PUT: api/Athletes/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutAthlete(int id, Athlete athlete)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != athlete.Id)
            {
                return BadRequest();
            }

            db.Entry(athlete).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AthleteExists(id))
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

        // POST: api/Athletes
        [ResponseType(typeof(Athlete))]
        public async Task<IHttpActionResult> PostAthlete(Athlete athlete)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Athletes.Add(athlete);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = athlete.Id }, athlete);
        }

        // DELETE: api/Athletes/5
        [ResponseType(typeof(Athlete))]
        public async Task<IHttpActionResult> DeleteAthlete(int id)
        {
            Athlete athlete = await db.Athletes.FindAsync(id);
            if (athlete == null)
            {
                return NotFound();
            }

            db.Athletes.Remove(athlete);
            await db.SaveChangesAsync();

            return Ok(athlete);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool AthleteExists(int id)
        {
            return db.Athletes.Count(e => e.Id == id) > 0;
        }
    }
}