using LibrarySystem.Entities;
using LibrarySystem.Interface.Service;
using LibrarySystem.Repositories;

namespace LibrarySystem.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly CategoryRepository _categoryRepository = new CategoryRepository();

        public int AddCategory(string name)
        {
            var category = new Category
            {
                Name = name
            };
            return _categoryRepository.Create(category);
        }
        public void UpdateCategory(int id, string name)
        {
            _categoryRepository.Update(id, name);
        }
        public void DeleteCategory(int id)
        {
            _categoryRepository.Delete(id);
        }
        public List<Category> GetAllCategories()
        {
            return _categoryRepository.GetAll();
        }
        public Category? GetCategoryById(int id)
        {
            return _categoryRepository.GetById(id);
        }
    }
}
