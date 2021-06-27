using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCSS.Application.Admin.Models.AdminCategoryModels
{
    public class CategoryAdminViewDetailModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public Guid? UnitId { get; set; }

        public string Image { get; set; }

        public string Description { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? CreatedTime { get; set; }
    }
}
