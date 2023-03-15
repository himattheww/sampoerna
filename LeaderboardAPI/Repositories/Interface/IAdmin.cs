using LeaderboardAPI.Entities;
using LeaderboardAPI.ViewModels;
using LeaderboardAPI.ViewModels.Input;
using LeaderboardAPI.ViewModels.Output;
using System.Data;

namespace LeaderboardAPI.Repositories.Interface
{
    public interface IAdmin
    {
        Task InsertUserProfile(Employee employee);
        Task InsertWholesellerMapping(WholesellerMapping wholeseller);
        Employee CreateUserProfile(DataRow row);
        Leaderboard CreateLeaderBoard(DataRow row);
        WholesellerMapping CreateWholesellerMapping(DataRow row);
        Task InsertLeaderBoard(Leaderboard leaderboard);
        Round GetRoundId(int roundId);
        int DeleteRoundId(int roundId);
        int UpdateRound(Round round);
        IEnumerable<Round> GetAllRound();
        IEnumerable<EmployeeVM> GetAllEmployeesFromAdmin();
        IEnumerable<EmployeeVM> GetEmployeeFromAdminByName(string? employeeName);
        void DetachDuplicateEmployee(Employee dupEmployee);
        void DetachDuplicateLeaderboard(Leaderboard leaderboard);
        IEnumerable<UserProfile> GetEmployeesResult(List<string> resultList);
        IEnumerable<Leaderboard> GetLeaderboardsResult(List<string> resultList, int roundId);
        Task CreateFileUpload(FileUpload fileUpload);
        Task<IEnumerable<FileUploadVM>> GetAllFileUpload();
        string GetEmployeeOutput(string customerCode);
        Employee GetEmployeeId(string employeeId);
        Task UpdateEmployee(Employee employee);
        WholesellerMapping GetWholesellerId(string wholesellerCode);
        Task UpdateWholesellerMapping(WholesellerMapping wholeseller);
        DataTable? CreateDataTableFromDatabaseEmployees();
        DataTable? CreateDataTableFromDatabaseLeaderBoard();
        Task UploadFileToS3(Stream data, string file);
    }
}
