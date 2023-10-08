using ApiRestFull.DTO;
using ApiRestFull.Entities;
using ApiRestFull.Repository.IRepository;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace ApiRestFull.Controllers;

[ApiController]
[Route("api/actors")]
public class ActorController : ControllerBase
{
    private readonly IActorRepository _actorRepository;
    private readonly IMapper _mapper;

    public ActorController(IActorRepository actorRepository, IMapper mapper)
    {
        _actorRepository = actorRepository;
        _mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<ActorDTo>> Get()
    {
        var actorsFromDb = await _actorRepository.GetActors();
        var actors = _mapper.Map<List<ActorDTo>>(actorsFromDb);
        foreach (var actor in actors)
        {
            actor.Age = _actorRepository.CalculateAge(actor.Birthday);
        }
        return Ok(actors);
    }

    [HttpGet("{id:int}", Name = "GetActorById")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<ActorDTo>> Get(int id)
    {
        var actorFromDb = await _actorRepository.GetActorById(id);
        var actor = _mapper.Map<ActorDTo>(actorFromDb);
     
        actor.Age = _actorRepository.CalculateAge(actor.Birthday);
        
        return Ok(actor);
    }

    [HttpPost]
    public async Task<ActionResult> Post(ActorCreationDTo actorCreationDTo)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var isAlreadyExistActor = await _actorRepository.IsExistActor(actorCreationDTo.Name);
        if (isAlreadyExistActor) return BadRequest("The actor that you can to create is already exist");
        var newActor = _mapper.Map<Actor>(actorCreationDTo);
        var canCreateIntoDatabase = await _actorRepository.CreateActor(newActor);
        if (!canCreateIntoDatabase)
        {
            ModelState.AddModelError("", $"Some error has happened with the register {newActor.Name}");
            return StatusCode(500, ModelState);
        }
        return CreatedAtRoute("GetActorById", new{Id = newActor.Id}, newActor);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> Put(ActorCreationDTo actorCreationDTo, int id)
    {
        if (!ModelState.IsValid) return BadRequest($"The model is with error {ModelState}");

        var isExistActorById = await _actorRepository.IsExistActor(id);

        if (!isExistActorById)
            return NotFound("The actor that you try to update is not exist, please try with another id");

        var actorFromDb = await _actorRepository.GetActorById(id);

        _mapper.Map(actorCreationDTo, actorFromDb);
        
        var canUpdateActor = await _actorRepository.UpdateActor(actorFromDb);

        if (!canUpdateActor) return BadRequest("some went wrong, please try again");

        return NoContent();

    }

    [HttpPatch("{id:int}")]
    public async Task<ActionResult> Patch(int id, [FromBody] JsonPatchDocument<ActorPatchDTo> jsonPatchDocument)
    {
        if (!ModelState.IsValid) return BadRequest();
        var actorFromDb = await _actorRepository.GetActorById(id);
        if (actorFromDb is null) return NotFound();

        actorFromDb.UpdatedAt = DateTime.Now;

        var movieDto = _mapper.Map<ActorPatchDTo>(actorFromDb);
        
        jsonPatchDocument.ApplyTo(movieDto, ModelState);
        
        var isValid = TryValidateModel(movieDto);
        if (!isValid) return BadRequest();

        _mapper.Map(movieDto, actorFromDb);

        var canToUpdate = await _actorRepository.Save();

        if (!canToUpdate) return BadRequest($"Something error has occurred {ModelState}");

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Delete(int id)
    {
        var isExistActorById = await _actorRepository.IsExistActor(id);
        var actor = await _actorRepository.GetActorById(id);

        if (!isExistActorById)
            return NotFound("The actor that you try to delete is not exist, please try with another id");

        var canDelete = await _actorRepository.DeleteActor(actor);

        if (!canDelete) return BadRequest($"something error has occurred while we were trying to delete the actor: {actor}");

        return NoContent();
    }
}