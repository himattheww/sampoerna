using System;
using System.Collections.Generic;

#nullable disable

namespace LeaderboardAPI.Entities
{
    public partial class Round
    {
        public int RoundId { get; set; }
        public string RoundName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
