using DataAccess.Contexts;
using Domain.Entities;
using Domain.Interfaces.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class RequestRepository : RepositoryBase<Request>, IRequestRepository
    {
        public RequestRepository(AppDbContext context) : base(context)
        {
        }
    }
}
