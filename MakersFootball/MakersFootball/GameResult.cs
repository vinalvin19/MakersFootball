﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MakersFootball
{
    public class GameResult
    {
        public DateTime GameDate { get; set; }

        public String TeamName { get; set; }

        public HomeOrAway HomeOrAway { get; set; }

        public int Goals { get; set; }

        public int GoalAttempts { get; set; }

        public int ShotsOnGoal { get; set; }

        public int ShotsOffGoal { get; set; }
    }

    public enum HomeOrAway
    {
        Home,
        Away
    }
}
