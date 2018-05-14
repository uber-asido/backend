using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Uber.Module.File.Abstraction.Model;
using Uber.Module.File.Abstraction.Store;
using Uber.Module.File.EFCore.Entity;

namespace Uber.Module.File.EFCore.Store
{
    public class UploadHistoryStore : IUploadHistoryStore
    {
        private readonly DataStore db;

        public UploadHistoryStore(DataStore db)
        {
            this.db = db;
        }

        public IQueryable<UploadHistory> Query() => db.UploadHistories;
        public IQueryable<UploadHistory> QuerySingle(Guid key) => db.UploadHistories.Where(e => e.Key == key);

        public Task<UploadHistory> Find(Guid key) => QuerySingle(key).SingleOrDefaultAsync();

        public Task<byte[]> FindFileData(Guid uploadHistoryKey) => db.FileData
            .Where(e => e.Key == uploadHistoryKey)
            .Select(e => e.Data)
            .SingleOrDefaultAsync();

        public async Task<UploadHistory> Create(UploadHistory history, byte[] fileData)
        {
            db.Insert(history);
            db.Insert(new FileData { Key = history.Key, Data = fileData });
            await db.Commit();

            return history;
        }
    }
}
