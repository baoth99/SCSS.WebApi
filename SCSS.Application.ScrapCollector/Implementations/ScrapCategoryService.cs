using SCSS.Application.ScrapCollector.Interfaces;
using SCSS.Application.ScrapCollector.Models.ScrapCategoryModels;
using SCSS.AWSService.Interfaces;
using SCSS.Data.EF.Repositories;
using SCSS.Data.EF.UnitOfWork;
using SCSS.Data.Entities;
using SCSS.Utilities.AuthSessionConfig;
using SCSS.Utilities.ResponseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        #region Services

        /// <summary>
        /// The storage BLOB service
        /// </summary>
        private readonly IStorageBlobS3Service _storageBlobService;

        #endregion

        #region Constructor 


        public ScrapCategoryService(IUnitOfWork unitOfWork, IAuthSession userAuthSession, ILoggerService logger,
                                    IStorageBlobS3Service storageBlobS3Service) : base(unitOfWork, userAuthSession, logger)
        {
            _scrapCategoryRepository = unitOfWork.ScrapCategoryRepository;
            _scrapCategoryDetailRepository = unitOfWork.ScrapCategoryDetailRepository;
            _storageBlobService = storageBlobS3Service;
        }

        #endregion


        #region Create New Scrap Category


        public async Task<BaseApiResponseModel> CreateScrapCategory(ScrapCategoryCreateModel model)
        {
            return null;
        }

        #endregion




    }
}
