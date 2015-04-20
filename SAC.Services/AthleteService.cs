using SAC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAC.Services
{
    public class AthleteService : IDisposable
    {
        SACServiceContext _context;

        public AthleteService(SACServiceContext context)
        {
            _context = context;
        }

        public int AthleteExists(string athleteName)
        {
            Athlete athlete = _context.Athletes.Where(t => t.Name == athleteName).FirstOrDefault();
            return athlete != null ? athlete.Id : -1;
        }

        public IQueryable<Athlete> GetAthletes()
        {
            return _context.Athletes;
        }

        public Athlete AddAthlete(string name, int bibNumber, int ageRankId, int teamId)
        {
            Athlete athlete = _context.Athletes.Add(new Athlete
            {
                Name = name,
                Number = bibNumber,
                AgeRankId = ageRankId,
                TeamId = teamId
            });
            _context.SaveChanges();
            return athlete;
        }

        public void Dispose()
        {
            
        }

    }
}
