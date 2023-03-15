using ClosedXML.Excel;
using ExcelDataReader;
using LeaderboardAPI.Entities;
using System.Data;
using System.Net.Mail;
using System.Net;
using System.Reflection;
using LeaderboardAPI.Repositories.Interface;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Security.Cryptography;
using LeaderboardAPI.ViewModels.Output;
using System.IO;
using System.Text;
using System.Net.Security;
using Amazon.S3;
using Amazon.S3.Transfer;
using Amazon;
using Amazon.S3.Model;

namespace LeaderboardAPI.CustomServices
{
    public static class CreateObject
    {

        public static DataTable? CreateDataTableFromFile(IFormFile file)
        {
            DataTable dtResult = null;
            DataSet dsExcel = new DataSet();
            IExcelDataReader reader = null;
            Stream fileStream = null;
            if (file == null)
            {

            }
            else
            {
                fileStream = file.OpenReadStream();
                var fileExtension = file.FileName.Split('.')[1];
                if (fileExtension == "xlsx")
                {
                    reader = ExcelReaderFactory.CreateOpenXmlReader(fileStream);
                }
                else
                {
                    return dtResult;
                }
                dsExcel = reader.AsDataSet();
                reader.Close();
                if (dsExcel != null && dsExcel.Tables.Count > 0)
                {
                    dtResult = dsExcel.Tables[0];
                    if (dtResult.Rows.Count > 0)
                    {
                        var columnArray = dtResult.Rows[1].ItemArray;
                        dtResult.Rows.Remove(dtResult.Rows[0]);
                        dtResult.Rows.Remove(dtResult.Rows[0]);
                        foreach (DataColumn clm in dtResult.Columns)
                        {
                            try
                            {
                                clm.ColumnName = columnArray[clm.Ordinal].ToString().ToUpper();
                            }
                            catch
                            {
                                continue;
                            }
                        }
                    }
                }
            }
            return dtResult;
        }

        public static DataTable? CSVtoDataTable(IFormFile file)
        {
            DataTable dt = new DataTable();
            Stream fileStream = null;
            fileStream = file.OpenReadStream();
            using (StreamReader sr = new StreamReader(fileStream))
            {
                string[] typeData = sr.ReadLine().Split(",");
                string[] headers = sr.ReadLine().Split(",");
                foreach (string header in headers)
                {
                    try
                    {
                        dt.Columns.Add(header);
                    }
                    catch { }
                }
                while (!sr.EndOfStream)
                {
                    string[] rows = sr.ReadLine().Split(",");
                    DataRow dr = dt.NewRow();
                    for (int i = 0; i < headers.Length; i++)
                    {
                        dr[i] = rows[i];
                    }
                    dt.Rows.Add(dr);
                }

            }
            return dt;
        }


        public static string? CreateTableType(List<string> inHeader, List<string> userProfileHeader, List<string> leaderboardHeader)
        {
            if (inHeader.Count() >= 8)
            {
                return inHeader.Take(8).SequenceEqual(leaderboardHeader) ?
                    "leaderboard" :
                    inHeader.Take(7).SequenceEqual(userProfileHeader) ?
                    "userProfile" :
                    null;
            }
            else if (inHeader.Count() >= 7)
            {
                return inHeader.Take(7).SequenceEqual(userProfileHeader) ?
                    "userProfile" :
                    null;
            }
            else return null;
        }

        public static DataTable ListToDataTable<T>(List<T> items)
        {
            DataTable dt = new DataTable(typeof(T).Name);
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                dt.Columns.Add(prop.Name);
            }
            foreach (T item in items)
            {
                var value = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    value[i] = Props[i].GetValue(item, null);
                }
                dt.Rows.Add(value);
            }
            return dt;
        }

        public static byte[] DataTableToBinary(DataTable dt, string headerType)
        {
            DataSet ds = new DataSet();
            DataTable dtCopy = dt.Copy();
            ds.Tables.Add(dtCopy);
            var workbook = new XLWorkbook();
            workbook.Worksheets.Add(ds);

            var ws = workbook.Worksheet(1);
            if (headerType == "userProfile")
            {
                ws.Cell(1, 1).Value = "USER_CODE";
                ws.Cell(1, 2).Value = "ROLE_TYPE";
                ws.Cell(1, 3).Value = "USER_NAME";
                ws.Cell(1, 4).Value = "PHONE_NUMBER";
                ws.Cell(1, 5).Value = "AREA";
                ws.Cell(1, 6).Value = "SALESMAN_CODE";
                ws.Cell(1, 7).Value = "PASSWORD";
                ws.Cell(1, 8).Value = "STATUS";
                ws.Cell(1, 9).Value = "KETERANGAN";
            }
            else
            {
                ws.Cell(1, 1).Value = "WHOLESELLER_CODE";
                ws.Cell(1, 2).Value = "ROUND_ID";
                ws.Cell(1, 3).Value = "GROUP_NAME";
                ws.Cell(1, 4).Value = "BASELINE_STOCK";
                ws.Cell(1, 5).Value = "RANK";
                ws.Cell(1, 6).Value = "POINT_TETAP";
                ws.Cell(1, 7).Value = "POTENSI_POINT";
                ws.Cell(1, 8).Value = "TOTAL_POINT";
                ws.Cell(1, 9).Value = "STATUS";
                ws.Cell(1, 10).Value = "KETERANGAN";
            }

            using (var ms = new MemoryStream())
            {
                workbook.SaveAs(ms);
                return ms.ToArray();
            }
        }

        public static byte[] DataTableToBinaryAll(DataTable dt)
        {

            DataSet ds = new DataSet();
            ds.Tables.Add(dt);
            var workbook = new XLWorkbook();
            workbook.Worksheets.Add(ds);

            using (var ms = new MemoryStream())
            {
                workbook.SaveAs(ms);
                return ms.ToArray();
            }
        }

        public static string getUrlResetPassword(string customerCode, string token)
        {
            var strreader = new StreamReader("hostname.json");
            var reader = new JsonTextReader(strreader);
            var host = JObject.Load(reader);
            var hostname = host.GetValue("Host");
            var url = $"https://{hostname}/resetpassword?UserId={customerCode}&Token={token}";

            return url;
        }

        public static string randomTokenString()
        {
            var rngCryptoServiceProvider = new RNGCryptoServiceProvider();
            var randomBytes = new byte[40];
            rngCryptoServiceProvider.GetBytes(randomBytes);
            // convert random bytes to hex string
            return BitConverter.ToString(randomBytes).Replace("-", "");
        }

        public static DataTable MergeData(DataTable dtFirst, DataTable dtSecond, string headerType)
        {
            if (headerType == "userProfile")
            {
                dtFirst.Columns.Add("STATUS");
                dtFirst.Columns.Add("KETERANGAN");
                for (int i = 0; i < dtFirst.Rows.Count; i++)
                {
                    dtFirst.Rows[i]["PASSWORD"] = dtSecond.Rows[i]["PASSWORD"];
                    dtFirst.Rows[i]["STATUS"] = dtSecond.Rows[i]["STATUS"];
                    dtFirst.Rows[i]["KETERANGAN"] = dtSecond.Rows[i]["KETERANGAN"];
                }
                return dtFirst;
            }
            else
            {
                dtFirst.Columns.Add("STATUS");
                dtFirst.Columns.Add("KETERANGAN");
                for (int i = 0; i < dtFirst.Rows.Count; i++)
                {
                    dtFirst.Rows[i]["STATUS"] = dtSecond.Rows[i]["STATUS"];
                    dtFirst.Rows[i]["KETERANGAN"] = dtSecond.Rows[i]["KETERANGAN"];
                }
                return dtFirst;
            }

        }
        public static Stream DataTableCSVToBinary(DataTable dt)
        {
            DataSet ds = new DataSet();
            ds.Tables.Add(dt);

            StringBuilder sb = new StringBuilder();

            IEnumerable<string> columnNames = dt.Columns.Cast<DataColumn>().
                                              Select(column => column.ColumnName);
            sb.AppendLine(string.Join(",", columnNames));

            foreach (DataRow row in dt.Rows)
            {
                IEnumerable<string> fields = row.ItemArray.Select(field => field.ToString());
                sb.AppendLine(string.Join(",", fields));
            }

            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(sb.ToString());
            writer.Flush();
            stream.Position = 0;
            return stream;
        }

        public static Byte[] DataTableCSVToBinaryAll(DataTable dt)
        {
            DataSet ds = new DataSet();
            ds.Tables.Add(dt);

            StringBuilder sb = new StringBuilder();

            IEnumerable<string> columnNames = dt.Columns.Cast<DataColumn>().
                                              Select(column => column.ColumnName);
            sb.AppendLine(string.Join(",", columnNames));

            foreach (DataRow row in dt.Rows)
            {
                IEnumerable<string> fields = row.ItemArray.Select(field => field.ToString());
                sb.AppendLine(string.Join(",", fields));
            }

            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(sb.ToString());
            writer.Flush();
            stream.Position = 0;
            return stream.ToArray();
        }
    }
}
