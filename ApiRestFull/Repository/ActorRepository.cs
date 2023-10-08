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
        return  await _context.Actors
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
        actor.CreationDate = DateTime.Now;
        actor.UpdatedAt = DateTime.Now;
        _context.Actors.Add(actor);
        return await Save();
    }

    public async Task<bool> UpdateActor(Actor actor)
    {
        actor.UpdatedAt = DateTime.Now;
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
    
    public int CalculateAge(DateTime birthDate)
    {
        var age = DateTime.Now.Year - birthDate.Year;

        if (age > 100) return 0;
        
        if (DateTime.Now.Date < birthDate.AddYears(age))
            age--;
        
        return age;
    }

    public async Task<bool> Save()
    {
        return await _context.SaveChangesAsync() >= 0;
    }
}