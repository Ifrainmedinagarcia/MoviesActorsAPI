using ApiRestFull.Entities;
using ApiRestFull.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace ApiRestFull.Repository;

public class MovieRepository : IMovieRepository
{
    private readonly ApplicationDbContext _context;

    public MovieRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Movie>> GetMovies()
    {
        return await _context.Movies
            .Include(x => x.MovieCategories)
            .ThenInclude(x => x.Category)
            .Include(x => x.MoviesActors)
            .ThenInclude(x => x.Actor)
            .ToListAsync();
    }

    public async Task<Movie> GetMovieById(int id)
    {
        return await _context.Movies
            .Include(x => x.MovieCategories)
            .ThenInclude(x => x.Category)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<bool> CreateMovie(Movie movie)
    {
        movie.CreationDate = DateTime.Now;
        _context.Add(movie);
        return await Save();
    }

    public async Task<bool> UpdateMovie(Movie movie)
    {
        movie.UpdatedDate = DateTime.Now;
        _context.Update(movie);
        return await Save();
    }

    public async Task<bool> DeleteMovie(Movie movie)
    {
        _context.Remove(movie);
        return await Save();
    }

    public async Task<bool> IsExistMovie(string title)
    {
        return await _context.Movies.AnyAsync(x => x.Title == title);
    }

    public async Task<bool> IsExistMovie(int id)
    {
        return await _context.Movies.AnyAsync(x => x.Id == id);
    }

    public async Task<bool> Save()
    {
        return await _context.SaveChangesAsync() >= 0;
    }
}