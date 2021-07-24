using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCSS.Application.Admin.Models.AccountModels
{
    public class AccountStatusRequestModel
    {
        public Guid Id { get; set; }

        public int Status { get; set; }
    }
}
