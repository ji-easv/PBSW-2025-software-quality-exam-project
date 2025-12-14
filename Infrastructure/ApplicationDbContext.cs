using Microsoft.EntityFrameworkCore;
using Models.Models;

namespace Infrastructure;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<Dimensions> Dimensions { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<Box> Boxes { get; set; }

    public DbSet<Customer> Customers { get; set; }
    public DbSet<Address> Addresses { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Box>()
            .HasOne(o => o.Dimensions)
            .WithOne()
            .HasForeignKey<Box>("DimensionsId")
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Order>(order =>
        {
            order.HasMany(o => o.Boxes)
                .WithMany();
            
            order.HasOne(o => o.Customer)
                .WithMany()
                .HasForeignKey("CustomerEmail")
                .HasPrincipalKey(c => c.Email);
        });

        modelBuilder.Entity<Customer>(customer =>
        {
            customer.HasKey(c => c.Email);

            customer.HasOne(c => c.Address)
                .WithOne()
                .HasForeignKey<Customer>("AddressId")
                .OnDelete(DeleteBehavior.Cascade);
        });

        base.OnModelCreating(modelBuilder);
    }
}