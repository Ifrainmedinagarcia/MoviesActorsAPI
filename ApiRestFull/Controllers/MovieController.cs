using ApiRestFull.DTO;
using ApiRestFull.Entities;
using ApiRestFull.Repository.IRepository;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace ApiRestFull.Controllers;

[ApiController]
[Route("api/movie")]
public class MovieController : ControllerBase
{
    private readonly IMovieRepository _movieRepository;
    private readonly IMapper _mapper;

    public MovieController(IMovieRepository movieRepository,IMapper mapper)
    {
        _movieRepository = movieRepository;
        _mapper = mapper;
   }
    
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<List<MovieDTo>>> Get()
    {
        var getMovies = await _movieRepository.GetMovies();
        var movies = _mapper.Map<List<MovieDTo>>(getMovies);
        return Ok(movies);
    }
    
    [HttpGet("{id:int}", Name = "GetMovieById")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<MovieDTo>> Get(int id)
    {
        var isExistMovie = await _movieRepository.IsExistMovie(id);
        if (!isExistMovie) return NotFound();
        var getMovies = await _movieRepository.GetMovieById(id);
        var movies = _mapper.Map<MovieDTo>(getMovies);
        return Ok(movies);
    }
    
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpGet("byCategoryName")]
    public async Task<ActionResult<List<MovieDTo>>> Get(string categoryName)
    {
        var movieFromDb = await _movieRepository.GetMoviesByCategory(categoryName);
        var movie = _mapper.Map<List<MovieDTo>>(movieFromDb);
        return Ok(movie);
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpGet("byTitle")]
    public async Task<ActionResult<List<MovieDTo>>> GetByTitle(string title)
    {
        var movieFromDb = await _movieRepository.GetMoviesByTitle(title);
        var movie = _mapper.Map<List<MovieDTo>>(movieFromDb);
        return Ok(movie);
    }
    
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> Post(MovieCreationDTo movieCreationDTo)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        if (movieCreationDTo is null) return BadRequest(ModelState);
        
        var existMovie = await _movieRepository.IsExistMovie(movieCreationDTo.Title);
        if (existMovie) return BadRequest("The movie is already exist");
        var newMovie = _mapper.Map<Movie>(movieCreationDTo);
        var canCreateMovie = await _movieRepository.CreateMovie(newMovie);
        if (canCreateMovie) return CreatedAtRoute("GetMovieById", new { newMovie.Id }, newMovie);
        ModelState.AddModelError("", $"Some error has happened with the register {newMovie.Title}");
        return StatusCode(500, ModelState);
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> Patch(int id,  [FromBody] MovieCreationDTo movieCreationDTo)
    {
        if (movieCreationDTo is null) return BadRequest();
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var isExistMovie = await _movieRepository.IsExistMovie(id);
        
        var movieFromDb = await _movieRepository.GetMovieById(id);

        _mapper.Map(movieCreationDTo, movieFromDb);
        
        if (!isExistMovie) return NotFound("The movie that you want to update doesn't exist");
        var canUpdateMovie = await _movieRepository.UpdateMovie(movieFromDb);
        if (canUpdateMovie) return NoContent();
        return BadRequest("There are some errors inside our platform");
    }

    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpPatch("{id:int}")]
    public async Task<ActionResult> Patch(int id, [FromBody] JsonPatchDocument<MoviePatchDTo> patchDocument)
    {
        if (patchDocument is null) return BadRequest();
        var movieFromDb = await _movieRepository.GetMovieById(id);
        if (movieFromDb is null) return NotFound();
        
        movieFromDb.UpdatedAt = DateTime.Now;

        var movieDTo = _mapper.Map<MoviePatchDTo>(movieFromDb);
        
        patchDocument.ApplyTo(movieDTo, ModelState);
        
        var isValid = TryValidateModel(movieDTo);
        
        if (!isValid) return BadRequest(ModelState);

        _mapper.Map(movieDTo, movieFromDb);

        var canToUpdate = await _movieRepository.Save();

        if (!canToUpdate) return BadRequest("Something error has occurred");

        return NoContent();
    }

    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Delete(int id)
    {
        var isExist = await _movieRepository.IsExistMovie(id);
        if (!isExist) return BadRequest("You are trying delete a movie that doesnt exist");
        var movie = await _movieRepository.GetMovieById(id);
        var canDelete = await _movieRepository.DeleteMovie(movie);
        if (!canDelete) return BadRequest("The error has occurred");
        return NoContent();
    }
}                                               