using System;
using System.Collections.Generic;

namespace SCSS.Application.ScrapCollector.Models.ScrapCategoryModels
{
    public class ScrapCategoryViewDetailModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string ImageUrl { get; set; }

        public List<ScrapCategoryDetailViewModel> Details { get; set; }

    }

    public class ScrapCategoryDetailViewModel
    {
        public Guid Id { get; set; }

        public string Unit { get; set; }

        public long? Price { get; set; }

        public int? Status { get; set; }
    }
}
