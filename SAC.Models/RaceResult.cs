using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAC.Models
{
    public class RaceResult
    {
        public int Id { get; set; }
        public int Position { get; set; }

        public int RaceId { get; set; }
        public int AthleteId { get; set; }
        public int AgeRankId { get; set; }
    }
}
