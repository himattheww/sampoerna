using DocumentFormat.OpenXml.Presentation;
using DocumentFormat.OpenXml.Spreadsheet;
using ExcelDataReader.Log;
using Irony.Parsing;
using LeaderboardAPI.CustomServices;
using LeaderboardAPI.Entities;
using LeaderboardAPI.Repositories.Interface;
using LeaderboardAPI.ViewModels;
using LeaderboardAPI.ViewModels.Input;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Sockets;
using System.Security.Claims;
using System.Text.Json;

namespace LeaderboardAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployee _employee;
        private readonly IToken _token;
        public IConfiguration? _configuration;

        public EmployeesController(IEmployee employee, IToken token, IConfiguration configuration)
        {
            _employee = employee;
            _token = token;
            _configuration = configuration;
        }

        [HttpGet]
        [Route("id/{customerCode}")]
        [Authorize]
        public ActionResult GetEmployee(string customerCode)
        {
            var employeeResult = _employee.GetEmployeeOutput(customerCode);
            if (employeeResult == null)
            {
                return StatusCode(404, new
                {
                    statusCode = HttpStatusCode.NotFound,
                    message = "Data dengan Customer Code " + customerCode + " tidak ditemukan.",
                    data = ""
                });
            }
            else
            {
                return StatusCode(200, new
                {
                    statusCode = HttpStatusCode.OK,
                    message = "Data ditemukan.",
                    data = employeeResult
                });
            }
        }

        [HttpPut]
        [Route("forgot")]
        public ActionResult ForgotPassword(ForgotPasswordRequestVM forgotRequest)
        {
            var employee = _employee.GetEmployeeOutput(forgotRequest.CustomerCode);
            if (employee != null)
            {
                if (employee.EmployeePhone.Equals(forgotRequest.PhoneNumber))
                {
                    var employeeIssued = _employee.GenerateEmployeeToken(forgotRequest.CustomerCode);
                    var forgotPassResult = _employee.SetEmployeeToken(employeeIssued);
                    var url = CreateObject.getUrlResetPassword(employeeIssued.EmployeeCode, employeeIssued.ChangeToken);

                    return StatusCode(200, new
                    {
                        statusCode = HttpStatusCode.OK,
                        message = "berhasil lupa password",
                        data = new
                        {
                            token = employeeIssued.ChangeToken,
                            TokenExpired = employeeIssued.TokenExpired,
                            URL = url.ToString()
                        }
                    });
                }
                else
                {
                    return StatusCode(400, new
                    {
                        statusCode = HttpStatusCode.BadRequest,
                        message = "nomor HP salah",
                        data = ""

                    });
                }
            }
            else return StatusCode(404, new
            {
                statusCode = HttpStatusCode.NotFound,
                message = "Tidak ada data dengan Customer Code " + forgotRequest.CustomerCode,
                data = ""
            });
        }

        [HttpPut]
        [Route("resetAdmin")]
        [Authorize(Roles = "A")]
        public ActionResult ResetAdmin(string CustomerCode)
        {
            var employee = _employee.GetEmployeeOutput(CustomerCode);
            if (employee != null)
            {
                var employeeIssued = _employee.GenerateEmployeeToken(CustomerCode);
                var resetPassword = _employee.SetEmployeeToken(employeeIssued);
                var url = CreateObject.getUrlResetPassword(employeeIssued.EmployeeCode, employeeIssued.ChangeToken);

                return StatusCode(200, new
                {
                    statusCode = HttpStatusCode.OK,
                    message = "Berhasil Membuat Link Reset Password",
                    data = new
                    {
                        token = employeeIssued.ChangeToken,
                        TokenExpired = employeeIssued.TokenExpired,
                        URL = url.ToString()
                    }
                });
            }
            else return StatusCode(404, new
            {
                statusCode = HttpStatusCode.NotFound,
                message = "Tidak ada data dengan Customer Code " + CustomerCode,
                data = ""
            });
        }

        [HttpPut]
        [Route("reset")]
        public ActionResult ResetPassword(ResetPasswordVM resetPassword)
        {
            int changeResult = 0;
            var firstLogin = false;
            var correctToken = false;
            var employeeIssued = _employee.FindEmployeeFullData(resetPassword.CustomerCode);
            var verifyConfirmPassword = _employee.VerifyConfirmPassword(employeeIssued.EmployeePassword, resetPassword.NewPassword, resetPassword.ConfirmPassword);
            if (employeeIssued == null)
            {
                return StatusCode(404, new
                {
                    statusCode = HttpStatusCode.NotFound,
                    message = "Tidak ada data dengan Customer Code " + resetPassword.CustomerCode,
                    data = ""
                });
            }
            else if (resetPassword.ChangeToken == null)
            {
                if (employeeIssued.FirstLogin != 1)
                {
                    return StatusCode(400, new
                    {
                        statusCode = HttpStatusCode.BadRequest,
                        message = "Anda tidak bisa reset password tanpa token karena sudah pernah login",
                        data = ""
                    });
                }
                else if (verifyConfirmPassword != null)
                {
                    return StatusCode(400, new
                    {
                        statusCode = HttpStatusCode.BadRequest,
                        message = verifyConfirmPassword,
                        data = ""
                    });
                }
                else
                {
                    changeResult = _employee.ChangePassword(resetPassword.CustomerCode, resetPassword.NewPassword, employeeIssued.PasswordIssued);
                }
            }
            else
            {
                if (employeeIssued.PasswordIssued != 1)
                {
                    return StatusCode(400, new
                    {
                        statusCode = HttpStatusCode.BadRequest,
                        message = "Tidak bisa mengganti password karena user ini tidak meminta reset password/forget password",
                        data = ""
                    });
                }
                else if (!employeeIssued.ChangeToken.Equals(resetPassword.ChangeToken) && employeeIssued.TokenExpired >= DateTime.Now)
                {
                    return StatusCode(400, new
                    {
                        statusCode = HttpStatusCode.BadRequest,
                        message = "Link Token Expired, Ulang Forgot Password",
                        data = ""
                    });
                }
                else if (verifyConfirmPassword != null)
                {
                    return StatusCode(400, new
                    {
                        statusCode = HttpStatusCode.BadRequest,
                        message = verifyConfirmPassword,
                        data = ""
                    });
                }
                else
                {
                    changeResult = _employee.ChangePassword(resetPassword.CustomerCode, resetPassword.NewPassword);
                }
            }
            return StatusCode(200, new
            {
                statusCode = HttpStatusCode.OK,
                message = "Berhasil mengubah password",
                data = new
                {
                    result = changeResult
                }
            });
        }

        [HttpPost]
        [Route("login")]
        public ActionResult EmployeeLogin(UserLoginVM login)
        {
            if (login.Password == "" || login.Password == null)
            {
                return StatusCode(400, new
                {
                    statusCode = HttpStatusCode.BadRequest,
                    message = "Password tidak boleh kosong",
                    data = ""
                });
                //BadRequest("Password tidak boleh kosong");
            }
            var employee = _employee.FindEmployeeFullData(login.CustomerCode);
            if (employee != null)
            {
                var passwordMatch = PasswordEncryption.VerifyPassword(login.Password, employee.EmployeePassword);
                if (passwordMatch)
                {
                    if (employee.FirstLogin == 1)
                    {
                        return StatusCode(200, new
                        {
                            statusCode = HttpStatusCode.OK,
                            message = "Ganti password sebelum masuk kembali",
                            data = new
                            {
                                firstLogin = employee.FirstLogin == 1
                            }
                        });
                    }
                    else
                    {
                        if (!employee.EmployeePhone.Trim().Equals(""))
                        {
                            // string sessionCode = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
                            var employeeResult = _employee.GetEmployeeOutput(login.CustomerCode);
                            var clearForgotRequest = _employee.ClearForgotRequestOnLogin(employeeResult.EmployeeCode);
                            var role = "";
                            switch (employeeResult.EmployeeRole)
                            {
                                case "A":
                                    role = "Admin";
                                    break;
                                case "W":
                                    role = "Wholesaler";
                                    break;
                                case "S":
                                    role = "Salesman";
                                    break;
                                default:
                                    break;
                            }
                            var mainData = new
                            {
                                EmployeeCode = employeeResult.EmployeeCode,
                                EmployeeRole = employeeResult.EmployeeRole,
                                RoleName = role,
                                EmployeeName = employeeResult.EmployeeName,
                                GroupName = employeeResult.GroupName
                            };
                            var token = _token.GenerateJwtToken(login.CustomerCode, mainData.EmployeeRole);

                            //Console.WriteLine("Succes Login"+ login.CustomerCode);

                            return StatusCode(200, new
                            {
                                statusCode = HttpStatusCode.OK,
                                message = "Berhasil Masuk",
                                data = new
                                {
                                    firstLogin = employee.FirstLogin == 1,
                                    employeePhone = false,
                                    token = token,
                                    expiration = DateTime.Now.AddMinutes(20),
                                    mainData = mainData,
                                    clearForgotResult = clearForgotRequest
                                }
                            });
                        }
                        else
                        {
                            return StatusCode(200, new
                            {
                                statusCode = HttpStatusCode.OK,
                                message = "Update No HP sebelum masuk kembali",
                                data = new
                                {
                                    firstLogin = employee.FirstLogin == 1,
                                    employeePhone = true
                                }
                            });
                        }
                    }
                }
                else
                {
                    return StatusCode(400, new
                    {
                        statusCode = HttpStatusCode.BadRequest,
                        message = "password salah",
                        data = ""
                    });
                }
            }
            else
            {
                return StatusCode(404, new
                {
                    statusCode = HttpStatusCode.NotFound,
                    message = "Akun tidak ditemukan",
                    data = ""
                });
            }

        }

        [HttpPut]
        [Route("updatePhoneNumber")]
        public ActionResult UpdatePhoneNumber(PhoneNumberVM employee)
        {
            var nohp = employee.EmployeePhone.Replace(" ", "");
            var employeeIssued = _employee.FindEmployeeFullData(employee.CustomerCode);
            var verifyNoHp = _employee.VerifyConfirmPhoneNumber(nohp);

            if(employeeIssued == null)
            {
                return StatusCode(404, new
                {
                    statusCode = HttpStatusCode.NotFound,
                    message = "Tidak ada data dengan Customer Code " + employee.CustomerCode,
                    data = ""
                });
            }
            else
            {
                if(verifyNoHp == null)
                {
                    var employeeUpdate = _employee.updateEmployeePhone(employee);
                    if(employeeUpdate >= 1)
                    {
                        return StatusCode(200, new
                        {
                            statusCode = HttpStatusCode.OK,
                            message = "Berhasil Update Phone Number",
                            data = ""
                        });
                    }
                    else return StatusCode(404, new
                    {
                        statusCode = HttpStatusCode.NotFound,
                        message = "Tidak Berhasil Update Phone Number ",
                        data = ""
                    });
                }else
                {
                    return StatusCode(400, new
                    {
                        statusCode = HttpStatusCode.BadRequest,
                        message = verifyNoHp,
                        data = ""
                    });
                }
            }
           
        }

        [HttpGet]
        [Route("getRoundDetails")]
        [Authorize(Roles = "W,S")]
        public ActionResult GetRoundDetails()
        {
            var roundDetails = _employee.GetRoundDetails();
            return roundDetails != null ?
                StatusCode(200, new
                {
                    statusCode = HttpStatusCode.OK,
                    message = "Data ditemukan",
                    data = roundDetails
                })
                :
                StatusCode(404, new
                {
                    statusCode = HttpStatusCode.OK,
                    message = "Data tidak ada",
                    data = ""
                });
        }

        [HttpGet]
        [Route("main")]
        [Authorize(Roles = "W,S")]
        public ActionResult GetEmployeeMainData(string customerCode)
        {
            var result = _employee.GetMainMenuData(customerCode);
            if (result == null)
            {
                return StatusCode(404, new
                {
                    statusCode = HttpStatusCode.NotFound,
                    message = "Data dengan code " + customerCode + " tidak ditemukan",
                    data = ""
                });
            }
            else return StatusCode(200, new
            {
                statusCode = HttpStatusCode.OK,
                message = "Data ditemukan",
                data = result
            });
        }

        [HttpPost]
        [Route("leaderboards")]
        [Authorize(Roles = "W,S")]
        public ActionResult GetLeaderboard(LeaderboardInputVM leaderboardInput)
        {
            var result = _employee.GetLeaderboard(leaderboardInput);
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
                    message = "Data ditemukan",
                    data = result
                });
        }

    }
}
