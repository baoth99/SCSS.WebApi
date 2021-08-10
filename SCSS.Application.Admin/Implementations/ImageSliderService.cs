using Microsoft.EntityFrameworkCore;
using SCSS.Application.Admin.Interfaces;
using SCSS.Application.Admin.Models.ImageSliderModels;
using SCSS.AWSService.Interfaces;
using SCSS.Data.EF.Repositories;
using SCSS.Data.EF.UnitOfWork;
using SCSS.Data.Entities;
using SCSS.Utilities.AuthSessionConfig;
using SCSS.Utilities.BaseResponse;
using SCSS.Utilities.Constants;
using SCSS.Utilities.Extensions;
using SCSS.Utilities.Helper;
using SCSS.Utilities.ResponseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCSS.Application.Admin.Implementations
{
    public class ImageSliderService : BaseService, IImageSliderService
    {
        #region Repositories

        /// <summary>
        /// The image slider repository
        /// </summary>
        private readonly IRepository<ImageSlider> _imageSliderRepository;


        #endregion

        #region Services

        /// <summary>
        /// The storage BLOB s3 service
        /// </summary>
        private readonly IStorageBlobS3Service _storageBlobS3Service;

        #endregion

        #region Constructor

        public ImageSliderService(IUnitOfWork unitOfWork, IAuthSession userAuthSession, IStorageBlobS3Service storageBlobS3Service) : base(unitOfWork, userAuthSession)
        {
            _imageSliderRepository = unitOfWork.ImageSliderRepository;
            _storageBlobS3Service = storageBlobS3Service;
        }

        #endregion


        #region Create New Image

        /// <summary>
        /// Creates the new image.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public async Task<BaseApiResponseModel> CreateNewImage(ImageSliderCreateModel model)
        {
            string imageUrl = string.Empty;

            if (model.Image != null)
            {
                var fileName = CommonUtils.GetFileName(PrefixFileName.ImageSlider, model.Image.FileName);
                imageUrl = await _storageBlobS3Service.UploadFile(model.Image, fileName, FileS3Path.ImageSliderImages);

                var entity = new ImageSlider()
                {
                    Name = model.Image.FileName,
                    ImageUrl = imageUrl,
                    IsSelected = BooleanConstants.FALSE
                };

                _imageSliderRepository.Insert(entity);

                await UnitOfWork.CommitAsync();
            }

            return BaseApiResponse.OK();
        }

        #endregion


        #region Get Image Slider

        public async Task<BaseApiResponseModel> GetImageSlider()
        {
            var dataQuery = _imageSliderRepository.GetAllAsNoTracking().Select(x => new ImageSliderViewModel()
            {
                Id = x.Id,
                ImageUrl = x.ImageUrl,
                IsSelected = x.IsSelected,
                Name = x.Name
            });

            var total = await dataQuery.CountAsync();

            return BaseApiResponse.OK(resData: dataQuery.ToList(), totalRecord: total);
        }

        #endregion

        #region Get Image Slider Detail

        /// <summary>
        /// Gets the image slider detail.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public async Task<BaseApiResponseModel> GetImageSliderDetail(Guid id)
        {
            if (_imageSliderRepository.IsExisted(x => x.Id.Equals(id)))
            {
                return BaseApiResponse.NotFound();
            }
            var dataQuery = _imageSliderRepository.GetById(id);

            var fileViewModel = await _storageBlobS3Service.GetImage(dataQuery.ImageUrl);

            var resData = new ImageSliderViewDetailModel()
            {
                Id = dataQuery.Id,
                Name = dataQuery.Name,
                Image = fileViewModel.Data,
                IsSelected = dataQuery.IsSelected,
                CreatedTime = dataQuery.CreatedTime.ToStringFormat(DateTimeFormat.DD_MM_yyyy_time),
            };

            return BaseApiResponse.OK(resData);
        }

        #endregion

    }
}
