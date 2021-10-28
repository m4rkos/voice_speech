using System.Reflection;
using Microsoft.EntityFrameworkCore;

public class MyDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Filename=Database.db", options =>
        {
            options.MigrationsAssembly(Assembly.GetExecutingAssembly().FullName);
        });
        base.OnConfiguring(optionsBuilder);
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Map table names
        modelBuilder.Entity<User>().ToTable("Users", "test");
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId);
            entity.HasIndex(e => e.UserName).IsUnique();
            entity.HasIndex(e => e.Password).IsUnique();
            entity.Property(e => e.FullName);
            entity.Property(e => e.Birthday);
            entity.Property(e => e.Photo).HasDefaultValue("https://cdn1.iconfinder.com/data/icons/logos-brands-in-colors/231/among-us-player-white-512.png");
            entity.Property(e => e.DateTimeAdd).HasDefaultValueSql("CURRENT_TIMESTAMP");
        });
        base.OnModelCreating(modelBuilder);
    }
}