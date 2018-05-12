using System;
using System.Linq;
using System.Threading.Tasks;

namespace Uber.Module.Movie.Location.Abstraction.Manager
{
    public interface IMovieManager
    {
        IQueryable<Model.Movie> Query();
        IQueryable<Model.Movie> QuerySingle(Guid key);

        Task<Model.Movie> Create(Model.Movie movie);
    }
}
