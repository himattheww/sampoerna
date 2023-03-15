namespace LeaderboardAPI.ViewModels.Output
{
    public class LeaderBoardVM
    {
        public string WholesellerCode { get; set; }
        public int RoundId { get; set; }
        public string GroupName { get; set; }
        public int BaselineStock { get; set; }
        public DateTime SaleDate { get; set; }
        public int SalePoint { get; set; }
        public int Rank { get; set; }
        public int Point_Tetap { get; set; }
        public int Potensi_Point { get; set; }
        public string? Status { get; set; }
        public string? Keterangan { get; set; }
    }
}
