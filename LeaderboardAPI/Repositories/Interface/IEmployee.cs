using LeaderboardAPI.Entities;
using LeaderboardAPI.ViewModels;
using LeaderboardAPI.ViewModels.Input;
using LeaderboardAPI.ViewModels.Output;

namespace LeaderboardAPI.Repositories.Interface
{
    public interface IEmployee
    {
        EmployeeVM? GetEmployeeOutput(string customerCode);
        Employee FindEmployeeFullData(string customerCode);
        int SetEmployeeToken(Employee tokenEmployee);
        int ChangePassword(string CustomerCode, string newPassword, ulong? firstLogin = null);
        Employee GenerateEmployeeToken(string customerCode);
        string? VerifyConfirmPassword(string oldPassword, string newPassword, string confirmPassword);
        EmployeeVM? GetTokenOwner(string token);
        RoundVM GetRoundDetails();
        EmployeeMainMenuVM? GetMainMenuData(string customerCode);
        IEnumerable<WholesellerSaleVM> GetLeaderboard(LeaderboardInputVM leaderboardInput);
        int ClearForgotRequestOnLogin(string customerCode);
        int updateEmployeePhone(PhoneNumberVM phoneNumber);
        string? VerifyConfirmPhoneNumber(string phoneNUmber);
    }
}
