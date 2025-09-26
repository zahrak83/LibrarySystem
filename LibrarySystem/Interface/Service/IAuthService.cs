using LibrarySystem.Entities;
using LibrarySystem.Enum;

namespace LibrarySystem.Interface.Service
{
    public interface IAuthService
    {
        User Register(string username, string password, RoleEnum role);
        User? Login(string username, string password);
        List<User> GetAllUsers();
    }
}
