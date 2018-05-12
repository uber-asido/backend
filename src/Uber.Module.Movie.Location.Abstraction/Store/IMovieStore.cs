using System;
using System.Linq;
using System.Threading.Tasks;

namespace Uber.Module.Movie.Location.Abstraction.Store
{
    public interface IMovieStore
    {
        IQueryable<Model.Movie> Query();
        IQueryable<Model.Movie> QuerySingle(Guid key);

        Task Create(Model.Movie movie);
    }
}
