using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCSS.Application.Admin.Models.AccountModels
{
    public class AccountViewResponseModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Phone { get; set; }

        public int? Gender { get; set; }

        public int? Status { get; set; }

        public int? Role { get; set; }

        public float? TotalPoint { get; set; }

    }
}
