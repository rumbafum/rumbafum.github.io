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