using LibrarySystem.Entities;

namespace LibrarySystem.Interface
{
    public interface IUserRepository
    {
        int Create(User user);
        void Update(User user);
        void Delete(int id);
        User GetById(int id);
        List<User> GetAll();
        User GetByUsername(string username, string password);

    }
}
