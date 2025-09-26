using LibrarySystem.Entities;

namespace LibrarySystem.Interface
{
    public interface ICategoryRepository
    {
        int Create(Category category);
        void Update(int id, string name);
        void Delete(int id);
        Category GetById(int id);
        List<Category> GetAll();
    }
}
