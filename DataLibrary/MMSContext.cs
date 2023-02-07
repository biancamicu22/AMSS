using System;
using DataLibrary.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DataLibrary
{
    public partial class MMSContext : IdentityDbContext<User>
    {
        public MMSContext()
        {
        }

        public MMSContext(DbContextOptions<MMSContext> options)
            : base(options)
        {
        }
        public virtual DbSet<Comment> Comments { get; set; }
        public virtual DbSet<Sprint> Sprints { get; set; }
        public virtual DbSet<Models.AppTask> Tasks { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                throw new Exception("options builder not configured");
            }
            optionsBuilder.UseLazyLoadingProxies();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
            });

            modelBuilder.Entity<AppTask>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.HasOne<User>(t => t.User)
                    .WithMany(u => u.Tasks)
                    .HasForeignKey(t => t.User_Id).OnDelete(DeleteBehavior.NoAction);

                entity.HasOne<Sprint>(t => t.Sprint)
                    .WithMany(t => t.Tasks)
                    .HasForeignKey(t => t.Sprint_Id).OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Comment>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.HasOne<User>(c => c.User)
                    .WithMany(u => u.Comments)
                    .HasForeignKey(c => c.User_Id).OnDelete(DeleteBehavior.Cascade);


                entity.HasOne<AppTask>(c => c.Task)
                    .WithMany(t => t.Comments)
                    .HasForeignKey(c => c.Task_Id).OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Sprint>(entity =>
            {
                entity.HasKey(e => e.Id);

            });


            OnModelCreatingPartial(modelBuilder);
            base.OnModelCreating(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
