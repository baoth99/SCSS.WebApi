using SCSS.Application.Admin.Models.ImageSliderModels;
using SCSS.Utilities.ResponseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCSS.Application.Admin.Interfaces
{
    public interface IImageSliderService
    {
        Task<BaseApiResponseModel> CreateNewImage(ImageSliderCreateModel model);

        Task<BaseApiResponseModel> GetImageSlider();

        Task<BaseApiResponseModel> GetImageSliderDetail(Guid id);

        Task<BaseApiResponseModel> ChangeSelectedImages(List<ActiveImageSliderModel> activeList);

        Task<BaseApiResponseModel> GetImages();
    }
}
