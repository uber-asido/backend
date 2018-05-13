using System;
using System.Threading.Tasks;

namespace Uber.Module.Movie.Abstraction.Service
{
    public interface IMovieService
    {
        Task<Model.Movie> Find(Guid key);
        Task<Model.Movie> Merge(Model.Movie movie);
    }
}
