
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Drawing;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Vml;
using ExcelDataReader;
using ExcelDataReader.Log;
using LeaderboardAPI.CustomServices;
using LeaderboardAPI.Entities;
using LeaderboardAPI.Repositories.Interface;
using LeaderboardAPI.ViewModels;
using LeaderboardAPI.ViewModels.Input;
using LeaderboardAPI.ViewModels.Output;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Hosting.Internal;
using Newtonsoft.Json;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Security;
using System.Security.Claims;
using System.Text.Json.Serialization;
using System.Xml;
using Round = LeaderboardAPI.Entities.Round;

namespace LeaderboardAPI.Controllers
{
    [Authorize(Roles = "A")]
    [Route("api/[controller]")]
    [ApiController]
    public class AdminsController : ControllerBase
    {
        private readonly IAdmin _admin;
        private readonly IWebHostEnvironment _hostEnvironment;

        public AdminsController(IAdmin admin, IWebHostEnvironment hostEnvironment)
        {
            _admin = admin;
            _hostEnvironment = hostEnvironment;
        }

        [HttpPut]
        [Route("Round/update")]

        public ActionResult UpdateRound(Round round)
        {
            var updateRound = _admin.UpdateRound(round);
            if (updateRound >= 1)
            {
                return StatusCode(200, new
                {
                    statusCode = HttpStatusCode.OK,
                    message = "Berhasil Update Data " + round.RoundId,
                    data = new
                    {
                        updateRound = updateRound
                    }
                });
            }
            else
            {
                return StatusCode(500, new
                {
                    statusCode = HttpStatusCode.InternalServerError,
                    message = "Gagal Update Data " + round.RoundId,
                    data = ""
                });
            }
        }

        [HttpGet]
        [Route("Round/")]
        public ActionResult GetAllRound()
        {
            var data = _admin.GetAllRound();
            if (data != null)
            {
                return StatusCode(200, new
                {
                    statusCode = HttpStatusCode.OK,
                    message = $"{data.Count()} Record Data",
                    data = data
                });
            }
            else
            {
                return StatusCode(404, new
                {
                    statusCode = HttpStatusCode.NotFound,
                    message = "Data Round Kosong",
                    data = ""
                });
            }
        }

        [HttpGet]
        [Route("Round/{roundId}")]
        public ActionResult GetRoundId(int roundId)
        {
            var data = _admin.GetRoundId(roundId);
            if (data != null)
            {
                return StatusCode(200, new
                {
                    statusCode = HttpStatusCode.OK,
                    message = $"Data RoundId " + roundId + " Ditemukan",
                    data = data
                });
            }
            else
            {
                return StatusCode(404, new
                {
                    statusCode = HttpStatusCode.NotFound,
                    message = $"Data RoundId " + roundId + " Tidak Ditemukan",
                    data = ""
                });
            }
        }

        [HttpGet]
        [Route("userList")]
        public ActionResult GetEmployeeListFromAdmin(string? employeeName)
        {
            var result = employeeName == null ?
                _admin.GetAllEmployeesFromAdmin()
                :
                _admin.GetEmployeeFromAdminByName(employeeName);
            return result.Count() == 0 ?
                StatusCode(404, new
                {
                    statusCode = HttpStatusCode.NotFound,
                    message = "Tidak ada data",
                    data = ""
                })
                :
                StatusCode(200, new
                {
                    statusCode = HttpStatusCode.OK,
                    message = result.Count() + " Data ditemukan",
                    data = result
                });
        }

        [HttpGet]
        [Route("UserProfileAll")]
        public ActionResult GetDataEmployeeAll()
        {
            DataTable dt = _admin.CreateDataTableFromDatabaseEmployees();
            var result = CreateObject.DataTableCSVToBinaryAll(dt);
            return result == null ?
                StatusCode(404, new
                {
                    statusCode = HttpStatusCode.NotFound,
                    message = "Tidak ada data",
                    data = ""
                })
                :
                StatusCode(200, new
                {
                    statusCode = HttpStatusCode.OK,
                    message = "Berhasil Download Data User Profile",
                    data = new
                    {
                        namaFile = "LeaderBoard Data_" + DateTime.Now.ToString("yyyy-MM-dd"),
                        data = result
                    }
                });
        }

        [HttpGet]
        [Route("LeaderBoardAll")]
        public ActionResult GetDataLeaderBoardAll()
        {
            DataTable dt = _admin.CreateDataTableFromDatabaseLeaderBoard();
            var result = CreateObject.DataTableCSVToBinaryAll(dt);
            return result == null ?
                StatusCode(404, new
                {
                    statusCode = HttpStatusCode.NotFound,
                    message = "Tidak ada data",
                    data = ""
                })
                :
                StatusCode(200, new
                {
                    statusCode = HttpStatusCode.OK,
                    message = "Berhasil Download Data LeaderBoard",
                    data = new
                    {
                        namaFile = "LeaderBoard Data_" + DateTime.Now.ToString("yyyy-MM-dd"),
                        data = result
                    }
                });
        }

        [HttpPost]
        [Route("file/upload")]
        [Authorize(Roles = "A")]
        public IActionResult CreateUploadFile(IFormFile file)
        {
            var claimIssued = HttpContext.Request.Headers.Authorization.ToString().Split(' ')[1];
            var jwt = new JwtSecurityTokenHandler().ReadJwtToken(claimIssued);
            string userCode = jwt.Claims.First(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            string user = _admin.GetEmployeeOutput(userCode);

            switch (!file.FileName.EndsWith(".csv"))
            {
                case true:
                    return StatusCode(400, new
                    {
                        statuscode = HttpStatusCode.BadRequest,
                        message = "Tidak Menerima Selain File .CSV",
                        data = file.FileName
                    });
                default:
                    break;
            }

            var dtUpload = CreateObject.CSVtoDataTable(file);

            if (dtUpload == null)
            {
                return StatusCode(400, new
                {
                    statusCode = HttpStatusCode.BadRequest,
                    message = "File Yang Diupload Tidak Ada",
                    data = ""
                });
            }
            else if (dtUpload.Rows.Count == 0)
            {
                return StatusCode(400, new
                {
                    statusCode = HttpStatusCode.BadRequest,
                    message = "File Kosong Atau Tidak Sesuai Template",
                    data = ""
                });
            }
            else
            {
                var userProfileHeader = new List<string> {
                    "user_code",
                    "role_type",
                    "user_name",
                    "phone_number",
                    "area",
                    "salesman_code",
                    "password"
                };
                var leaderboardHeader = new List<string> {
                    "wholeseller_code",
                    "round_id",
                    "group_name",
                    "baseline_stock",
                    "rank",
                    "point_tetap",
                    "potensi_point",
                    "total_point",
                };

                var inputHeader = new List<string>();
                foreach (DataColumn clm in dtUpload.Columns)
                {
                    inputHeader.Add(clm.ColumnName.Contains("Column") ? "" : clm.ColumnName.ToLower());
                }
                var headerType = CreateObject.CreateTableType(inputHeader, userProfileHeader, leaderboardHeader);
                var resultFileName = file.FileName;

                if (headerType == null)
                {
                    return StatusCode(400, new
                    {
                        statusCode = HttpStatusCode.BadRequest,
                        message = "File Tidak Sesuai Template",
                        data = " "
                    });
                }
                else
                {
                    try
                    {
                        return StatusCode(200, new
                        {
                            statusCode = HttpStatusCode.OK,
                            message = "File " + resultFileName + " Sudah Diterima, Sedang Proses Upload Data",
                            data = " "
                        });
                    }
                    finally
                    {
                        Response.OnCompleted(async () =>
                        {
                            await InsertDatabase(headerType, dtUpload, resultFileName, user);
                            
                        });
                    }
                }
            }
        }

        [HttpGet]
        [Route("file/download")]
        public ActionResult GetAllUGetAllFileUpload()
        {
            var data = _admin.GetAllFileUpload();
            if (data != null)
            {
                return StatusCode(200, new
                {
                    statusCode = HttpStatusCode.OK,
                    message = $"All Data",
                    data = data
                });
            }
            else
            {
                return StatusCode(404, new
                {
                    statusCode = HttpStatusCode.NotFound,
                    message = "Data Round Kosong",
                    data = ""
                });
            }
        }

        private async Task InsertDatabase(string headerType, DataTable dtUpload, string resultFileName, string user)
        {
            var rowIndex = 0;
            var keyList = new List<string>();
            var dateUpload = DateTime.Now;
            //var errorList = new List<string>();

            DataTable dtStatusUploadLdb = new DataTable();
            dtStatusUploadLdb.Columns.Add("STATUS", typeof(string));
            dtStatusUploadLdb.Columns.Add("KETERANGAN", typeof(string));

            DataTable dtStatusUploadUsp = new DataTable();
            dtStatusUploadUsp.Columns.Add("PASSWORD", typeof(string));
            dtStatusUploadUsp.Columns.Add("STATUS", typeof(string));
            dtStatusUploadUsp.Columns.Add("KETERANGAN", typeof(string));

            switch (headerType)
            {
                case "userProfile":
                    var wholesellerCreated = 0;
                    var employeeCreated = 0;
                    foreach (DataRow row in dtUpload.Rows)
                    {
                        rowIndex++;
                        try
                        {
                            var newEmployee = _admin.CreateUserProfile(row);
                            var cekDataUserProfile = _admin.GetEmployeeId(newEmployee.EmployeeCode) == null;
                            if (cekDataUserProfile)
                            {
                                //Insert User Profile
                                await _admin.InsertUserProfile(newEmployee);
                                employeeCreated++;
                            }
                            else
                            {
                                //Update User Profile
                                await _admin.UpdateEmployee(newEmployee);
                            }

                            keyList.Add(newEmployee.EmployeeCode);

                            if (newEmployee.RoleType == "W")
                            {
                                var newWholeseller = _admin.CreateWholesellerMapping(row);

                                var cekDataWholeseller = _admin.GetWholesellerId(newEmployee.EmployeeCode) == null;
                                if (cekDataWholeseller)
                                {
                                    //insert wholeseller
                                    await _admin.InsertWholesellerMapping(newWholeseller);
                                }
                                else
                                {
                                    //update wholeseller
                                    await _admin.UpdateWholesellerMapping(newWholeseller);
                                }
                                wholesellerCreated++;
                            }
                            dtStatusUploadUsp.Rows.Add(newEmployee.EmployeePassword,"Succes Upload", "");
                        }
                        catch (Exception ex)
                        {
                            var errorIndex = rowIndex + 2;
                            if (ex.InnerException != null)
                            {
                                dtStatusUploadUsp.Rows.Add(row[6].ToString(), "Gagal Upload", ex.InnerException.Message);
                                //errorList.Add("error at Excel row " + errorIndex + ". Message: " + ex.InnerException.Message);
                            }
                            else
                            {
                                dtStatusUploadUsp.Rows.Add(row[6].ToString(), "Gagal Upload", ex.Message);
                                //errorList.Add("error at Excel row " + errorIndex + ". Message: " + ex.Message);
                            }
                            continue;
                        }
                    }

                    //var employeesResult = _admin.GetEmployeesResult(keyList);
                    //DataTable dtEmployeeResult = CreateObject.ListToDataTable(employeesResult.ToList());

                    DataTable datamergeEmployee = CreateObject.MergeData(dtUpload, dtStatusUploadUsp,headerType);
                    var streamCsv = CreateObject.DataTableCSVToBinary(datamergeEmployee);
                    resultFileName = resultFileName.Replace(".csv", "_") + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".csv";

                    //Insert Ke Database File Upload
                    var newDataFileUserProfile = new FileUpload
                    {
                        NameFile = resultFileName,
                        UploadDateTime = dateUpload,
                        FinishUpload = DateTime.Now,
                        Status = "DONE",
                        UserUpload = user
                    };
                    await _admin.CreateFileUpload(newDataFileUserProfile);
                    await _admin.UploadFileToS3(streamCsv, resultFileName);
                    break;

                case "leaderboard":
                    var newLeaderboardData = 0;
                    var getRoundId = dtUpload.Rows[0][1];
                    var roundExist = _admin.GetRoundId(Convert.ToInt32(getRoundId)) != null;

                    _admin.DeleteRoundId(Convert.ToInt32(getRoundId));

                    foreach (DataRow row in dtUpload.Rows)
                    {
                        try
                        {
                            if (roundExist)
                            {
                                rowIndex++;
                                if (row[1].Equals(getRoundId))
                                {
                                    var dataLeaderBoard = _admin.CreateLeaderBoard(row);
                                    //insert 
                                    await _admin.InsertLeaderBoard(dataLeaderBoard);
                                    newLeaderboardData++;
                                    //keyList.Add(dataLeaderBoard.WholesellerCode);
                                    dtStatusUploadLdb.Rows.Add("Succes Upload", "");
                                }
                                else{
                                    dtStatusUploadLdb.Rows.Add("Gagal Upload", "RoundId Tidak Sama Dengan RoundId Row Pertama");
                                    continue; }
                            }
                            else {
                                var dataLeaderBoard = _admin.CreateLeaderBoard(row);
                                dtStatusUploadLdb.Rows.Add("Gagal Upload", "RoundId Tidak Sesuai");
                                continue; }
                        }
                        catch (Exception ex)
                        {
                            var errorIndex = rowIndex + 2;
                            //_admin.DetachDuplicateLeaderboard(_admin.CreateLeaderBoard(row));
                            if (ex.InnerException != null)
                            {
                                dtStatusUploadLdb.Rows.Add("Gagal Upload", ex.InnerException.Message);
                                //errorList.Add("error at Excel row " + errorIndex + ". Message: " + ex.InnerException.Message);
                            }
                            else
                            {
                                dtStatusUploadLdb.Rows.Add("Gagal Upload", ex.Message);
                                //errorList.Add("error at Excel row " + errorIndex + ". Message: " + ex.Message);
                            }
                            continue;
                        }
                    }

                    //var leaderboardsResult = _admin.GetLeaderboardsResult(keyList, Convert.ToInt32(getRoundId));
                    //DataTable dtLeaderboardsResult = CreateObject.ListToDataTable(leaderboardsResult.ToList());

                    DataTable datamergeLeaderboard = CreateObject.MergeData(dtUpload, dtStatusUploadLdb,headerType);
                    var leaderboardByte = CreateObject.DataTableCSVToBinary(datamergeLeaderboard);
                    resultFileName = resultFileName.Replace(".csv", "_") + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".csv";

                    //Insert Ke Database File Upload
                    var newDataFileLeaderboard = new FileUpload
                    {
                        NameFile = resultFileName,
                        UploadDateTime = dateUpload,
                        FinishUpload = DateTime.Now,
                        Status = "DONE",
                        UserUpload = user
                    };
                    await _admin.CreateFileUpload(newDataFileLeaderboard);
                    await _admin.UploadFileToS3(leaderboardByte, resultFileName);
                    break;
            }
        }
    }
}
