using AI_Coffee_Advisor.Entities;
using Microsoft.EntityFrameworkCore;

namespace AI_Coffee_Advisor.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<UserPreference> UserPreferences { get; set; }
    }
}
