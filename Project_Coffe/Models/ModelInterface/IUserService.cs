using Project_Coffe.Entities;

namespace Project_Coffe.Models.ModelInterface
{
    public interface IUserService
    {
        Task<User?> Register(string name, string email, string password);
<<<<<<< HEAD
        Task<string?> Login(string email, string password);
=======
        Task<User?> Login(string email, string password);
>>>>>>> 0d50e16b2a6a77a4377ebb9f8c716686a9238ed9
        Task<User?> GetUserById(int userId);
        Task<User?> UpdateUser(int userId, string name, string email, string password);
        Task<bool> DeleteUser(int userId);
        Task<bool> IsEmailTaken(string email);
    }
}
