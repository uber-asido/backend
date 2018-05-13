using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Uber.Core.EFCore
{
    public abstract class DataStoreBase<TDataContext>
        where TDataContext : DbContext
    {
        protected readonly TDataContext DataContext;

        public DataStoreBase(TDataContext dataContext)
        {
            DataContext = dataContext;

            DataContext.ChangeTracker.AutoDetectChangesEnabled = false;
            DataContext.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        public void Insert(object entity) => DataContext.Entry(entity).State = EntityState.Added;
        public void Insert(IEnumerable<object> entities)
        {
            foreach (var entity in entities)
                Insert(entity);
        }

        public Task InsertAndCommit(object entity)
        {
            Insert(entity);
            return Commit();
        }

        public Task InsertAndCommit(IEnumerable<object> entities)
        {
            Insert(entities);
            return Commit();
        }

        public void Update(object entity) => DataContext.Entry(entity).State = EntityState.Modified;
        public void Update(IEnumerable<object> entities)
        {
            foreach (var entity in entities)
                Update(entity);
        }

        public Task UpdateAndCommit(object entity)
        {
            Update(entity);
            return Commit();
        }

        public Task UpdateAndCommit(IEnumerable<object> entities)
        {
            Update(entities);
            return Commit();
        }

        public void Delete(object entity) => DataContext.Entry(entity).State = EntityState.Deleted;
        public void Delete(IEnumerable<object> entities)
        {
            foreach (var entity in entities)
                Delete(entity);
        }

        public Task DeleteAndCommit(object entity)
        {
            Delete(entity);
            return Commit();
        }

        public Task DeleteAndCommit(IEnumerable<object> entities)
        {
            Delete(entities);
            return Commit();
        }

        public async Task Commit()
        {
            await DataContext.SaveChangesAsync();

            foreach (var entry in DataContext.ChangeTracker.Entries().ToList())
                entry.State = EntityState.Detached;
        }
    }
}
