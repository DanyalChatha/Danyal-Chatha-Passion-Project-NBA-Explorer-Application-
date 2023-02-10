using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Danyal_Chatha_Passion_Project.Models.ViewModels
{
    public class DetailAccolade
    {
        public AccoladeDto SelectedAccolade { get; set; }
        public IEnumerable<PlayerDto> RewardedPlayer { get; set; }
    }
}