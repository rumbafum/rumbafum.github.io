using SAC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAC.Services
{
    public class RaceService : IDisposable
    {
        SACServiceContext _context;

        public RaceService(SACServiceContext context)
        {
            _context = context;
        }

        public int RaceExists(string raceName)
        {
            Race race = _context.Races.Where(r => r.Name == raceName).FirstOrDefault();
            return race != null ? race.Id : -1;
        }

        public IQueryable<Race> GetRaces()
        {
            return _context.Races;
        }

        public Race AddRace(string name, DateTime? raceDate)
        {
            Race race = _context.Races.Add(new Race
            {
                Name = name,
                RaceDate = raceDate
            });
            _context.SaveChanges();
            return race;
        }

        public void Dispose()
        {
            
        }

    }
}
