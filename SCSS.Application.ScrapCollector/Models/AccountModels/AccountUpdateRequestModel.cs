using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCSS.Application.ScrapCollector.Models.AccountModels
{
    public class AccountUpdateRequestModel
    {
        public Guid Id { get; set; }

        public string Phone { get; set; }

        public string Name { get; set; }



    }
}
