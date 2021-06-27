using SCSS.Application.Admin.Interfaces;
using SCSS.Application.Admin.Models.AdminCategoryModels;
using SCSS.AWSService.Interfaces;
using SCSS.Data.EF.Repositories;
using SCSS.Data.EF.UnitOfWork;
using SCSS.Data.Entities;
using SCSS.Utilities.AuthSessionConfig;
using SCSS.Utilities.BaseResponse;
using SCSS.Utilities.Constants;
using SCSS.Utilities.Helper;
using SCSS.Utilities.ResponseModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCSS.Application.Admin.Implementations
{
    public class CategoryAdminService : BaseService, ICategoryAdminService
    {

        #region Repositories

        /// <summary>
        /// The category admin repository
        /// </summary>
        private readonly IRepository<CategoryAdmin> _categoryAdminRepository;

        /// <summary>
        /// The unit repository
        /// </summary>
        private readonly IRepository<Unit> _unitRepository;

        /// <summary>
        /// The account repository
        /// </summary>
        private readonly IRepository<Account> _accountRepository;

        #endregion

        #region Services

        /// <summary>
        /// The storage BLOB s3 service
        /// </summary>
        private readonly IStorageBlobS3Service _storageBlobS3Service;

        #endregion

        #region Constructor

        public CategoryAdminService(IUnitOfWork unitOfWork, IAuthSession userAuthSession,
                                    IStorageBlobS3Service storageBlobS3Service) : base(unitOfWork, userAuthSession)
        {
            _categoryAdminRepository = unitOfWork.CategoryAdminRepository;
            _storageBlobS3Service = storageBlobS3Service;
            _unitRepository = unitOfWork.UnitRepository;
            _accountRepository = unitOfWork.AccountRepository;
        }

        #endregion


        #region Create AdminCategory

        /// <summary>
        /// Creates the admin category.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public async Task<BaseApiResponseModel> CreateCategoryAdmin(CreateCategoryAdminModel model)
        {
            string imageName = "";
            if (model.Image != null)
            {
                var fileName = CommonUtils.GetFileName(PrefixFileName.AdminCategory, model.Image.FileName);
                imageName = await _storageBlobS3Service.UploadFile(model.Image, fileName, FileS3Path.AdminCategoryImages);
            }

            var unit = _unitRepository.GetById(model.Unit);

            if (unit == null)
            {
                return BaseApiResponse.Error(SystemMessageCode.DataInvalid);
            }

            var categoryAdmin = new CategoryAdmin()
            {
                Name = model.Name,
                UnitId = model.Unit,
                ImageName = imageName,
                Description = model.Description
            };

            _categoryAdminRepository.Insert(categoryAdmin);

            await UnitOfWork.CommitAsync();

            return BaseApiResponse.OK();
        }

        #endregion

        #region Get Category Admin

        public async Task<BaseApiResponseModel> GetCategoryAdminDetail(Guid id)
        {
            if (!_categoryAdminRepository.IsExisted(x => x.Id.Equals(id)))
            {
                return BaseApiResponse.NotFound(SystemMessageCode.DataNotFound);
            }

            var data = _categoryAdminRepository.GetManyAsNoTracking(x => x.Id.Equals(id)).Join(_accountRepository.GetAllAsNoTracking(),
                                                                        x => x.CreatedBy, y => y.Id, (x, y) => new
                                                                        {
                                                                            CategoryAdminId = x.Id,
                                                                            CategoryName = x.Name,
                                                                            x.Description,
                                                                            x.ImageName,
                                                                            CreatedBy =  y.Name,
                                                                            x.CreatedTime,
                                                                            x.UnitId
                                                                        }).FirstOrDefault();
            string image = "";
            if (!ValidatorUtil.IsBlank(data.ImageName))
            {
                var imageBase64 = await _storageBlobS3Service.GetFile(data.ImageName, FileS3Path.AdminCategoryImages);
                image = imageBase64.Base64;
            }

            var dataResult = new CategoryAdminViewDetailModel()
            {
                Id = data.CategoryAdminId,
                Image = image,
                CreatedBy = data.CreatedBy,
                CreatedTime = data.CreatedTime,
                Description = data.Description,
                Name = data.CategoryName,
                UnitId = data.UnitId
            };

            return BaseApiResponse.OK(dataResult);
        }

        #endregion

        public async Task<BaseApiResponseModel> SearchCategoryAdmin(SearchCategoryAdminModel model)
        {
            Console.WriteLine(UserAuthSession.UserSession.Id);
            return BaseApiResponse.OK();
        }
    }
}
