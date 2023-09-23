using ApiRestFull.DTO;
using ApiRestFull.Entities;

namespace ApiRestFull.Repository.IRepository;

public interface ICategoryRepository
{
    Task<List<Category>> GetCategories();
    Task<Category> GetCategoryById(int id);
    Task<bool> CreateCategory(Category category);
    Task<bool> UpdateCategory(Category category);
    Task<bool> DeleteCategory(Category category);
    Task<bool> IsExistCategory(string name);
    Task<bool> IsExistCategory(int id);
    Task<bool> Save();
}