using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Dtos
{
    public class ChangePassDto
    {
        public int Id { get; set; }
        public string CurrentPass { get; set; }
        public string NewPass { get; set; }

        public string ReNewPass { get; set; }

    }
}
