using Microsoft.EntityFrameworkCore;
using Project_Coffe.Data;
using Project_Coffe.Entities;
using Project_Coffe.Models.ModelInterface;
using System.Security.Cryptography;
using System.Text;

namespace Project_Coffe.Models.ModelRealization
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly AuthenticationService _authService;

        public UserService(ApplicationDbContext dbContext, AuthenticationService authService)
        {
            _dbContext = dbContext;
            _authService = authService;
        }

        public async Task<User?> Register(string name, string email, string password)
        {
            if (await IsEmailTaken(email))
            {
                return null;
            }

            var hashedPassword = HashPassword(password);

            var user = new User
            {
                Name = name,
                Email = email,
                PasswordHash = hashedPassword
            };

            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();
            return user;
        }

        public async Task<string?> Login(string email, string password)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null || !VerifyPassword(password, user.PasswordHash))
            {
                return null;
            }

            var token = _authService.GenerateToken(user.Id, "User");
            return token;
        }

        public async Task<User?> GetUserById(int userId)
        {
            return await _dbContext.Users.FindAsync(userId);
        }
        public async Task<User?> UpdateUser(int userId, string name, string email, string password)
        {
            var user = await _dbContext.Users.FindAsync(userId);
            if (user == null)
                return null;

            user.Name = name;
            user.Email = email;
            if (!string.IsNullOrWhiteSpace(password))
                user.PasswordHash = HashPassword(password);

            _dbContext.Users.Update(user);
            await _dbContext.SaveChangesAsync();
            return user;
        }
        public async Task<bool> DeleteUser(int userId)
        {
            var user = await _dbContext.Users.FindAsync(userId);
            if (user == null)
                return false;

            _dbContext.Users.Remove(user);
            await _dbContext.SaveChangesAsync();
            return true;
        }
        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hash);
        }

        private bool VerifyPassword(string password, string hashedPassword)
        {
            var hash = HashPassword(password);
            return hash == hashedPassword;
        }

        public async Task<bool> IsEmailTaken(string email)
        {
            return await _dbContext.Users.AnyAsync(u => u.Email == email);
        }
    }
}
