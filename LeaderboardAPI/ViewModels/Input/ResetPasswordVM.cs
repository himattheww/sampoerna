namespace LeaderboardAPI.ViewModels.Input
{
    public class ResetPasswordVM
    {
        public string CustomerCode { get; set; }
        public string? ChangeToken { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
