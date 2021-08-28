using SCSS.AWSService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCSS.Application.Admin.Models.ImageSliderModels
{
    public class ImageSliderViewDetailModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string CreatedTime { get; set; }

        public bool IsSelected { get; set; }

        public FileViewModel Image { get; set; }      
    }
}
