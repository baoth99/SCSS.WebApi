using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCSS.Application.Admin.Models.ScrapCategoryModels
{
    public class ScrapCategoryViewModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string CreatedBy { get; set; }

        public int? Status { get; set; }

        public string CreatedTime { get; set; }

        public string UpdatedTime { get; set; }
    }
}
