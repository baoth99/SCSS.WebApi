using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCSS.Application.Admin.Models.AccountModels
{
    public class SearchAccountRequestModel : BaseFilterModel
    {
        public string Name { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public string Address { get; set; }

        public string IdCard { get; set; }

        public int Gender { get; set; }

        public int Role { get; set; }

        public int Status { get; set; }
    }
}
