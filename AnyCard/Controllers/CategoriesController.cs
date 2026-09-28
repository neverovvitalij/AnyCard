using AnyCard.Application.Extensions;
using AnyCard.Application.Interfaces;
using AnyCard.Domain.Model;
using AnyCard.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AnyCard.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
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
        var userId = User.GetUserId();
        var categories =  await _categoryRepository.GetAllAsync(userId);
        var categoriesDto = categories.Select(c => new CategoryDto(c.Id, c.Name)).ToList();
        return Ok(categoriesDto);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CategoryDto?>> GetById(int id)
    {
        var userId = User.GetUserId();
        var category = await _categoryRepository.GetByIdAsync(id, userId);
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
        var userId = User.GetUserId();
        var category = new Category { Name = createCategoryDto.Name, UserId = userId };
        try
        {
        await _categoryRepository.AddAsync(category);
        await _categoryRepository.SaveChangesAsync();
        } catch(DbUpdateException)
        {
            return Conflict("Diese Kategorie existiert bereits.");
        }

        var categoryDto = new CategoryDto(category.Id, category.Name);
        return CreatedAtAction(nameof(GetById), new { id = category.Id }, categoryDto);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<CategoryDto>> UpdateCategory(int id, CreateCategoryDto createCategoryDto)
    {
        var userId = User.GetUserId();
        var category = await _categoryRepository.GetByIdAsync(id, userId);
        if(category == null)
        {
            return NotFound("Kein Kategorie gefunden");
        }
        category.Name = createCategoryDto.Name;
        try
        {
        await _categoryRepository.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            return Conflict("Diese Kategorie existiert bereits.");
        }

        var categoryDto = new CategoryDto(category.Id, category.Name);
        return Ok(categoryDto);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteCategory(int id)
    {
        var userId = User.GetUserId();
        var category = await _categoryRepository.GetByIdAsync(id, userId);
        if (category == null)
        {
            return NotFound("Kein Kategorie gefunden");
        }

        _categoryRepository.Delete(category);
        await _categoryRepository.SaveChangesAsync();
        return NoContent();
    }
}
