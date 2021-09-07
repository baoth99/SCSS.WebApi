using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace SCSS.Application.ScrapCollector.Models.ScrapCategoryModels
{
    public class ScrapCategoryCreateModel
    {
        public string Name { get; set; }

        public IFormFile Image { get; set; }

        public List<ScrapCategoryDetailCreateModel> Details { get; set; }
    }

    public class ScrapCategoryDetailCreateModel
    {
        public string Unit { get; set; }

        public long? Price { get; set; }
    }
}
