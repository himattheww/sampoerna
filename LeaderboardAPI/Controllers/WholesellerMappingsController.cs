using ExcelDataReader;
using LeaderboardAPI.Entities;
using LeaderboardAPI.Repositories.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System.Data;
using System.Net;
using System.Reflection.PortableExecutable;

namespace LeaderboardAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WholesellerMappingsController : ControllerBase
    {
        // private readonly LeaderboardContext _context;
        private readonly IWholesellerMapping _wholesellerMapping;
        private readonly IWebHostEnvironment _hostEnvironment;

        public WholesellerMappingsController(IWholesellerMapping wholesellerMapping, IWebHostEnvironment hostEnvironment)
        {
            _wholesellerMapping = wholesellerMapping;
            _hostEnvironment = hostEnvironment;
        }


        [HttpGet]
        public ActionResult Get()
        {
            var wholesellerResult = _wholesellerMapping.Get();
            return Ok(wholesellerResult);
        }

        [HttpPost]
        public ActionResult Upload(IFormFile file)
        {
            try {
                List<WholesellerMapping> wholesellerMappings = new List<WholesellerMapping>();
                if (file == null)
                {
                    throw new Exception("File tidak diunggah...");

                }

                string dir = Path.Combine(_hostEnvironment.ContentRootPath, "reports");

                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }

                string dataFileName = Path.GetFileName(file.FileName);
                string extension = Path.GetExtension(dataFileName);
                string[] allowedExtsnions = new string[] { ".xls", ".xlsx" };

                if (!allowedExtsnions.Contains(extension))
                {
                    throw new Exception("Jenis file tidak sesuai...");
                }

                string saveToPath = Path.Combine(dir, dataFileName);

                using (FileStream stream = new FileStream(saveToPath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }

                using (var stream = new FileStream(saveToPath, FileMode.Open, FileAccess.Read))
                {
                    IExcelDataReader reader;
                    if (extension == ".xls")
                    {
                        reader = ExcelReaderFactory.CreateBinaryReader(stream);
                    }
                    else
                    {
                        reader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                    }

                    DataSet ds = new DataSet();
                    ds = reader.AsDataSet();
                    reader.Close();

                    if (ds != null && ds.Tables.Count > 0)
                    {
                        DataTable mapping = ds.Tables[0];
                        foreach (DataRow row in mapping.Rows)
                        {
                            var newMapping = new WholesellerMapping
                            {
                                WholesellerCode = row[0].ToString(),
                                SalesmanCode = row[1].ToString(),
                            };
                            wholesellerMappings.Add(newMapping);
                        }
                    }
                    var uploadResult = _wholesellerMapping.UploadMapping(wholesellerMappings);
                    return StatusCode(200, new
                    {
                        statusCode = HttpStatusCode.OK,
                        message = "berhasil upload",
                        result = uploadResult,
                        output = wholesellerMappings
                    });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
    }
}
