namespace LeaderboardAPI.ViewModels.Input
{
    public class RoundUpdateVM
    {
        public int RoundId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
