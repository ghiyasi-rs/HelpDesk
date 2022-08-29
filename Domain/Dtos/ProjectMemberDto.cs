using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Dtos
{
    public class ProjectMemberDto
    {
        public int ProjectId { get; set; }
      
        public int UserId { get; set; }

        public List<Project> Project { get; set; }
        public List<User> User { get; set; }
    }
}
