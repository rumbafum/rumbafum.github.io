using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAC.Models
{
    public class Team
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
    }

    public class TeamClassificationDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Points { get; set; }
        public int Position { get; set; }
    }
}
