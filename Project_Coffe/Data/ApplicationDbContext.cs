using Microsoft.EntityFrameworkCore;
using Project_Coffe.Entities;

namespace Project_Coffe.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderProduct> OrderProducts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>()
                .HasOne(o => o.User)
                .WithMany()
                .HasForeignKey(o => o.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<OrderProduct>()
<<<<<<< HEAD
=======
                .HasOne(op => op.Order)
                .WithMany(o => o.OrderProducts)
                .HasForeignKey(op => op.OrderId);

            modelBuilder.Entity<OrderProduct>()
>>>>>>> 0d50e16b2a6a77a4377ebb9f8c716686a9238ed9
                .HasOne(op => op.Product)
                .WithMany()
                .HasForeignKey(op => op.ProductId);

<<<<<<< HEAD
            modelBuilder.Entity<OrderProduct>()
                .HasOne(op => op.Product)         
                .WithMany()
                .HasForeignKey(op => op.ProductId)  
                .OnDelete(DeleteBehavior.Cascade);  

=======
>>>>>>> 0d50e16b2a6a77a4377ebb9f8c716686a9238ed9
            modelBuilder.Entity<Product>()
                .Property(p => p.Price)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            base.OnModelCreating(modelBuilder);
        }
<<<<<<< HEAD

=======
>>>>>>> 0d50e16b2a6a77a4377ebb9f8c716686a9238ed9
    }
}
