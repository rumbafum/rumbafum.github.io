using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAC.Models
{
    public class Athlete
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public int Number { get; set; }

        public int TeamId { get; set; }

        public int AgeRankId { get; set; }

        public Team Team { get; set; }

        public AgeRank AgeRank { get; set; }
    }

    public class AthleteDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Number { get; set; }
        public string Team { get; set; }
        public string AgeRank { get; set; }
        public int TotalPoints { get; set; }
    }

    public class AthleteClassificationDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Number { get; set; }
        public string Team { get; set; }
        public int TeamId { get; set; }
        public int Points { get; set; }
        public int Races { get; set; }
        public int Position { get; set; }
    }
}
