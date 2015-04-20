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
    public class RaceResultsController : ApiController
    {
        private SACServiceContext db = new SACServiceContext();

        // GET: api/RaceResults
        public IQueryable<RaceResult> GetRaceResults()
        {
            return db.RaceResults;
        }

        // GET: api/RaceResults/5
        [ResponseType(typeof(RaceResult))]
        public async Task<IHttpActionResult> GetRaceResult(int id)
        {
            RaceResult raceResult = await db.RaceResults.FindAsync(id);
            if (raceResult == null)
            {
                return NotFound();
            }

            return Ok(raceResult);
        }

        // PUT: api/RaceResults/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutRaceResult(int id, RaceResult raceResult)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != raceResult.Id)
            {
                return BadRequest();
            }

            db.Entry(raceResult).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RaceResultExists(id))
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

        // POST: api/RaceResults
        [ResponseType(typeof(RaceResult))]
        public async Task<IHttpActionResult> PostRaceResult(RaceResult raceResult)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.RaceResults.Add(raceResult);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = raceResult.Id }, raceResult);
        }

        // DELETE: api/RaceResults/5
        [ResponseType(typeof(RaceResult))]
        public async Task<IHttpActionResult> DeleteRaceResult(int id)
        {
            RaceResult raceResult = await db.RaceResults.FindAsync(id);
            if (raceResult == null)
            {
                return NotFound();
            }

            db.RaceResults.Remove(raceResult);
            await db.SaveChangesAsync();

            return Ok(raceResult);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool RaceResultExists(int id)
        {
            return db.RaceResults.Count(e => e.Id == id) > 0;
        }
    }
}