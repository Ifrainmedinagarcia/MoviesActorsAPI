using ApiRestFull.DTO;
using ApiRestFull.Entities;
using ApiRestFull.Repository.IRepository;
using AutoMapper;
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
        return Ok(actors);
    }

    [HttpGet("{id:int}", Name = "GetActorById")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<ActorDTo>> Get(int id)
    {
        var actorFromDb = await _actorRepository.GetActorById(id);
        var actor = _mapper.Map<ActorDTo>(actorFromDb);
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
    
}