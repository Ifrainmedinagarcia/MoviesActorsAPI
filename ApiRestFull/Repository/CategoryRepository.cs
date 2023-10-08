using ApiRestFull.DTO;
using ApiRestFull.Entities;
using ApiRestFull.Repository.IRepository;
using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiRestFull.Repository;

public class CategoryRepository : ICategoryRepository
{
    private readonly ApplicationDbContext _context;

    public CategoryRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Category>> GetCategories()
    {
       return await _context.Categories
            .ToListAsync();
    }

    public async Task<Category> GetCategoryById(int id)
    {
        return await _context.Categories.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<bool> CreateCategory(Category category)
    {
        category.CreationDate = DateTime.Now;
        category.UpdatedAt = DateTime.Now;
        _context.Categories.Add(category);
        return await Save();
    }

    public async Task<bool> UpdateCategory(Category category)
    {
        category.UpdatedAt = DateTime.Now;
        _context.Categories.Update(category);
        return await Save();
    }

    public Task<bool> DeleteCategory(Category category)
    {
        _context.Categories.Remove(category);
        return Save();
    }

    public async Task<bool> IsExistCategory(string name)
    {
        return await _context.Categories
            .AnyAsync(x => x.CategoryName.ToLower().Trim() == name.ToLower().Trim());
    }

    public async Task<bool> IsExistCategory(int id)
    {
        return await _context.Categories.AnyAsync(x => x.Id == id);
    }

    public async Task<bool> Save()
    {
        return await _context.SaveChangesAsync() >= 0;
    }
}