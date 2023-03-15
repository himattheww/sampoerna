using System;
using System.Collections.Generic;

#nullable disable

namespace LeaderboardAPI.Entities
{
    public partial class FileUpload
    {
        public int Id { get; set; }
        public string NameFile { get; set; }
        public DateTime UploadDateTime { get; set; }
        public DateTime FinishUpload { get; set; }
        public string Status { get; set; }
        public string UserUpload { get; set; }
    }
}