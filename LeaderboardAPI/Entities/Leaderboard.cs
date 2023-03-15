using System;
using System.Collections.Generic;

#nullable disable

namespace LeaderboardAPI.Entities
{
    public partial class Leaderboard
    {
        public string WholesellerCode { get; set; }
        public int RoundId { get; set; }
        public string GroupName { get; set; }
        public int BaselineStock { get; set; }
        public DateTime SaleDate { get; set; }
        public int SalePoint { get; set; }
        public int Rank { get; set; }
        public int PointA { get; set; }
        public int PointB { get; set; }
    }
}
