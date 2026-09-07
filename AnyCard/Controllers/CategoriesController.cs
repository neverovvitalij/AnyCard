using AnyCard.Application.Interfaces;
using AnyCard.Domain.Model;
using AnyCard.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace AnyCard.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly ICategoryRepository _categoryRepository;

    public CategoriesController(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CategoryDto>>> GetAll()
    {
        var categories =  await _categoryRepository.GetAllAsync();
        var categoriesDto = categories.Select(c => new CategoryDto(c.Id, c.Name)).ToList();
        return Ok(categoriesDto);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CategoryDto?>> GetById(int id)
    {
        var category = await _categoryRepository.GetByIdAsync(id);
        if(category == null)
        {
            return NotFound("Kein Kategorie gefunden");
        }
        var categorieDto = new CategoryDto(category.Id, category.Name);
        return Ok(categorieDto);
    }

    [HttpPost]
    public async Task<ActionResult<CategoryDto>> CreateCategory(CreateCategoryDto createCategoryDto)
    {
        var category = new Category { Name = createCategoryDto.Name };
        await _categoryRepository.AddAsync(category);
        await _categoryRepository.SaveChangesAsync();

        var categoryDto = new CategoryDto(category.Id, category.Name);
        return CreatedAtAction(nameof(GetById), new { id = category.Id }, categoryDto);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<CategoryDto>> UpdateCategory(int id, CreateCategoryDto createCategoryDto)
    {
        var category = await _categoryRepository.GetByIdAsync(id);
        if(category == null)
        {
            return NotFound("Kein Kategorie gefunden");
        }
        category.Name = createCategoryDto.Name;
        await _categoryRepository.SaveChangesAsync();

        var categoryDto = new CategoryDto(category.Id, category.Name);
        return Ok(categoryDto);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteCategory(int id)
    {
        var category = await _categoryRepository.GetByIdAsync(id);
        if (category == null)
        {
            return NotFound("Kein Kategorie gefunden");
        }

        _categoryRepository.Delete(category);
        await _categoryRepository.SaveChangesAsync();
        return NoContent();
    }
}
