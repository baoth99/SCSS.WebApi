using Microsoft.EntityFrameworkCore;
using SCSS.Application.Admin.Interfaces;
using SCSS.Application.Admin.Models.ImageSliderModels;
using SCSS.AWSService.Interfaces;
using SCSS.AWSService.Models;
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
using System.Drawing;
using System.IO;
using System.Linq;
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


        /// <summary>
        /// The cache service
        /// </summary>
        private readonly ICacheService _cacheService;

        #endregion

        #region Constructor

        public ImageSliderService(IUnitOfWork unitOfWork, IAuthSession userAuthSession, ILoggerService logger,
                                  IStorageBlobS3Service storageBlobS3Service,
                                  ICacheService cacheService) : base(unitOfWork, userAuthSession, logger)
        {
            _imageSliderRepository = unitOfWork.ImageSliderRepository;
            _storageBlobS3Service = storageBlobS3Service;
            _cacheService = cacheService;
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
            if (model.Image != null)
            {
                var fileName = CommonUtils.GetFileName(PrefixFileName.ImageSlider, model.Image.FileName);
                string imageUrl = await _storageBlobS3Service.UploadFile(model.Image, fileName, FileS3Path.ImageSliderImages);
                var entity = new ImageSlider()
                {
                    Name = Path.GetFileNameWithoutExtension(model.Image.FileName),
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

        /// <summary>
        /// Gets the image slider.
        /// </summary>
        /// <returns></returns>
        public async Task<BaseApiResponseModel> GetImageSlider()
        {
            var dataQuery = _imageSliderRepository.GetAllAsNoTracking().Select(x => new ImageSliderViewModel()
            {
                Id = x.Id,
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
            if (!_imageSliderRepository.IsExisted(x => x.Id.Equals(id)))
            {
                return BaseApiResponse.NotFound();
            }
            var dataQuery = _imageSliderRepository.GetById(id);

            var fileResultModel = await _storageBlobS3Service.GetFile(dataQuery.ImageUrl);

            var fileResponse = new FileResponseModel()
            {
                Base64 = fileResultModel.Stream.ToBase64(),
                Extension = fileResultModel.Extension
            };

            var resData = new ImageSliderViewDetailModel()
            {
                Id = dataQuery.Id,
                Name = dataQuery.Name,
                Image = fileResponse,
                IsSelected = dataQuery.IsSelected,
                CreatedTime = dataQuery.CreatedTime.ToStringFormat(DateTimeFormat.DD_MM_yyyy_time),
            };

            return BaseApiResponse.OK(resData);
        }

        #endregion

        #region Changes Images to show

        /// <summary>
        /// Actives the image.
        /// </summary>
        /// <param name="activeList">The active list.</param>
        /// <returns></returns>
        public async Task<BaseApiResponseModel> ChangeSelectedImages(List<ActiveImageSliderModel> list)
        {
            var selectedItems = list.Join(_imageSliderRepository.GetAllAsNoTracking(), x => x.Id, y => y.Id,
                                        (x, y) =>
                                        {
                                            y.IsSelected = x.IsSelected;
                                            return y;
                                        }).ToList();

            _imageSliderRepository.UpdateRange(selectedItems);

            await UnitOfWork.CommitAsync();

            await SaveCache();
            
            return BaseApiResponse.OK();
        }

        #endregion

        #region Get Images

        /// <summary>
        /// Gets the images.
        /// </summary>
        /// <returns></returns>
        public async Task<BaseApiResponseModel> GetImages(bool isWeb = false)
        {
            var cacheData = await _cacheService.GetCacheData(CacheRedisKey.ImageSlider);

            if (cacheData == null)
            {
                await SaveCache();
                cacheData = await _cacheService.GetCacheData(CacheRedisKey.ImageSlider);
            }

            var listData = cacheData.ToList<FileResponseModel>();

            if (listData.Count > 0)
            {
                if (isWeb)
                {
                    return BaseApiResponse.OK(listData);
                }
                var result = listData.Select(x => x.Base64.ToBitmap());

                return BaseApiResponse.OK(result);
            }

            return BaseApiResponse.OK(CollectionConstants.Empty<Bitmap>());

        }

        #endregion

        #region Save Data to Cache

        /// <summary>
        /// Saves the cache data.
        /// </summary>
        private async Task SaveCache()
        {
            var selectedList = _imageSliderRepository.GetManyAsNoTracking(x => x.IsSelected).Select(x => x.ImageUrl).ToList();

            if (selectedList.Count > 0)
            {
                var imgCache = new List<FileResponseModel>();

                foreach (var item in selectedList)
                {
                    var file = await _storageBlobS3Service.GetFile(item);
                    var image = new FileResponseModel()
                    {
                        Extension = file.Extension,
                        Base64 = file.Stream.ToBase64()
                    };
                    imgCache.Add(image);
                }

                if (imgCache.Count > 0)
                {
                    var cacheData = imgCache.ToJson();
                    await _cacheService.SetCacheData(CacheRedisKey.ImageSlider, cacheData);
                }
            }
            else
            {
                await _cacheService.RemoveCacheData(CacheRedisKey.ImageSlider);
            }
        }

        #endregion

    }
}
