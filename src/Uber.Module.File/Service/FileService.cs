using System;
using System.Linq;
using System.Threading.Tasks;
using Uber.Core;
using Uber.Module.File.Abstraction.Model;
using Uber.Module.File.Abstraction.Service;
using Uber.Module.File.Abstraction.Store;
using Uber.Module.File.FileProcessor;

namespace Uber.Module.File.Service
{
    public class FileService : IFileService
    {
        private readonly IUploadHistoryStore historyStore;

        public FileService(IUploadHistoryStore historyStore)
        {
            this.historyStore = historyStore;
        }

        public IQueryable<UploadHistory> QueryHistory() => historyStore.Query();
        public IQueryable<UploadHistory> QueryHistorySingle(Guid key) => historyStore.QuerySingle(key);

        public Task<UploadHistory> FindHistory(Guid key) => historyStore.Find(key);

        public async Task<OperationResult<UploadHistory>> ScheduleForProcessing(Abstraction.Service.File file)
        {
            var processor = ProcessorResolver.Resolve(file.Filename);
            if (processor == null)
                return OperationResult<UploadHistory>.Failed("The file format is not supported.");

            var history = new UploadHistory
            {
                Key = Guid.NewGuid(),
                Filename = file.Filename,
                Status = UploadStatus.Ongoing,
                Timestamp = DateTimeOffset.UtcNow
            };
            await historyStore.Create(history, file.Data);

            return OperationResult<UploadHistory>.Success(history);
        }
    }
}
