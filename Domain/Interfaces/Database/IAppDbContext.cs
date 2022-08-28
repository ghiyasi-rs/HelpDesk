using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;


namespace Domain.Interfaces.Database
{
    public interface IAppDbContext
    {
        DbSet<Activity> Activities { get; set; }
        DbSet<Duty> Duties { get; set; }
        DbSet<Project> Projects { get; set; }
        DbSet<ProjectMember> ProjectMembers { get; set; }
        DbSet<User> Users { get; set; }
    }
}
