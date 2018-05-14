using Microsoft.EntityFrameworkCore;
using Uber.Core.EFCore;
using Uber.Module.File.Abstraction.Model;
using Uber.Module.File.EFCore.Entity;

namespace Uber.Module.File.EFCore
{
    public class DataStore : DataStoreBase<DataContext>
    {
        public DbSet<UploadHistory> UploadHistories => DataContext.UploadHistories;
        public DbSet<FileData> FileData => DataContext.FileData;

        public DataStore(DataContext context) : base(context) { }
    }
}
