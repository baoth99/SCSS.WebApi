using SCSS.AWSService.Models;
using System;

namespace SCSS.Application.Admin.Models.ImageSliderModels
{
    public class ImageSliderViewDetailModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string CreatedTime { get; set; }

        public bool IsSelected { get; set; }

        public FileResponseModel Image { get; set; }      
    }
}
