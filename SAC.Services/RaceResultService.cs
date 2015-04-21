using SAC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAC.Services
{
    public class RaceResultService : IDisposable
    {
        SACServiceContext _context;

        public RaceResultService(SACServiceContext context)
        {
            _context = context;
        }

        public RaceResult AddRaceResult(int position, int points, int athleteId, int ageRankId, int raceId)
        {
            RaceResult result = _context.RaceResults.Add(new RaceResult
            {
                Position = position,
                Points = points,
                AthleteId = athleteId,
                AgeRankId = ageRankId,
                RaceId = raceId
            });
            _context.SaveChanges();
            return result;
        }

        public void Dispose()
        {
            
        }

    }
}
