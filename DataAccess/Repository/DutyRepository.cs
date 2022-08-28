using DataAccess.Contexts;
using DataAccess.Repository;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces.Repository
{
    public class DutyRepository : RepositoryBase<Duty>, IDutyRepository
    {
        public DutyRepository(AppDbContext context) : base(context)
        {
        }
    }
}
