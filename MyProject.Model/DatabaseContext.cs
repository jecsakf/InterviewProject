using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MyProject.Model.Database;

namespace MyProject.Model
{
    public class DatabaseContext : IdentityDbContext<User,IdentityRole<int>,int>
    {
        public DatabaseContext() {}
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) {}

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<User>().ToTable("Users");
        }

        public virtual DbSet<User> AppUsers { get; set; } = null;
        public virtual DbSet<Npu> Npus { get; set; } = null;
        public virtual DbSet<Score> Scores { get; set; } = null;
        
    }
}
