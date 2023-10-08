using ApiRestFull.DTO;
using ApiRestFull.Entities;
using ApiRestFull.Repository.IRepository;
using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace ApiRestFull.Controllers;

[ApiController]
[Route("api/category")]
public class CategoryController : ControllerBase
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IMapper _mapper;

    public CategoryController(ICategoryRepository categoryRepository, IMapper mapper)
    {
        _categoryRepository = categoryRepository;
        _mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<List<CategoryDTo>>> Get()
    {
        var categoryListDb = await _categoryRepository.GetCategories();
        var categoryList = _mapper.Map<List<CategoryDTo>>(categoryListDb);
        return Ok(categoryList);
    }
    
    [HttpGet("{id:int}", Name = "GetCategoryById")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CategoryDTo>> Get(int id)
    {
        var categoryDb = await _categoryRepository.GetCategoryById(id);
        if (categoryDb is null) return NotFound();
        var category = _mapper.Map<CategoryDTo>(categoryDb);
        return Ok(category);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> Post([FromBody] CategoryCreationDTo categoryCreationDTo)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        if (categoryCreationDTo is null) return BadRequest(ModelState);
        var isExit = await _categoryRepository.IsExistCategory(categoryCreationDTo.CategoryName);
        if (isExit)
        {
            ModelState.AddModelError("", "The category is already exist");
            return StatusCode(400, ModelState);
        }

        var category = _mapper.Map<Category>(categoryCreationDTo);
        var canCreateCategory = await _categoryRepository.CreateCategory(category);
        if (!canCreateCategory)
        {
            ModelState.AddModelError("", $"Some error has happened with the register {category.CategoryName}");
            return StatusCode(500, ModelState);
        }
        return CreatedAtRoute("GetCategoryById", new { category.Id }, category);
    }

    [HttpPut("{id:int}", Name = "UpdateCategory")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> Put(int id, [FromBody] CategoryCreationDTo categoryCreationDTo)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        if (string.IsNullOrEmpty(categoryCreationDTo.CategoryName)) return BadRequest();

        var originalCategory = await _categoryRepository.GetCategoryById(id);
        if (originalCategory == null) return NotFound("The category that you want to update doesn't exist");

        _mapper.Map(categoryCreationDTo, originalCategory);

        var isAlreadyExist = await _categoryRepository.IsExistCategory(originalCategory.CategoryName);
        if (isAlreadyExist) return BadRequest("The category is already exist");

        var canUpdateCategory  = await _categoryRepository.UpdateCategory(originalCategory);
        if (!canUpdateCategory) return BadRequest($"Something error has happened updating {originalCategory.CategoryName}");

        return NoContent();
    }


    [HttpPatch("{id:int}")]
    public async Task<ActionResult> Patch(int id, [FromBody] JsonPatchDocument<CategoryPatchDTo> jsonPatchDocument)
    {
        if (jsonPatchDocument is null) return BadRequest();

        var categoryFromDb = await _categoryRepository.GetCategoryById(id);
        if (categoryFromDb is null) return NotFound();

        categoryFromDb.UpdatedAt = DateTime.Now;

        var categoryDto = _mapper.Map<CategoryPatchDTo>(categoryFromDb);
        
        jsonPatchDocument.ApplyTo(categoryDto, ModelState);

        var isValid = TryValidateModel(categoryDto);

        if (!isValid) return BadRequest();

        _mapper.Map(categoryDto, categoryFromDb);

        var canToUpdate = await _categoryRepository.Save();

        if (!canToUpdate) return BadRequest();

        return NoContent();


    }

    [HttpDelete("{id:int}", Name = "DeleteCategory")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> Delete(int id)
    {
        var isExist = await _categoryRepository.IsExistCategory(id);
        if (!isExist) return NotFound("You try to delete a category that doesn't exist");
        var categoryDb = await _categoryRepository.GetCategoryById(id);
        var canDelete = await _categoryRepository.DeleteCategory(categoryDb);
        if (!canDelete) return BadRequest();
        return NoContent();
    }
}