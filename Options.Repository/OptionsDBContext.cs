using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Options.DbContext.Models;

namespace Options.DbContext
{
    public class OptionsDBContext : IdentityDbContext
    {
        public DbSet<Option> Options { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<Settings> Settings { get; set; }

        public OptionsDBContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Option>()
                .HasMany(p => p.RollOvers)
                //Folder has many child folders
                .WithOne()
                //Folder Id should be on ChildFolder
                .HasForeignKey(p => p.ParentOptionId)
                .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(modelBuilder);
        }
    }
}
