using System;
using System.Linq;
using System.Threading.Tasks;
using Uber.Core;
using Uber.Module.File.Abstraction.Model;

namespace Uber.Module.File.Abstraction.Service
{
    public class File
    {
        public readonly string Filename;
        public readonly byte[] Data;

        public File(string filename, byte[] data)
        {
            Filename = filename;
            Data = data;
        }
    }

    public interface IFileService
    {
        IQueryable<UploadHistory> QueryHistory();
        IQueryable<UploadHistory> QueryHistorySingle(Guid key);

        Task<UploadHistory> FindHistory(Guid key);

        Task<OperationResult<UploadHistory>> ScheduleForProcessing(File file);
    }
}
