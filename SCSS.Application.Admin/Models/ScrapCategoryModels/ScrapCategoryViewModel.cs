using System;

namespace SCSS.Application.Admin.Models.ScrapCategoryModels
{
    public class ScrapCategoryViewModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string CreatedBy { get; set; }

        public int? Status { get; set; }

        public int? Role { get; set; }

        public string CreatedTime { get; set; }

    }
}
