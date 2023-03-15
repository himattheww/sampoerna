using LeaderboardAPI.Entities;

namespace LeaderboardAPI.ViewModels.Output
{
    public class RoundVM
    {
        public int CurrentRoundId { get; set; }
        public string CurrentRoundName { get; set; }
        public int CompletedRounds { get; set; }
        public int TotalRounds { get; set; }
        public List<Object> MonthList { get; set; }
    }
}
