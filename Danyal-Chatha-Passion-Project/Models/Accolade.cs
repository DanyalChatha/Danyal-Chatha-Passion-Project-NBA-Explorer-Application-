using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Danyal_Chatha_Passion_Project.Models
{
    public class Accolade
    {
        [Key]
        public int AccoladeId { get; set; }
        public string AccoladeName { get; set; }
        public int AccoladeYear { get; set; }

        //An accolade can go to many players
        public ICollection<Player> Players { get; set; }
    }

        public class AccoladeDto
        {
            public int AccoladeId { get; set; }
            public string AccoladeName { get; set; }
            public int AccoladeYear { get; set; }

        
        }
    
}