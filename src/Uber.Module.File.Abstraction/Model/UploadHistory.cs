using System;

namespace Uber.Module.File.Abstraction.Model
{
    public enum UploadStatus
    {
        Ongoing = 1,
        Success,
        Error
    }

    public class UploadHistory
    {
        public Guid Key { get; set; }
        public string Filename { get; set; }
        public UploadStatus Status { get; set; }
        public string Error { get; set; }
        public DateTimeOffset Timestamp { get; set; }
    }
}
