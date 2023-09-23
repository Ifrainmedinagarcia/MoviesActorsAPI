using ApiRestFull.Entities;
using ApiRestFull.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace ApiRestFull.Repository;

public class ActorRepository : IActorRepository
{
    private readonly ApplicationDbContext _context;

    public ActorRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<List<Actor>> GetActors()
    {
        return await _context.Actors
            .Include(x => x.MoviesActors)
            .ThenInclude(x => x.Movie)
            .ToListAsync();
    }
    public async Task<Actor> GetActorById(int id)
    {
        return await _context.Actors
            .Include(x => x.MoviesActors)
            .ThenInclude(x=> x.Movie)
            .FirstOrDefaultAsync(x => x.Id == id)!;
    }

    public async Task<bool> CreateActor(Actor actor)
    {
        _context.Actors.Add(actor);
        return await Save();
    }

    public async Task<bool> UpdateActor(Actor actor)
    {
        _context.Actors.Update(actor);
        return await Save();
    }

    public async Task<bool> DeleteActor(Actor actor)
    {
        _context.Actors.Remove(actor);
        return await Save();
    }

    public async Task<bool> IsExistActor(string name)
    {
        return await _context.Actors.AnyAsync(x => x.Name == name);
    }

    public async Task<bool> IsExistActor(int id)
    {
        return await _context.Actors.AnyAsync(x => x.Id == id);
    }

    public async Task<bool> Save()
    {
        return await _context.SaveChangesAsync() >= 0;
    }
}