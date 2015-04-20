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
    public class AgeRanksController : ApiController
    {
        private SACServiceContext db = new SACServiceContext();

        // GET: api/AgeRanks
        public IQueryable<AgeRank> GetAgeRanks()
        {
            return db.AgeRanks;
        }

        // GET: api/AgeRanks/5
        [ResponseType(typeof(AgeRank))]
        public async Task<IHttpActionResult> GetAgeRank(int id)
        {
            AgeRank ageRank = await db.AgeRanks.FindAsync(id);
            if (ageRank == null)
            {
                return NotFound();
            }

            return Ok(ageRank);
        }

        // PUT: api/AgeRanks/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutAgeRank(int id, AgeRank ageRank)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != ageRank.Id)
            {
                return BadRequest();
            }

            db.Entry(ageRank).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AgeRankExists(id))
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

        // POST: api/AgeRanks
        [ResponseType(typeof(AgeRank))]
        public async Task<IHttpActionResult> PostAgeRank(AgeRank ageRank)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.AgeRanks.Add(ageRank);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = ageRank.Id }, ageRank);
        }

        // DELETE: api/AgeRanks/5
        [ResponseType(typeof(AgeRank))]
        public async Task<IHttpActionResult> DeleteAgeRank(int id)
        {
            AgeRank ageRank = await db.AgeRanks.FindAsync(id);
            if (ageRank == null)
            {
                return NotFound();
            }

            db.AgeRanks.Remove(ageRank);
            await db.SaveChangesAsync();

            return Ok(ageRank);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool AgeRankExists(int id)
        {
            return db.AgeRanks.Count(e => e.Id == id) > 0;
        }
    }
}