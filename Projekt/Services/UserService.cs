using Projekt.Data;
using Projekt.Models;
using Projekt.Services;

namespace Projekt.Services
{
    public class UserService : IService
    {
        private static readonly P4ProjektDbContext _context = AppService.DbContext;
        private static readonly UserService _instance = new UserService();

        public UserService() { }

        public static UserService Instance { get { return _instance; } }

        public static bool IsValid()
        {
            return _instance != null;
        }

        public static UserModel[] GetUsers()
        {
            return [.. _context.Users];
        }

        public static UserModel? GetUserById(int id)
        {
            return _context.Users.FirstOrDefault(u => u.Id == id) 
                ?? throw new KeyNotFoundException($"User with ID {id} not found.");
        }

        public static UserModel? LogInAsUser(string name, string password)
        {
            UserModel? user = _context.Users.FirstOrDefault(u => u.Name == name && u.Password == password);
            return user ?? null;
        }

        public static async Task<UserModel> AddUser(UserModel user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }
    }
}
