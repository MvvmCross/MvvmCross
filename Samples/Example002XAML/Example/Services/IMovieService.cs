using System.Collections.Generic;
using System.Threading.Tasks;
using Example.Models;

namespace Example.Services
{
    public interface IMovieService
    {
        Task<List<Movie>> SearchMovie(string movieTitle);
        Task<DetailedMovie> DetailedMovieFromId(int id);
    }
}
