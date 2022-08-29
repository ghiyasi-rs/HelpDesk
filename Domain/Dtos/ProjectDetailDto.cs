using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Dtos
{
    public class ProjectDetailDto
    {
        public int Id { get; set; }    
        public string ProjectName { get; set; }
     
        public List<User> ProjectMember { get; set; }

        public List <DutyDto> Duties { get; set; }
         
    }
}
