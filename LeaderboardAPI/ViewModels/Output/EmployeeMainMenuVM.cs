using LeaderboardAPI.Entities;

namespace LeaderboardAPI.ViewModels.Output
{
    public class EmployeeMainMenuVM
    {
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public string RoleType { get; set; }
        public string RoleName { get; set; }
        public WholesellerSaleDetail? WholesellerDetail { get; set; }
        public List<WholesellerVM> Wholesellers { get; set; }
        public List<WholesellerSaleVM> Standings { get; set; }

    }
}
