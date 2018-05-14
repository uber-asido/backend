using System;
using System.Linq;
using System.Threading.Tasks;
using Uber.Module.File.Abstraction.Model;

namespace Uber.Module.File.Abstraction.Store
{
    public interface IUploadHistoryStore
    {
        IQueryable<UploadHistory> Query();
        IQueryable<UploadHistory> QuerySingle(Guid key);

        Task<UploadHistory> Find(Guid key);
        Task<byte[]> FindFileData(Guid uploadHistoryKey);

        Task<UploadHistory> Create(UploadHistory history, byte[] fileData);
        Task<UploadHistory> Update(UploadHistory history);
    }
}
