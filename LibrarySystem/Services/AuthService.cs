using LibrarySystem.Entities;
using LibrarySystem.Enum;
using LibrarySystem.Interface.Service;
using LibrarySystem.Repositories;

namespace LibrarySystem.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserRepository _userRepository = new UserRepository();

        public User Register(string username, string password, RoleEnum role)
        {
            var existingUser = _userRepository.GetAll()
                .FirstOrDefault(u => u.UserName.ToLower() == username.ToLower());

            if (existingUser != null)
                throw new Exception("This username is already registered.");

            var user = new User
            {
                UserName = username,
                Password = password,
                Role = role
            };

            _userRepository.Create(user);
            return user;
        }
        public User? Login(string username, string password)
        {
            return _userRepository.GetByUsername(username, password);
        }
        public List<User> GetAllUsers()
        {
            return _userRepository.GetAll();
        }

    }
}

