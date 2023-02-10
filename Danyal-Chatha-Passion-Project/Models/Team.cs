using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace Danyal_Chatha_Passion_Project.Models
{
    public class Team
    {
        [Key]
        public int TeamId { get; set; }
        public string TeamName { get; set; }

        public string TeamBio { get; set; } 

    }

    public class TeamDto
    {
        public int TeamId { get; set; }
        public string TeamName { get; set; }
        public string TeamBio { get; set; }

    }
}