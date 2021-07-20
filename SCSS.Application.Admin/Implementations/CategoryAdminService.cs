using Microsoft.EntityFrameworkCore;
using SCSS.Application.Admin.Interfaces;
using SCSS.Application.Admin.Models.AdminCategoryModels;
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
using System.IO;
using System.Linq;
using System.Linq.Dynamic.Core;
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
            string imageUrl = string.Empty;
            if (model.Image != null)
            {
                var fileName = CommonUtils.GetFileName(PrefixFileName.AdminCategory, model.Image.FileName);
                imageUrl = await _storageBlobS3Service.UploadFile(model.Image, fileName, FileS3Path.AdminCategoryImages);
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
                ImageUrl = imageUrl,
                Description = model.Description
            };

            _categoryAdminRepository.Insert(categoryAdmin);

            await UnitOfWork.CommitAsync();

            return BaseApiResponse.OK();
        }

        #endregion

        #region Get Category Admin Detail

        /// <summary>
        /// Gets the category admin detail.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public async Task<BaseApiResponseModel> GetCategoryAdminDetail(Guid id)
        {
            if (!_categoryAdminRepository.IsExisted(x => x.Id.Equals(id)))
            {
                return BaseApiResponse.NotFound(SystemMessageCode.DataNotFound);
            }

            var data = await _categoryAdminRepository.GetManyAsNoTracking(x => x.Id.Equals(id)).Join(_accountRepository.GetAllAsNoTracking(),
                                                                        x => x.CreatedBy, y => y.Id, (x, y) => new
                                                                        {
                                                                            CategoryAdminId = x.Id,
                                                                            CategoryName = x.Name,
                                                                            x.Description,
                                                                            x.ImageUrl,
                                                                            CreatedBy =  y.Name,
                                                                            x.CreatedTime,
                                                                            x.UnitId
                                                                        }).FirstOrDefaultAsync();


            var dataResult = new CategoryAdminViewDetailModel()
            {
                Id = data.CategoryAdminId,
                Image = data.ImageUrl,
                CreatedBy = data.CreatedBy,
                CreatedTime = data.CreatedTime.ToStringFormat(DateTimeFormat.DD_MM_yyyy_time),
                Description = data.Description,
                Name = data.CategoryName,
                UnitId = data.UnitId
            };

            return BaseApiResponse.OK(dataResult);
        }

        #endregion

        #region Remove Admin Category

        public async Task<BaseApiResponseModel> RemoveCategoryAdmin(Guid id)
        {
            if (!_categoryAdminRepository.IsExisted(x => x.Id.Equals(id)))
            {
                return BaseApiResponse.NotFound(SystemMessageCode.DataNotFound);
            }

            _categoryAdminRepository.RemoveById(id);

            await UnitOfWork.CommitAsync();

            return BaseApiResponse.OK();
        }

        #endregion

        #region Search Category Admin

        /// <summary>
        /// Searches the category admin.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public async Task<BaseApiResponseModel> SearchCategoryAdmin(SearchCategoryAdminModel model)
        {
            if (model == null)
            {
                return BaseApiResponse.Error(SystemMessageCode.DataInvalid);
            }

            var UnitId = CommonUtils.CheckGuid(model.Unit);


            var dataQuery = _categoryAdminRepository.GetManyAsNoTracking(x => (ValidatorUtil.IsBlank(model.Name) || x.Name.Contains(model.Name)) &&
                                                                              (ValidatorUtil.IsBlank(model.Description) || x.Description.Contains(model.Description)))
                                                                                .GroupJoin(_unitRepository.GetManyAsNoTracking(x => (ValidatorUtil.IsBlank(UnitId) || x.Id.Equals(UnitId))),
                                                                                    x => x.UnitId, y => y.Id, (x, y) => new
                                                                                    {
                                                                                        CategoryAdminId = x.Id,
                                                                                        SCName = x.Name,
                                                                                        Unit = y,
                                                                                        CreatedTime = x.CreatedTime,
                                                                                        x.CreatedBy
                                                                                    }).SelectMany(x => x.Unit.DefaultIfEmpty(), (x, y) => new
                                                                                    {
                                                                                        x.CategoryAdminId,
                                                                                        x.SCName,
                                                                                        UnitName = y.Name,
                                                                                        x.CreatedTime,
                                                                                        x.CreatedBy
                                                                                    }).Join(_accountRepository.GetManyAsNoTracking(x => (ValidatorUtil.IsBlank(model.CreatedBy) || x.Name.Contains(model.CreatedBy))), x => x.CreatedBy, y => y.Id,
                                                                                        (x, y) => new
                                                                                        {
                                                                                            Id = x.CategoryAdminId,
                                                                                            Name = x.SCName,
                                                                                            Unit = x.UnitName,
                                                                                            CreatedTime = x.CreatedTime,
                                                                                            CreatedBy = y.Name
                                                                                        }).OrderBy("CreatedTime DESC");




            var totalRecord = await dataQuery.CountAsync();

            var dataRes = dataQuery.Skip((model.Page - 1) * model.PageSize).Take(model.PageSize).Select(x => new CategoryAdminViewModel()
            {
                Id= x.Id,
                Name = x.Name,
                Unit = x.Unit,
                CreatedBy = x.CreatedBy,
                CreatedTime = x.CreatedTime.ToStringFormat(DateTimeFormat.DD_MM_yyyy_time)
            }).ToList();

            return BaseApiResponse.OK(resData: dataRes, totalRecord: totalRecord);
        }

        #endregion

        #region Edit Category Admin

        /// <summary>
        /// Edits the category admin.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public async Task<BaseApiResponseModel> EditCategoryAdmin(CategoryAdminEditModel model)
        {
            if (model == null)
            {
                return BaseApiResponse.Error(SystemMessageCode.DataInvalid);
            }

            var categoryAdmin = _categoryAdminRepository.GetById(model.Id);

            if (categoryAdmin == null)
            {
                return BaseApiResponse.NotFound(SystemMessageCode.DataNotFound);
            }

            if (!_unitRepository.IsExisted(x => x.Id.Equals(CommonUtils.CheckGuid(model.Unit))))
            {
                return BaseApiResponse.Error(SystemMessageCode.DataInvalid);
            }

            categoryAdmin.Name = model.Name;
            categoryAdmin.Description = model.Description;
            categoryAdmin.UnitId = CommonUtils.CheckGuid(model.Unit);

            if (model.IsDeleteImg)
            {
                categoryAdmin.ImageUrl = string.Empty;
            }

            if (model.ImageFile != null)
            {
                var fileName = CommonUtils.GetFileName(PrefixFileName.AdminCategory, model.ImageFile.FileName);
                string imageUrl = await _storageBlobS3Service.UploadFile(model.ImageFile, fileName, FileS3Path.AdminCategoryImages);
                categoryAdmin.ImageUrl = imageUrl;
            }
            
            _categoryAdminRepository.Update(categoryAdmin);
            await UnitOfWork.CommitAsync();
            return BaseApiResponse.OK();
        }

        #endregion

        #region Get Unit List

        /// <summary>
        /// Gets the unit list.
        /// </summary>
        /// <returns></returns>
        public async Task<BaseApiResponseModel> GetUnitList()
        {
            var data = await _unitRepository.GetAllAsNoTracking().Select(x => new UnitListViewModel()
            {
                Key = x.Id,
                Val = x.Name
            }).ToListAsync();

            return BaseApiResponse.OK(data);
        }

        #endregion
    }
}
