using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Danyal_Chatha_Passion_Project.Models.ViewModels
{
    public class UpdatePlayer
    {
        public PlayerDto SelectedPlayer { get; set; }

        public IEnumerable<TeamDto> TeamOptions { get; set; }
    }
}