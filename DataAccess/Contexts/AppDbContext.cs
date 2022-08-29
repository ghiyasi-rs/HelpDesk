using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces.Database;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Contexts
{
    public partial class AppDbContext : IdentityDbContext, IAppDbContext
    {

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<User>().HasMany(x => x.Duties).WithOne(p => p.User).HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<User>().HasMany<ProjectMember>().WithOne(x => x.User).HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Project>().HasMany<ProjectMember>().WithOne(x => x.Project).HasForeignKey(x => x.ProjectId).OnDelete(DeleteBehavior.Restrict);

            #region User
            modelBuilder.Entity<User>().HasData(
                new { Id = 1, Name = "Sevda", LastName = "Ghiyasi", Email = "Sevda@gmail.com", Password = "123", Type = UserType.Admin });

            #endregion

        }

        public DbSet<Activity> Activities { get; set; }
        public DbSet<Duty> Duties { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<ProjectMember> ProjectMembers { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Request> Requests { get; set; }
    }


}
