using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCSS.Application.Admin.Models.ScrapCategoryModels
{
    public class ScrapCategoryViewDetailModel
    {
        public string Name { get; set; }

        public string CreatedBy { get; set; }

        public int? Status { get; set; }

        public string ImageUrl { get; set; }

        public string CreatedTime { get; set; }

        public string UpdatedTime { get; set; }

        public List<ScrapCategoryDetailViewItemModel> Items { get; set; }
    }

    public class ScrapCategoryDetailViewItemModel
    {
        public string Unit { get; set; }

        public long? Price { get; set; }

        public int? Status { get; set; }
    }
}
