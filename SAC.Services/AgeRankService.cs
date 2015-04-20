using SAC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAC.Services
{
    public class AgeRankService : IDisposable
    {
        SACServiceContext _context;

        public AgeRankService(SACServiceContext context)
        {
            _context = context;
        }

        public int AgeRankExists(string ageRankName)
        {
            AgeRank ageRank = _context.AgeRanks.Where(r => r.Name == ageRankName).FirstOrDefault();
            return ageRank != null ? ageRank.Id : -1;
        }

        public IQueryable<AgeRank> GetAgeRanks()
        {
            return _context.AgeRanks;
        }

        public AgeRank AddAgeRank(string name)
        {
            AgeRank ageRank = _context.AgeRanks.Add(new AgeRank
            {
                Name = name
            });
            _context.SaveChanges();
            return ageRank;
        }

        public void Dispose()
        {
            
        }

    }
}
