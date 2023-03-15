namespace LeaderboardAPI.ViewModels.Output
{
    public class FileUploadVM
    {
        public int Id { get; set; }
        public string NameFile { get; set; }
        public DateTime UploadDateTime { get; set; }
        public DateTime FinishUpload { get; set; }
        public string Status { get; set; }
        public byte[]? Data { get; set; }
        public string UserUpload { get; set; }
    }
}
