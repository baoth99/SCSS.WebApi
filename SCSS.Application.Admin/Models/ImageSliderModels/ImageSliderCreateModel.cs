using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCSS.Application.Admin.Models.ImageSliderModels
{
    public class ImageSliderCreateModel
    {
        public IFormFile Image { get; set; }

    }
}
