using DevFreela.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace DevFreela.Infrastructure.Persistence
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

            builder.Entity<UserSkill>(e =>
            {
                e.ToTable("UserSkills");
                e.HasKey(us => us.Id);
                e.HasIndex(us => new { us.IdUser, us.IdSkill }).IsUnique();

                e.HasOne(us => us.User)
                    .WithMany(u => u.Skills)
                    .HasForeignKey(us => us.IdUser)
                    .OnDelete(DeleteBehavior.Cascade);

                e.HasOne(us => us.Skill)
                    .WithMany(s => s.UserSkills)
                    .HasForeignKey(us => us.IdSkill)
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
                    
                    e.Property(p => p.TotalCost).HasPrecision(18,2);
                });
            

            builder
                .Entity<ProjectComment>(e =>
                {
                    e.HasKey(pc => pc.Id);

                    e.HasOne(p => p.Project)
                        .WithMany(p => p.Comments)
                        .HasForeignKey(pc => pc.IdProject)
                        .OnDelete(DeleteBehavior.Cascade);
                    
                    e.HasOne(pc => pc.User)
                        .WithMany(u => u.Comments)
                        .HasForeignKey(pc => pc.IdUser)   
                        .OnDelete(DeleteBehavior.Cascade);
                });

            builder
                .Entity<User>(e =>
                {
                    e.ToTable("Users");
                    e.HasKey(u => u.Id);

                    e.Property(u => u.FullName)
                        .IsRequired()
                        .HasMaxLength(150);

                    e.Property(u => u.Email)
                        .IsRequired()
                        .HasMaxLength(200);

                    e.HasMany(u => u.Skills)
                        .WithOne(us => us.User)
                        .HasForeignKey(us => us.IdUser)  
                        .OnDelete(DeleteBehavior.Cascade);

                    e.HasMany(u => u.OwnedProjects)
                        .WithOne(p => p.Client)
                        .HasForeignKey(p => p.IdClient)
                        .OnDelete(DeleteBehavior.Restrict);

                    e.HasMany(u => u.FreelanceProjects)
                        .WithOne(p => p.Freelancer)
                        .HasForeignKey(p => p.IdFreelancer)
                        .OnDelete(DeleteBehavior.Restrict);
                    
                });


            base.OnModelCreating(builder);
        }
    }
   
        
}

