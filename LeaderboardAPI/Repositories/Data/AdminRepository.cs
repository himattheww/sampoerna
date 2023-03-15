using DocumentFormat.OpenXml.InkML;
using ExcelDataReader;
using LeaderboardAPI.CustomServices;
using LeaderboardAPI.Entities;
using LeaderboardAPI.Repositories.Interface;
using LeaderboardAPI.ViewModels;
using LeaderboardAPI.ViewModels.Input;
using LeaderboardAPI.ViewModels.Output;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.Xml;
using System.Data.SqlClient;
using Z.EntityFramework.Extensions;
using Irony.Parsing;
using Amazon.S3.Model;
using Amazon.S3;
using Amazon;
using Microsoft.AspNetCore.Mvc;

namespace LeaderboardAPI.Repositories.Data
{
    public class AdminRepository : IAdmin
    {
        private readonly LeaderboardContext _context;
        public string AWS_bucketName = "qa-ligamahakarya";
        public string prefix = "upload";
        private readonly IAmazonS3 _s3Client;
        public readonly IConfiguration _config;

        public AdminRepository(LeaderboardContext context, IConfiguration configuration)
        {
            IAmazonS3 s3Client;
            _context = context;
            _s3Client = new AmazonS3Client(configuration["AWS_KEY"], configuration["AWS_SECRET"], RegionEndpoint.APSoutheast1);
            _config = configuration;

        }

        public async Task InsertUserProfile(Employee employee)
        {
            List<Employee> employees = new();
            employees.Add(new Employee
            {
                EmployeeCode = employee.EmployeeCode,
                RoleType = employee.RoleType,
                EmployeeName = employee.EmployeeName,
                EmployeeEmail = employee.EmployeeEmail,
                EmployeePhone = employee.EmployeePhone,
                EmployeeArea = employee.EmployeeArea,
                EmployeePassword = employee.EmployeePassword
            });
            await _context.BulkInsertAsync(employees);
            await _context.BulkSaveChangesAsync();
        }

        public Employee GetEmployeeId(string employeeId)
        {
            return _context.Employees.Find(employeeId);
        }

        public async Task UpdateEmployee(Employee employee)
        {
            List<Employee> employees = new();
            employees.Add(new Employee
            {
                EmployeeCode = employee.EmployeeCode,
                RoleType = employee.RoleType,
                EmployeeName = employee.EmployeeName,
                EmployeeEmail = employee.EmployeeEmail,
                EmployeePhone = employee.EmployeePhone,
                EmployeeArea = employee.EmployeeArea,
                EmployeePassword = employee.EmployeePassword
            });
            await _context.BulkUpdateAsync(employees);
            await _context.BulkSaveChangesAsync();
        }

        public async Task InsertWholesellerMapping(WholesellerMapping wholeseller)
        {
            List<WholesellerMapping> wholesellers = new();
            wholesellers.Add(new WholesellerMapping
            {
                WholesellerCode = wholeseller.WholesellerCode,
                SalesmanCode = wholeseller.SalesmanCode
            });
            await _context.BulkInsertAsync(wholesellers);
            await _context.BulkSaveChangesAsync();
        }
        public WholesellerMapping GetWholesellerId(string wholesellerCode)
        {
            return _context.WholesellerMappings.Find(wholesellerCode);
        }

        public async Task UpdateWholesellerMapping(WholesellerMapping wholeseller)
        {
            List<WholesellerMapping> wholesellers = new();
            wholesellers.Add(new WholesellerMapping
            {
                WholesellerCode = wholeseller.WholesellerCode,
                SalesmanCode = wholeseller.SalesmanCode
            });
            await _context.BulkUpdateAsync(wholesellers);
            await _context.BulkSaveChangesAsync();
        }
        public Employee CreateUserProfile(DataRow row)
        {
            var encPass = "";
            string pass = row[6].ToString();
            if (pass.Length >=50)
            {
                encPass = pass;
            }
            else
            {
                encPass = PasswordEncryption.EncryptPassword(row[6].ToString());
            }

            var employee = new Employee
            {
                EmployeeCode = row[0].ToString(),
                RoleType = row[1].ToString(),
                EmployeeName = (row[2].ToString() != null && row[2].ToString() != "") ? row[2].ToString() : null,
                EmployeePhone = (row[3].ToString() != null && row[3].ToString() != "") ? row[3].ToString() : null,
                EmployeeArea = (row[4].ToString() != null && row[4].ToString() != "") ? row[4].ToString() : null,
                EmployeePassword = encPass

            };
            return employee;
        }

        public Leaderboard CreateLeaderBoard(DataRow row)
        {
            var newLeaderBoard = new Leaderboard
            {
                WholesellerCode = row[0].ToString(),
                RoundId = Convert.ToInt32(row[1]),
                GroupName = (row[2].ToString() != null && row[2].ToString() != "") ? row[2].ToString() : null,
                BaselineStock = (row[3] is DBNull) ? 0 : Convert.ToInt32(row[3]),
                Rank = (row[4] is DBNull) ? 0 : Convert.ToInt32(row[4]),
                PointA = (row[5] is DBNull) ? 0 : Convert.ToInt32(row[5]),
                PointB = (row[6] is DBNull) ? 0 : Convert.ToInt32(row[6]),
                SalePoint = (row[7] is DBNull) ? 0 : Convert.ToInt32(row[7]),
            };
            return newLeaderBoard;
        }

        public WholesellerMapping CreateWholesellerMapping(DataRow row)
        {
            return new WholesellerMapping
            {
                WholesellerCode = row[0].ToString(),
                SalesmanCode = row[5].ToString(),
            };
        }

        public async Task InsertLeaderBoard(Leaderboard leaderboard)
        {
            List<Leaderboard> leaderboards = new();
            leaderboards.Add(new Leaderboard
            {
                WholesellerCode = leaderboard.WholesellerCode,
                RoundId = leaderboard.RoundId,
                GroupName = leaderboard.GroupName,
                BaselineStock = leaderboard.BaselineStock,
                SalePoint = leaderboard.SalePoint,
                Rank = leaderboard.Rank,
                PointA = leaderboard.PointA,
                PointB = leaderboard.PointB
            });
            await _context.BulkInsertAsync(leaderboards);
            await _context.BulkSaveChangesAsync();
        }
        public DataTable? CreateDataTableFromDatabaseEmployees()
        {
            DataTable dtEmployee = new DataTable();
            dtEmployee.Columns.Add("user_code", typeof(string));
            dtEmployee.Columns.Add("role_type", typeof(string));
            dtEmployee.Columns.Add("user_name", typeof(string));
            dtEmployee.Columns.Add("user_email", typeof(string));
            dtEmployee.Columns.Add("phone_number", typeof(string));
            dtEmployee.Columns.Add("area", typeof(string));
            dtEmployee.Columns.Add("salesman_code", typeof(string));
            dtEmployee.Columns.Add("password", typeof(string));


            var query = (from emp in _context.Employees
                         join wm in _context.WholesellerMappings
                         on emp.EmployeeCode equals wm.WholesellerCode into empwm
                         from subempwm in empwm.DefaultIfEmpty()
                         select new
                         {
                             user_code = emp.EmployeeCode,
                             role_type = emp.RoleType,
                             user_name = emp.EmployeeName,
                             user_email = emp.EmployeeEmail,
                             phone_number = emp.EmployeePhone,
                             area = emp.EmployeeArea,
                             salesman_code = subempwm.SalesmanCode ?? string.Empty,
                             password = emp.EmployeePassword
                         }).ToList();

            foreach (var row in query)
            {
                DataRow dr = dtEmployee.NewRow();
                dr["user_code"] = row.user_code;
                dr["role_type"] = row.role_type;
                dr["user_name"] = row.user_name;
                dr["user_email"] = row.user_email;
                dr["phone_number"] = row.phone_number;
                dr["area"] = row.area;
                dr["salesman_code"] = row.salesman_code;
                dr["password"] = row.password;
                dtEmployee.Rows.Add(dr);
            }

            return dtEmployee;
        }

        public DataTable? CreateDataTableFromDatabaseLeaderBoard()
        {
            DataTable dtLeaderBoard = new DataTable();
            dtLeaderBoard.Columns.Add("wholeseller_code", typeof(string));
            dtLeaderBoard.Columns.Add("round_id", typeof(int));
            dtLeaderBoard.Columns.Add("group_name", typeof(string));
            dtLeaderBoard.Columns.Add("baseline_stock", typeof(int));
            dtLeaderBoard.Columns.Add("sale_date", typeof(DateTime));
            dtLeaderBoard.Columns.Add("sale_point", typeof(int));
            dtLeaderBoard.Columns.Add("rank", typeof(int));
            dtLeaderBoard.Columns.Add("point_tetap", typeof(int));
            dtLeaderBoard.Columns.Add("potensi_point", typeof(int));

            var query = (from ldb in _context.Leaderboards
                         select new
                         {
                             ldb.WholesellerCode,
                             ldb.RoundId,
                             ldb.GroupName,
                             ldb.BaselineStock,
                             ldb.SaleDate,
                             ldb.SalePoint,
                             ldb.Rank,
                             ldb.PointA,
                             ldb.PointB
                         }).ToList();

            foreach (var row in query)
            {
                DataRow dr = dtLeaderBoard.NewRow();
                dr["wholeseller_code"] = row.WholesellerCode;
                dr["round_id"] = row.RoundId;
                dr["group_name"] = row.GroupName;
                dr["baseline_stock"] = row.BaselineStock;
                dr["sale_date"] = row.SaleDate;
                dr["sale_point"] = row.SalePoint;
                dr["rank"] = row.Rank;
                dr["point_tetap"] = row.PointA;
                dr["potensi_point"] = row.PointB;
                dtLeaderBoard.Rows.Add(dr);
            }

            return dtLeaderBoard;
        }

        public int DeleteRoundId(int roundId)
        {
            _context.Database.ExecuteSqlRaw("DELETE FROM `leaderboards` WHERE round_id = " + roundId);
            return _context.SaveChanges();
        }

        public Round GetRoundId(int roundId)
        {
            return _context.Rounds.Find(roundId);
        }
        public int UpdateRound(Round round)
        {
            _context.Entry(round).State = EntityState.Modified;
            return _context.SaveChanges();
        }

        public IEnumerable<Round> GetAllRound()
        {
            return _context.Rounds.ToList();
        }

        public IEnumerable<EmployeeVM> GetAllEmployeesFromAdmin()
        {
            var result = _context.Employees.Select(e =>
                new EmployeeVM
                {
                    EmployeeCode = e.EmployeeCode,
                    EmployeeName = e.EmployeeName,
                    EmployeePhone = e.EmployeePhone,
                }).ToList();
            return result.Count() == 0 ? new List<EmployeeVM>() : result;
        }

        public IEnumerable<EmployeeVM> GetEmployeeFromAdminByName(string? employeeName)
        {
            var result = _context.Employees.AsQueryable();
            if(employeeName != null)
            {
                return result.Where(r =>
                    r.EmployeeName.StartsWith(employeeName))
                    .Select(e => new EmployeeVM
                    {
                        EmployeeCode = e.EmployeeCode,
                        EmployeeName = e.EmployeeName,
                        EmployeePhone = e.EmployeePhone,
                    }).ToList();
            }
            else return result.Select(e => new EmployeeVM
            {
                EmployeeCode = e.EmployeeCode,
                EmployeeName = e.EmployeeName,
                EmployeePhone = e.EmployeePhone,
            }).ToList();
        }

        public void DetachDuplicateEmployee(Employee dupEmployee)
        {
            _context.Entry(dupEmployee).State = EntityState.Deleted;
            _context.SaveChanges();
        }

        public void DetachDuplicateLeaderboard(Leaderboard leaderboard)
        {
            _context.Entry(leaderboard).State = EntityState.Detached;
            _context.SaveChanges();
        }

        public IEnumerable<UserProfile> GetEmployeesResult(List<string> resultList)
        {
            var result = (from emp in _context.Employees
                         join wm in _context.WholesellerMappings
                         on emp.EmployeeCode equals wm.WholesellerCode into empwm
                         from subempwm in empwm.DefaultIfEmpty()
                         where resultList.Contains(emp.EmployeeCode)
                         select new UserProfile
                         {
                             UserCode = emp.EmployeeCode,
                             RoleType = emp.RoleType,
                             UserName = emp.EmployeeName,
                             PhoneNumber = emp.EmployeePhone,
                             Area = emp.EmployeeArea,
                             SalesmanCode = subempwm.SalesmanCode ?? string.Empty
                         }).ToList();
            return result;
        }

        public IEnumerable<Leaderboard> GetLeaderboardsResult(List<string> resultList, int roundId)
        {
            var result = _context.Leaderboards.AsNoTracking().Where(ld =>
            ld.RoundId == roundId && resultList.Contains(ld.WholesellerCode)).Select(s => new Leaderboard
            {
                WholesellerCode = s.WholesellerCode,
                RoundId = s.RoundId,
                GroupName = s.GroupName,
                BaselineStock = s.BaselineStock,
                SaleDate = s.SaleDate,
                SalePoint = s.SalePoint,
                Rank = s.Rank,
                PointA = s.PointA,
                PointB = s.PointB
            }).ToList();
            return result;
        }

        public async Task CreateFileUpload(FileUpload fileUpload)
        {
            List<FileUpload> fileUploads = new();
            fileUploads.Add(new FileUpload
            {
                NameFile = fileUpload.NameFile,
                UploadDateTime = fileUpload.UploadDateTime,
                FinishUpload = fileUpload.FinishUpload,
                Status = fileUpload.Status,
                UserUpload = fileUpload.UserUpload
            });

            await _context.BulkInsertAsync(fileUploads);
            await _context.BulkSaveChangesAsync();
        }

        public async Task<byte[]> GetFileFromS3(string key)
        {
            var bucketExists = await _s3Client.DoesS3BucketExistAsync(_config["AWS_BUCKET"]);
            try
            {
                var s3Object = await _s3Client.GetObjectAsync(_config["AWS_BUCKET"], key);
                MemoryStream memoryStream = new MemoryStream();
                using (Stream responseStream = s3Object.ResponseStream)
                {
                    responseStream.CopyTo(memoryStream);
                }
                return memoryStream.ToArray();
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error : " + ex);
                return null;
            }
           
        }
        public async Task<IEnumerable<FileUploadVM>> GetAllFileUpload()
        {
            var allDataFileDownload = (from fup in _context.FileUploads
                                        select new FileUploadVM
                                        {
                                           Id= fup.Id,
                                           NameFile= fup.NameFile,
                                           UploadDateTime = fup.UploadDateTime,
                                           FinishUpload = fup.FinishUpload,
                                           Status = fup.Status,
                                           Data = null,
                                           UserUpload = fup.UserUpload
                                        }).OrderByDescending(s => s.UploadDateTime).ToList();
            
            foreach(var file in allDataFileDownload)
            {
                file.Data = null;
                string namafile = file.NameFile;
                var namaFile = await GetFileFromS3("upload/"+namafile);
                if(namaFile != null)
                {
                    file.Data = namaFile;
                }
                else
                {
                    continue;
                }
            }
            return allDataFileDownload;
        }

        public string GetEmployeeOutput(string customerCode)
        {
            EmployeeVM? employeeVM = null;
            var employee = _context.Employees.Select(s => new EmployeeVM
            {
                EmployeeCode = s.EmployeeCode,
                EmployeeName = s.EmployeeName
            }).FirstOrDefault(o => o.EmployeeCode == customerCode);
            if (employee != null)
            {
                employeeVM = employee;

                return employeeVM.EmployeeName;
            }
            else
            {
                return null;
            }
        }

        public async Task UploadFileToS3(Stream data, string file)
        {
            var bucketExists = await _s3Client.DoesS3BucketExistAsync(_config["AWS_BUCKET"]);
            var request = new PutObjectRequest()
            {
                BucketName = _config["AWS_BUCKET"],
                Key = $"{prefix?.TrimEnd('/')}/{file}",
                InputStream = data
            };
            request.Metadata.Add("Content-Type", "text/csv");
            await _s3Client.PutObjectAsync(request);
        }

    }
}
