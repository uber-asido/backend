using System;

namespace Uber.Module.File.EFCore.Entity
{
    public class FileData
    {
        public Guid Key { get; set; }
        public byte[] Data { get; set; }
    }
}
