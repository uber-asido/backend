using System;
using System.Linq;
using System.Threading.Tasks;

namespace Uber.Module.Movie.Abstraction.Store
{
    public interface IMovieStore
    {
        Task<Model.Movie> Find(Guid key);
        Task<Model.Movie> Merge(Model.Movie movie);
    }
}
