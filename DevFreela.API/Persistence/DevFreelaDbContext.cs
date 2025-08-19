using DevFreela.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace DevFreela.API.Persistence
{
    public class DevFreelaDbContext : DbContext
    {
        public DevFreelaDbContext(DbContextOptions<DevFreelaDbContext> options) : base(options)
        {
        }

        public DbSet<Project> Projects { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<ProjectComment> ProjectComments { get; set; }
        public DbSet<UserSkill> UserSkills { get; set; }
        public DbSet<Skill> Skills { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder
                .Entity<Skill>(e=>
                {
                    e.HasKey(s => s.Id);
                });

            builder
                .Entity<UserSkill>(e =>
                {
                    e.HasKey(us => us.Id);

                    e.HasOne(u => u.Skill)
                        .WithMany(u => u.UserSkills)
                        .HasForeignKey(s => s.IdSkill)
                        .OnDelete(DeleteBehavior.Restrict);
                });
            builder
                .Entity<Skill>(e =>
                {
                    e.HasKey(s => s.Id);
                    e.HasIndex(s => s.Description).IsUnique();
                });

            builder
                .Entity<Project>(e =>
                {
                    e.HasKey(p => p.Id);

                    e.HasOne(fp =>fp.Freelancer)
                        .WithMany(fp => fp.FreelanceProjects)
                        .HasForeignKey(p => p.IdFreelancer)
                        .OnDelete(DeleteBehavior.Restrict);

                    e.HasOne(p => p.Client)
                        .WithMany(c => c.OwnedProjects)
                        .HasForeignKey(c => c.IdClient)
                        .OnDelete(DeleteBehavior.Restrict);
                });

            builder
                .Entity<ProjectComment>(e =>
                {
                    e.HasKey(pc => pc.Id);

                    e.HasOne(p => p.Project)
                        .WithMany(p => p.Comments)
                        .HasForeignKey(pc => pc.IdProject)
                        .OnDelete(DeleteBehavior.Cascade);
                });

            builder
                .Entity<User>(e =>
                {
                    e.HasKey(u => u.Id);
                    e.HasMany(u => u.Skills)
                        .WithOne(us => us.User)
                        .HasForeignKey(us => us.Id)
                        .OnDelete(DeleteBehavior.Cascade);
                });


            base.OnModelCreating(builder);
        }
    }
   
        
}

