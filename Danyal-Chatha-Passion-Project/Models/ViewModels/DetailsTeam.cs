using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Danyal_Chatha_Passion_Project.Models.ViewModels
{
    public class DetailsTeam
    {
        public TeamDto SelectedTeam { get; set; }

        public IEnumerable<PlayerDto> RelatedPlayers { get; set; }
    }
}