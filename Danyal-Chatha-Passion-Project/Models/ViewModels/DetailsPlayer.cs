﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Danyal_Chatha_Passion_Project.Models.ViewModels
{
    public class DetailsPlayer
    {
        public PlayerDto SelectedPlayer { get; set; }
        public IEnumerable<AccoladeDto> AquiredAccolades { get; set; }
        public IEnumerable<AccoladeDto> AvailableAccolade { get; set; }
    }
}