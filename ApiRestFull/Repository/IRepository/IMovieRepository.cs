using ApiRestFull.DTO;
using ApiRestFull.Entities;

namespace ApiRestFull.Repository.IRepository;

public interface IMovieRepository
{
    
    Task<List<Movie>> GetMovies();
    Task<List<Movie>> GetMoviesByCategory(string categoryName);
    Task<List<Movie>> GetMoviesByTitle(string title);
    Task<Movie> GetMovieById(int id);
    Task<bool> CreateMovie(Movie movie);
    Task<bool> UpdateMovie(Movie movie);
    Task<bool> DeleteMovie(Movie movie);
    Task<bool> IsExistMovie(string title);
    Task<bool> IsExistMovie(int id);
    Task<bool> Save();
}