using Project_Coffe.Entities;

namespace Project_Coffe.Models.ModelInterface
{
    public interface IUserService
    {
        Task<User?> Register(string name, string email, string password);
        Task<string?> Login(string email, string password);
        Task<User?> GetUserById(int userId);
        Task<User?> UpdateUser(int userId, string name, string email, string password, string role);
        Task<bool> DeleteUser(int userId);
        Task<bool> IsEmailTaken(string email);
        public string MakeNormalRole(string role);
    }
}
