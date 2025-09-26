using LibrarySystem.Entities;
using LibrarySystem.Infrastructure;
using LibrarySystem.Interface;
using Microsoft.EntityFrameworkCore;

namespace LibrarySystem.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        AppDBContext _context = new AppDBContext();

        public int Create(Category category)
        {
            _context.Categories.Add(category);
            _context.SaveChanges();
            return category.Id;
        }
        public void Delete(int id)
        {
            var category = _context.Categories.Find(id);
            if (category != null)
            {
                _context.Categories.Remove(category);
                _context.SaveChanges();
            }
        }
        public List<Category> GetAll()
        {
            return _context.Categories
                .Include(c => c.Books)
                .ThenInclude(b => b.Reviews)
                .ThenInclude(r => r.User)
                .ToList();
        }
        public Category? GetById(int id)
        {
            return _context.Categories
                .Include(c => c.Books)
                .FirstOrDefault(c => c.Id == id);
        }
        public void Update(int id, string name)
        {
            var c = _context.Categories.Find(id);

            if (c != null)
            {
                c.Name = name;
                _context.SaveChanges();
            }
        }
    }
}
