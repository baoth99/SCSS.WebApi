using Microsoft.EntityFrameworkCore;
using SCSS.Application.ScrapCollector.Interfaces;
using SCSS.Application.ScrapCollector.Models.ScrapCategoryModels;
using SCSS.AWSService.Interfaces;
using SCSS.Data.EF.Repositories;
using SCSS.Data.EF.UnitOfWork;
using SCSS.Data.Entities;
using SCSS.FirebaseService.Interfaces;
using SCSS.Utilities.AuthSessionConfig;
using SCSS.Utilities.BaseResponse;
using SCSS.Utilities.Constants;
using SCSS.Utilities.Helper;
using SCSS.Utilities.ResponseModel;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SCSS.Application.ScrapCollector.Implementations
{
    public class ScrapCategoryService : BaseService, IScrapCategoryService
    {
        #region Repositories

        /// <summary>
        /// The scrap category repository
        /// </summary>
        private readonly IRepository<ScrapCategory> _scrapCategoryRepository;

        /// <summary>
        /// The scrap category detail repository
        /// </summary>
        private readonly IRepository<ScrapCategoryDetail> _scrapCategoryDetailRepository;

        #endregion

        #region Constructor 

        /// <summary>
        /// Initializes a new instance of the <see cref="ScrapCategoryService"/> class.
        /// </summary>
        /// <param name="unitOfWork"></param>
        /// <param name="userAuthSession"></param>
        /// <param name="logger"></param>
        public ScrapCategoryService(IUnitOfWork unitOfWork, IAuthSession userAuthSession, ILoggerService logger, IFCMService fcmService) : base(unitOfWork, userAuthSession, logger, fcmService)
        {
            _scrapCategoryRepository = unitOfWork.ScrapCategoryRepository;
            _scrapCategoryDetailRepository = unitOfWork.ScrapCategoryDetailRepository;
        }

        #endregion

        #region Check Duplicate Scrap Category Name

        /// <summary>
        /// Checks the name of the scrap category.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public async Task<BaseApiResponseModel> CheckScrapCategoryName(string name)
        {
            var isDuplicate = await IsDuplicateSCName(name);
            if (isDuplicate)
            {
                return BaseApiResponse.Error(SystemMessageCode.DuplicateData);
            }

            return BaseApiResponse.OK();
        }

        #endregion

        #region Create New Scrap Category

        /// <summary>
        /// Creates the scrap category.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public async Task<BaseApiResponseModel> CreateScrapCategory(ScrapCategoryCreateModel model)
        {
            var isDuplicate = await IsDuplicateSCName(model.Name);

            if (isDuplicate)
            {
                return BaseApiResponse.Error(SystemMessageCode.DuplicateData);
            }

            var duplicateSCDetails = model.Details.GroupBy(x => x.Unit).Where(x => x.Count() > 1).Select(x => x.Key);

            if (duplicateSCDetails.Any())
            {
                return BaseApiResponse.Error(msgCode: SystemMessageCode.DataInvalid, resData: duplicateSCDetails.ToList());
            }

            var scrapCategory = new ScrapCategory()
            {
                AccountId = UserAuthSession.UserSession.Id,
                Status = ScrapCategoryStatus.ACTIVE,
                Name = model.Name,
                ImageUrl = model.ImageUrl
            };

            _scrapCategoryRepository.Insert(scrapCategory);

            var scrapCategoryId = scrapCategory.Id;

            var scDetails = model.Details.Select(x => new ScrapCategoryDetail()
            {
                Unit = x.Unit,
                Price = x.Price,
                ScrapCategoryId = scrapCategoryId,
                Status = ScrapCategoryStatus.ACTIVE
            }).ToList();

            _scrapCategoryDetailRepository.InsertRange(scDetails);

            await UnitOfWork.CommitAsync();

            return BaseApiResponse.OK();
        }

        #endregion

        #region Update Scrap Category

        /// <summary>
        /// Updates the scrap category.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public async Task<BaseApiResponseModel> UpdateScrapCategory(ScrapCategoryUpdateModel model)
        {
            var SCEntity = _scrapCategoryRepository.Get(x => (x.Id == model.Id) && (x.AccountId == UserAuthSession.UserSession.Id));

            if (SCEntity == null)
            {
                return BaseApiResponse.NotFound(SystemMessageCode.DataNotFound);
            }

            var duplicateSCDetails = model.Details.Where(x => x.Status == ScrapCategoryStatus.ACTIVE).GroupBy(x => x.Unit).Where(x => x.Count() > 1).Select(x => x.Key);

            if (duplicateSCDetails.Any())
            {
                return BaseApiResponse.Error(msgCode: SystemMessageCode.DataInvalid, resData: duplicateSCDetails.ToList());
            }

            var duplicateInsertSCDetails = model.Details.Where(x => x.Status == ScrapCategoryStatus.ACTIVE &&
                                                              ValidatorUtil.IsBlank(x.Id)).GroupBy(x => x.Unit).Where(x => x.Count() > 1).Select(x => x.Key);

            if (duplicateSCDetails.Any())
            {
                return BaseApiResponse.Error(msgCode: SystemMessageCode.DataInvalid, resData: duplicateInsertSCDetails.ToList());
            }

            SCEntity.Name = model.Name;
            SCEntity.ImageUrl = model.ImageUrl;

            _scrapCategoryRepository.Update(SCEntity);

            var SCDetailEntity = _scrapCategoryDetailRepository.GetManyAsNoTracking(x => x.ScrapCategoryId.Equals(model.Id) && x.Status == ScrapCategoryStatus.ACTIVE);

            // Get De-Active (Delete) Scrap Category Detail in Parameters
            var deActiveSCDetails = model.Details.Where(x => (x.Status == ScrapCategoryStatus.DEACTIVE) && (!ValidatorUtil.IsBlank(x.Id)))
                                            .Join(SCDetailEntity, x => x.Id, y => y.Id, (x, y) =>
                                            {
                                                y.Status = x.Status;
                                                return y;
                                            }).ToList();
            // De-Active (Delete) Scrap Category Detail
            if (deActiveSCDetails.Any())
            {
                _scrapCategoryDetailRepository.UpdateRange(deActiveSCDetails);
            }

            // Insert New Scrap Category Detail
            // Get Insert Scrap Category Detail
            var newSCDetailsParam = model.Details.Where(x => ValidatorUtil.IsBlank(x.Id)).ToList();

            // Insert 
            if (newSCDetailsParam.Any())
            {
                var insertSCDetailEntity = newSCDetailsParam.Select(x => new ScrapCategoryDetail()
                {
                    ScrapCategoryId = SCEntity.Id,
                    Price = x.Price,
                    Status = ScrapCategoryStatus.ACTIVE,
                    Unit = x.Unit
                }).ToList();

                _scrapCategoryDetailRepository.InsertRange(insertSCDetailEntity);
            }

            // Update Scrap Category Detail
            // Check New Scrap Category Detail with Old Scrap Category Detail
            var SCDetails = model.Details.Where(x => (x.Status == ScrapCategoryStatus.ACTIVE) && (!ValidatorUtil.IsBlank(x.Id)))
                                                .Join(_scrapCategoryDetailRepository.GetManyAsNoTracking(x => x.ScrapCategoryId.Equals(model.Id) && x.Status == ScrapCategoryStatus.ACTIVE),
                                                x => x.Id, y => y.Id, (x, y) =>
                                                {
                                                    if(!y.Unit.Equals(x.Unit) || y.Price != x.Price)
                                                    {
                                                        y.Status = ScrapCategoryStatus.DEACTIVE;
                                                    }
                                                    return y;
                                                }).ToList();

            // Get Old Scrap Category Detail
            var oldSCDetailEntity = SCDetails.Where(x => x.Status == ScrapCategoryStatus.DEACTIVE).ToList();

            // Remove Old Scrap Category Detail
            if (oldSCDetailEntity.Any())
            {
                _scrapCategoryDetailRepository.UpdateRange(oldSCDetailEntity);
            }

            // Get New Scrap Category Detail
            var newSCDetailEntity = oldSCDetailEntity.Join(model.Details.Where(x => x.Status == ScrapCategoryStatus.ACTIVE),
                                                        x => x.Id, y => y.Id, (x, y) => new ScrapCategoryDetail()
                                                        {
                                                            ScrapCategoryId = SCEntity.Id,
                                                            Unit = y.Unit,
                                                            Price = y.Price,
                                                            Status = ScrapCategoryStatus.ACTIVE
                                                        }).ToList();
            // Insert New Scrap Category Detail
            if (newSCDetailEntity.Any())
            {
                _scrapCategoryDetailRepository.InsertRange(newSCDetailEntity);
            }

            // Commit DB
            await UnitOfWork.CommitAsync();

            return BaseApiResponse.OK();
        }

        #endregion

        #region Get Scrap Category

        /// <summary>
        /// Gets the scrap categories.
        /// </summary>
        /// <returns></returns>
        public async Task<BaseApiResponseModel> GetScrapCategories()
        {
            var dataQuery = _scrapCategoryRepository.GetManyAsNoTracking(x => x.AccountId == UserAuthSession.UserSession.Id && x.Status == ScrapCategoryStatus.ACTIVE)
                                                    .Select(x => new ScrapCategoryViewModel()
                                                    {
                                                        Id = x.Id,
                                                        Name = x.Name,
                                                        ImageUrl = x.ImageUrl
                                                    }).OrderBy(x => x.Name);
            var totalRecord = await dataQuery.CountAsync();

            return BaseApiResponse.OK(totalRecord: totalRecord, resData: dataQuery.ToList());
        }

        #endregion

        #region Get Scrap Category Detail

        /// <summary>
        /// Gets the scrap category detail.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public async Task<BaseApiResponseModel> GetScrapCategoryDetail(Guid id)
        {
            var scrapCategoryEntity = await _scrapCategoryRepository.GetAsync(x => x.Id.Equals(id) && x.AccountId.Equals(UserAuthSession.UserSession.Id));

            if (scrapCategoryEntity == null)
            {
                return BaseApiResponse.NotFound(SystemMessageCode.DataNotFound);
            }


            var SCDetails = _scrapCategoryDetailRepository.GetManyAsNoTracking(x => x.ScrapCategoryId.Equals(scrapCategoryEntity.Id) && x.Status == ScrapCategoryStatus.ACTIVE)
                                                          .Select(x => new ScrapCategoryDetailViewModel()
                                                          {
                                                              Id = x.Id,
                                                              Price = x.Price,
                                                              Status = x.Status,
                                                              Unit = x.Unit
                                                          }).ToList();

            var dataResult = new ScrapCategoryViewDetailModel()
            {
                Id = scrapCategoryEntity.Id,
                Name = scrapCategoryEntity.Name,
                ImageUrl = scrapCategoryEntity.ImageUrl,
                Details = SCDetails
            };

            return BaseApiResponse.OK(dataResult);
        }

        #endregion

        #region Remove Scrap Category

        /// <summary>
        /// Removes the scrap category.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public async Task<BaseApiResponseModel> RemoveScrapCategory(Guid id)
        {
            var scrapCategoryEntity = await _scrapCategoryRepository.GetAsync(x => x.Id.Equals(id) && x.AccountId.Equals(UserAuthSession.UserSession.Id));

            if (scrapCategoryEntity == null)
            {
                return BaseApiResponse.NotFound(SystemMessageCode.DataNotFound);
            }

            scrapCategoryEntity.Status = ScrapCategoryStatus.DEACTIVE;
            _scrapCategoryRepository.Update(scrapCategoryEntity);

            var SCDetailEntities = _scrapCategoryDetailRepository.GetManyAsNoTracking(x => x.ScrapCategoryId.Equals(scrapCategoryEntity.Id) && x.Status == ScrapCategoryStatus.ACTIVE).ToList();
            foreach (var item in SCDetailEntities)
            {
                item.Status = ScrapCategoryStatus.DEACTIVE;
            }

            _scrapCategoryDetailRepository.UpdateRange(SCDetailEntities);

            await UnitOfWork.CommitAsync();

            return BaseApiResponse.OK();
        }

        #endregion

        #region Check Duplicate SC Name

        /// <summary>
        /// Determines whether [is duplicate sc name] [the specified name].
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>
        ///   <c>true</c> if [is duplicate sc name] [the specified name]; otherwise, <c>false</c>.
        /// </returns>
        private async Task<bool> IsDuplicateSCName(string name)
        {
            return await _scrapCategoryRepository.IsExistedAsync(x => x.Name.Equals(name) && x.Status == ScrapCategoryStatus.ACTIVE && x.AccountId.Equals(UserAuthSession.UserSession.Id));
        }

        #endregion
    }
}
