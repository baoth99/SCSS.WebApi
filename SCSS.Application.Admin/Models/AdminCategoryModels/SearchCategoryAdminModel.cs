using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCSS.Application.Admin.Models.AdminCategoryModels
{
    public class SearchCategoryAdminModel : BaseFilterModel
    {
        public string Name { get; set; }

        public string Unit { get; set; }

        public string CreatedBy { get; set; }

        public string Description { get; set; }

    }
}
