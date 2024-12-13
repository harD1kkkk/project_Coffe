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
<<<<<<< HEAD
        private readonly AuthenticationService _authService;
        private readonly ILogger<UserService> _logger;

        public UserService(ApplicationDbContext dbContext, AuthenticationService authService, ILogger<UserService> logger)
        {
            _dbContext = dbContext;
            _authService = authService;
            _logger = logger;
=======

        public UserService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
>>>>>>> 0d50e16b2a6a77a4377ebb9f8c716686a9238ed9
        }

        public async Task<User?> Register(string name, string email, string password)
        {
<<<<<<< HEAD
            try
            {
                if (await IsEmailTaken(email))
                {
                    _logger.LogWarning($"Email {email} is already taken.");
                    return null;
                }

                string hashedPassword = HashPassword(password);

                User user = new User
                {
                    Name = name,
                    Email = email,
                    PasswordHash = hashedPassword
                };

                _dbContext.Users.Add(user);
                await _dbContext.SaveChangesAsync();
                _logger.LogInformation($"User {user.Name} registered with email {user.Email}");
                return user;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error during registration: {ex.Message}");
                throw new Exception("An error occurred during user registration.");
            }
        }

        public async Task<string?> Login(string email, string password)
        {
            try
            {
                User? user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
                if (user == null)
                {
                    _logger.LogWarning($"User with email {email} not found.");
                    return null;
                }

                if (user.PasswordHash == null)
                {
                    _logger.LogError($"Error PasswordHash can not be null: {user.PasswordHash}");
                    return null;
                }
                if (!VerifyPassword(password, user.PasswordHash))
                {
                    _logger.LogWarning($"Invalid password attempt for email {email}");
                    return null;
                }

                string token = _authService.GenerateToken(user.Id, "User");
                _logger.LogInformation($"User {email} logged in successfully.");
                return token;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error during login: {ex.Message}");
                throw new Exception("An error occurred during login.");
            }
=======
            if (await IsEmailTaken(email))
                return null;

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

        public async Task<User?> Login(string email, string password)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null || !VerifyPassword(password, user.PasswordHash))
                return null;

            return user;
>>>>>>> 0d50e16b2a6a77a4377ebb9f8c716686a9238ed9
        }

        public async Task<User?> GetUserById(int userId)
        {
<<<<<<< HEAD
            try
            {
                User? user = await _dbContext.Users.FindAsync(userId);
                if (user == null)
                {
                    _logger.LogWarning($"User with ID {userId} not found.");
                }
                return user;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error fetching user with ID {userId}: {ex.Message}");
                throw new Exception("An error occurred while fetching the user.");
            }
=======
            return await _dbContext.Users.FindAsync(userId);
>>>>>>> 0d50e16b2a6a77a4377ebb9f8c716686a9238ed9
        }

        public async Task<User?> UpdateUser(int userId, string name, string email, string password)
        {
<<<<<<< HEAD
            try
            {
                User? user = await _dbContext.Users.FindAsync(userId);
                if (user == null)
                {
                    _logger.LogWarning($"User with ID {userId} not found for update.");
                    return null;
                }

                user.Name = name;
                user.Email = email;
                if (!string.IsNullOrWhiteSpace(password))
                {
                    user.PasswordHash = HashPassword(password);
                }

                _dbContext.Users.Update(user);
                await _dbContext.SaveChangesAsync();
                _logger.LogInformation($"User with ID {userId} updated successfully.");
                return user;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating user with ID {userId}: {ex.Message}");
                throw new Exception("An error occurred while updating the user.");
            }
=======
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
>>>>>>> 0d50e16b2a6a77a4377ebb9f8c716686a9238ed9
        }

        public async Task<bool> DeleteUser(int userId)
        {
<<<<<<< HEAD
            try
            {
                User? user = await _dbContext.Users.FindAsync(userId);
                if (user == null)
                {
                    _logger.LogWarning($"User with ID {userId} not found for deletion.");
                    return false;
                }

                _dbContext.Users.Remove(user);
                await _dbContext.SaveChangesAsync();
                _logger.LogInformation($"User with ID {userId} deleted successfully.");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error deleting user with ID {userId}: {ex.Message}");
                throw new Exception("An error occurred while deleting the user.");
            }
        }

        private string HashPassword(string password)
        {
            try
            {
                using SHA256 sha256 = SHA256.Create();
                byte[] hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hash);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error hashing password: {ex.Message}");
                throw new Exception("An error occurred while hashing the password.");
            }
        }

        private bool VerifyPassword(string password, string hashedPassword)
        {
            try
            {
                string hash = HashPassword(password);
                return hash == hashedPassword;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error verifying password: {ex.Message}");
                throw new Exception("An error occurred while verifying the password.");
            }
=======
            var user = await _dbContext.Users.FindAsync(userId);
            if (user == null)
                return false;

            _dbContext.Users.Remove(user);
            await _dbContext.SaveChangesAsync();
            return true;
>>>>>>> 0d50e16b2a6a77a4377ebb9f8c716686a9238ed9
        }

        public async Task<bool> IsEmailTaken(string email)
        {
<<<<<<< HEAD
            try
            {
                return await _dbContext.Users.AnyAsync(u => u.Email == email);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error checking if email {email} is taken: {ex.Message}");
                throw new Exception("An error occurred while checking the email.");
            }
=======
            return await _dbContext.Users.AnyAsync(u => u.Email == email);
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
>>>>>>> 0d50e16b2a6a77a4377ebb9f8c716686a9238ed9
        }
    }
}
