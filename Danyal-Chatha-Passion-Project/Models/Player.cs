using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Danyal_Chatha_Passion_Project.Models
{
    public class Player
    {
        [Key]
        public int PlayerId { get; set; }
        public string PlayerName { get; set; }
        public int  PlayerJersey { get; set; }
        public string PlayerPosition { get; set; }

        //A player belong to one team
        //A team can have many players
        [ForeignKey("Team")]
        public int TeamId { get; set; }
        public virtual Team Team { get; set; }
        public string TeamBio { get; set; }


        //An player can have many accolades
        public ICollection<Accolade> Accolades { get; set; }
    }   

        public class PlayerDto
        {
        public int PlayerId { get; set; }
        public string PlayerName { get; set; }
        public int PlayerJersey { get; set; }
        public string PlayerPosition { get; set; }
        public string TeamName { get; set; }
        public string TeamBio { get; set; }
        public int TeamId { get; set; }
        }
}