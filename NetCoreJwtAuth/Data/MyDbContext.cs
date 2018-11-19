using Microsoft.EntityFrameworkCore;
using NetCoreJwtAuth.Entities;

namespace NetCoreJwtAuth.Data {
  public class MyDbContext : DbContext {
    public MyDbContext(DbContextOptions<MyDbContext> options)
      : base(options) { }

    public DbSet<AppUser> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder) =>
      modelBuilder.Entity<AppUser>().HasData(
        new AppUser { ID = 1, UserName = "user1", Password = "password1" },
        new AppUser { ID = 2, UserName = "user2", Password = "password2" }
      );
  }
}
