using LibrarySystem.Entities;

namespace LibrarySystem.Interface.Service
{
    public interface ICategoryService
    {
        int AddCategory(string name);
        void UpdateCategory(int id, string name);
        void DeleteCategory(int id);
        List<Category> GetAllCategories();
        Category? GetCategoryById(int id);
    }
}
