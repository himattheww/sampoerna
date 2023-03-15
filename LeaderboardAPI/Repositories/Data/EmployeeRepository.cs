using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Vml;
using LeaderboardAPI.CustomServices;
using LeaderboardAPI.Entities;
using LeaderboardAPI.Repositories.Interface;
using LeaderboardAPI.ViewModels;
using LeaderboardAPI.ViewModels.Input;
using LeaderboardAPI.ViewModels.Output;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace LeaderboardAPI.Repositories.Data
{
    public class EmployeeRepository : IEmployee
    {
        private readonly LeaderboardContext _context;

        public EmployeeRepository(LeaderboardContext context)
        {
            _context = context;
        }

        public EmployeeVM? GetEmployeeOutput(string customerCode)
        {
            EmployeeVM? employeeVM = null;
           
            var employee = (from emp in _context.Employees
                                join ld in _context.Leaderboards
                                on emp.EmployeeCode equals ld.WholesellerCode into empld
                                from subempld in empld.DefaultIfEmpty()
                                where emp.EmployeeCode == customerCode
                                select new EmployeeVM
                                {
                                    EmployeeCode = emp.EmployeeCode,
                                    EmployeeRole = emp.RoleType,
                                    EmployeeName = emp.EmployeeName,
                                    EmployeeEmail = emp.EmployeeEmail,
                                    EmployeePhone = emp.EmployeePhone,
                                    GroupName = subempld.GroupName ?? ""
                                }).FirstOrDefault();

            if (employee != null)
            {
                employeeVM = employee;
            }

            return employeeVM;
        }

        public Employee FindEmployeeFullData(string customerCode)
        {
            var dataResult = _context.Employees.Find(customerCode);
            if (dataResult != null)
            {
                _context.Entry(dataResult).State = EntityState.Detached;
            }
            return dataResult;
        }

        public int SetEmployeeToken(Employee tokenEmployee)
        {
            _context.Employees.Attach(tokenEmployee);
            _context.Entry(tokenEmployee).Property(x => x.FirstLogin).IsModified = true;
            _context.Entry(tokenEmployee).Property(x => x.PasswordIssued).IsModified = true;
            _context.Entry(tokenEmployee).Property(x => x.ChangeToken).IsModified = true;
            _context.Entry(tokenEmployee).Property(x => x.TokenExpired).IsModified = true;
            return _context.SaveChanges();
        }
        
        public int ChangePassword(string CustomerCode, string newPassword, ulong? firstLogin = null)
        {
            if (firstLogin == null)
            {
                var changePasswordEmployee = new Employee
                {
                    EmployeeCode = CustomerCode,
                    EmployeePassword = PasswordEncryption.EncryptPassword(newPassword),
                    PasswordIssued = 0,
                    ChangeToken = null,
                    TokenExpired = null
                };
                _context.Employees.Attach(changePasswordEmployee);
                _context.Entry(changePasswordEmployee).Property(x => x.EmployeePassword).IsModified = true;
                _context.Entry(changePasswordEmployee).Property(x => x.PasswordIssued).IsModified = true;
                _context.Entry(changePasswordEmployee).Property(x => x.ChangeToken).IsModified = true;
                _context.Entry(changePasswordEmployee).Property(x => x.TokenExpired).IsModified = true;
                return _context.SaveChanges();
            }
            else
            {
                var changePasswordEmployee = new Employee
                {
                    EmployeeCode = CustomerCode,
                    EmployeePassword = PasswordEncryption.EncryptPassword(newPassword),
                    FirstLogin = 0
                };
                _context.Employees.Attach(changePasswordEmployee);
                _context.Entry(changePasswordEmployee).Property(x => x.EmployeePassword).IsModified = true;
                _context.Entry(changePasswordEmployee).Property(x => x.FirstLogin).IsModified = true;
                return _context.SaveChanges();
            }
        }

        public Employee GenerateEmployeeToken(string customerCode)
        {
            string token =CreateObject.randomTokenString();
            var tokenEmployee = new Employee { 
                EmployeeCode = customerCode, 
                FirstLogin = 0, 
                PasswordIssued = 1, 
                ChangeToken = token,
                TokenExpired = DateTime.Now.AddDays(7)};
            return tokenEmployee;
        }
    
        public string? VerifyConfirmPassword(string oldPassword, string newPassword, string confirmPassword)
        {
            string passwordRegex = "(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])[a-zA-Z0-9]{8,}";
            Regex rgPass = new Regex(passwordRegex);
            if (newPassword == "" || confirmPassword == "")
            {
                return "Password baru dan konfirmasi password tidak boleh kosong.";
            }
            else if (!confirmPassword.Equals(newPassword))
            {
                return "Konfirmasi password tidak sama dengan password baru.";
            }
            else if (PasswordEncryption.VerifyPassword(newPassword, oldPassword))
            {
                return "Password baru tidak boleh sama dengan password lama.";
            }
            else if (newPassword.Length < 8)
            {
                return "Password tidak boleh kurang dari 8 karakter";
            }
            else if (!Regex.IsMatch(newPassword, passwordRegex))
            {
                return "Password harus ada huruf kapital, huruf kecil, dan angka";
            }
            else return null;
        }

        public EmployeeVM? GetTokenOwner(string token)
        {
            var employee = _context.Employees.FirstOrDefault(x => x.ChangeToken.Equals(token));
            return employee != null ? new EmployeeVM
            {
                EmployeeCode = employee.EmployeeCode,
                EmployeeName = employee.EmployeeName
            } : null;
        }

        public RoundVM GetRoundDetails()
        {

            var currentDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd 00:00:00"));

            var roundDetail = new RoundVM();
            roundDetail.MonthList = new List<Object>();
            var roundList = _context.Rounds.ToList();
            var currentRound = roundList.FirstOrDefault(r => r.StartDate <= currentDate && r.EndDate >= currentDate);
            roundDetail.CurrentRoundId = currentRound.RoundId;
            roundDetail.CurrentRoundName = currentRound.RoundName;
            roundDetail.CompletedRounds = roundList.Where(r => r.EndDate <= currentDate).Count();
            roundDetail.TotalRounds = roundList.Count;
            var roundCount = 0;

            return roundDetail;
        }

        public EmployeeMainMenuVM? GetMainMenuData(string customerCode)
        {
            var result = new EmployeeMainMenuVM();
            result.Wholesellers = new List<WholesellerVM>();
            result.Standings = new List<WholesellerSaleVM>();
            var emp = _context.Employees.Find(customerCode);
            if (emp == null)
            {
                result = null;
            }
            else
            {
                result.EmployeeCode = customerCode;
                result.EmployeeName = emp.EmployeeName;
                result.RoleType = emp.RoleType;
                _context.Entry(emp).State = EntityState.Detached;
                var currentDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd 00:00:00"));

                var currentRound = _context.Rounds
                    .Where(r => r.StartDate <= currentDate)
                    .Where(r => r.EndDate >= currentDate)
                    .FirstOrDefault();

                int currentRoundId = currentRound.RoundId;
                _context.Entry(currentRound).State = EntityState.Detached;
                switch (result.RoleType)
                {
                    case "W":
                        result.RoleName = "Wholeseller";
                        var currentGroup = _context.Leaderboards.FirstOrDefault(l =>
                            l.WholesellerCode.Equals(result.EmployeeCode) &&
                            l.RoundId.Equals(currentRoundId));
                        if (currentGroup != null)
                        {
                            result.WholesellerDetail = new WholesellerSaleDetail
                            {
                                GroupName = currentGroup.GroupName,
                                BaselineStock = currentGroup.BaselineStock,
                                CurrentRank = currentGroup.Rank,
                                PointA = currentGroup.PointA,
                                PointB = currentGroup.PointB,
                                SalePoint = currentGroup.SalePoint
                            };

                            result.WholesellerDetail.LatestUpdate = _context.Leaderboards.AsNoTracking()
                                .Where(s => s.WholesellerCode.Equals(result.EmployeeCode))
                                .OrderByDescending(d => d.SaleDate.Date).First().SaleDate.Date;

                            var topSales = (from ld in _context.Leaderboards
                                            join em in _context.Employees
                                            on ld.WholesellerCode equals em.EmployeeCode
                                            where ld.GroupName == result.WholesellerDetail.GroupName &&
                                            ld.RoundId == currentRoundId
                                            select new WholesellerSaleVM
                                            {

                                                WholesellerName = em.EmployeeName,
                                                WholesellerArea = em.EmployeeArea,
                                                SalePoint = ld.SalePoint,
                                                Rank = ld.Rank

                                            }).OrderByDescending(s => s.SalePoint).ToList();
                            result.Standings = topSales;
                        }
                        break;
                    case "S":
                        result.RoleName = "Salesman";
                        var wholesellers = (from ws in _context.WholesellerMappings
                                            where ws.SalesmanCode == customerCode
                                            join em in _context.Employees
                                            on ws.WholesellerCode equals em.EmployeeCode
                                            join ld in (from ldround in _context.Leaderboards
                                                        where ldround.RoundId == currentRoundId
                                                        select new
                                                        {
                                                            WholesellerCode = ldround.WholesellerCode,
                                                            GroupName = ldround.GroupName
                                                        })
                                            on ws.WholesellerCode equals ld.WholesellerCode into wsld
                                            from subwsld in wsld.DefaultIfEmpty()
                                            select new WholesellerVM
                                            {
                                                WholesellerCode = ws.WholesellerCode,
                                                WholesellerName = em.EmployeeName,
                                                GroupName = subwsld.GroupName ?? ""
                                            }).ToList();
                        result.Wholesellers.AddRange(wholesellers);
                        break;
                    default:
                        result.RoleName = "Admin";
                        break;
                }
            }
            return result;
        }

        public IEnumerable<WholesellerSaleVM> GetLeaderboard(LeaderboardInputVM leaderboardInput)
        {
            var listSale = new List<WholesellerSaleVM>();
            var roundld = (from ld in _context.Leaderboards
                       join em in _context.Employees
                       on ld.WholesellerCode equals em.EmployeeCode into wsld
                       from subld in wsld.DefaultIfEmpty()
                       where ld.GroupName == leaderboardInput.GroupName &&
                       ld.RoundId == leaderboardInput.RoundId
                       select new WholesellerSaleVM
                       {
                           WholesellerCode = subld.EmployeeCode,
                           WholesellerName = subld.EmployeeName ?? string.Empty,
                           WholesellerArea = subld.EmployeeArea ?? string.Empty,
                           SalePoint = ld.SalePoint,
                           Rank = ld.Rank
                       }).OrderBy(s => s.Rank).ToList();
            listSale.AddRange(roundld);
            return listSale;
        }

        public int ClearForgotRequestOnLogin(string customerCode)
        {
            int result = 0;
            var emp = _context.Employees.Find(customerCode);
            if (emp.PasswordIssued == 1 || emp.ChangeToken != null)
            {
                emp.PasswordIssued = 0;
                emp.ChangeToken = null;
                _context.Entry(emp).State = EntityState.Modified;
                result = _context.SaveChanges();
                _context.Entry(emp).State = EntityState.Detached;
            }
            return result;
        }

        public int updateEmployeePhone(PhoneNumberVM phoneNumber)
        {
            var changePhoneNumberEmployee = new Employee
            {
                EmployeeCode = phoneNumber.CustomerCode,
                EmployeePhone = phoneNumber.EmployeePhone
            };
            _context.Employees.Attach(changePhoneNumberEmployee);
            _context.Entry(changePhoneNumberEmployee).Property(x => x.EmployeePhone).IsModified = true;
            return _context.SaveChanges();
        }

        public string? VerifyConfirmPhoneNumber(string phoneNUmber)
        {
            string passwordRegex = "^(^08)(\\d{3,4}-?){2}\\d{3,4}$";
            Regex rgPass = new Regex(passwordRegex);
            if (phoneNUmber == "")
            {
                return "Nomor Handphone Tidak Boleh Kosong.";
            }
            else if (!Regex.IsMatch(phoneNUmber, passwordRegex))
            {
                return "Nomor Handphone Harus Sesuai (Diawali 08,Tidak Ada Karakter, No Hp Valid,Dll)";
            }
            else return null;
        }
    }
}
