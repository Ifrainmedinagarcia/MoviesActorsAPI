using ApiRestFull.Entities;

namespace ApiRestFull.Repository.IRepository;

public interface IActorRepository
{
    Task<List<Actor>> GetActors();
    
    Task<Actor> GetActorById(int id);
    Task<bool> CreateActor(Actor actor);
    Task<bool> UpdateActor(Actor actor);
    Task<bool> DeleteActor(Actor actor);
    Task<bool> IsExistActor(string name);
    Task<bool> IsExistActor(int id);
    Task<bool> Save();
}