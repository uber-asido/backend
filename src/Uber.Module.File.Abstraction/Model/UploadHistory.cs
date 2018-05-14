using System;

namespace Uber.Module.File.Abstraction.Model
{
    public enum UploadStatus
    {
        Pending = 1,
        Done
    }

    public class UploadHistory
    {
        public Guid Key { get; set; }
        public string Filename { get; set; }
        public UploadStatus Status { get; set; }
        public DateTimeOffset Timestamp { get; set; }
        public string[] Errors { get; set; }
    }
}
