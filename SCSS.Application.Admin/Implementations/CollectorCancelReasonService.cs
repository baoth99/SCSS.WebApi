using Microsoft.EntityFrameworkCore;
using SCSS.Application.Admin.Interfaces;
using SCSS.Application.Admin.Models.CollectorCancelReasonModels;
using SCSS.AWSService.Interfaces;
using SCSS.Data.EF.Repositories;
using SCSS.Data.EF.UnitOfWork;
using SCSS.Data.Entities;
using SCSS.Utilities.AuthSessionConfig;
using SCSS.Utilities.BaseResponse;
using SCSS.Utilities.ResponseModel;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SCSS.Application.Admin.Implementations
{
    public class CollectorCancelReasonService : BaseService, ICollectorCancelReasonService
    {
        #region Repositories

        /// <summary>
        /// The collector cancel reason repository
        /// </summary>
        private readonly IRepository<CollectorCancelReason> _collectorCancelReasonRepository;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="CollectorCancelReasonService"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <param name="userAuthSession">The user authentication session.</param>
        /// <param name="logger">The logger.</param>
        public CollectorCancelReasonService(IUnitOfWork unitOfWork, IAuthSession userAuthSession, ILoggerService logger) : base(unitOfWork, userAuthSession, logger)
        {
            _collectorCancelReasonRepository = unitOfWork.CollectorCancelReasonRepository;
        }

        #endregion

        #region Create New Cancel Reason

        /// <summary>
        /// Creates the new cancel reason.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public async Task<BaseApiResponseModel> CreateNewCancelReason(CollectorCancelReasonCreateModel model)
        {
            var entity = new CollectorCancelReason()
            {
                Content = model.Content
            };

            _collectorCancelReasonRepository.Insert(entity);

            await UnitOfWork.CommitAsync();

            return BaseApiResponse.OK();
        }

        #endregion


        #region Delete Cancel Reason

        /// <summary>
        /// Deletes the cancel reason.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public async Task<BaseApiResponseModel> DeleteCancelReason(Guid id)
        {
            var entity = _collectorCancelReasonRepository.GetById(id);
            if (entity == null)
            {
                return BaseApiResponse.NotFound();
            }
            _collectorCancelReasonRepository.Remove(entity);

            await UnitOfWork.CommitAsync();
            return BaseApiResponse.OK();
        }

        #endregion

        #region Update Cancel Reason

        /// <summary>
        /// Updates the cancel reason.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public async Task<BaseApiResponseModel> UpdateCancelReason(CollectorCancelReasonUpdateModel model)
        {
            var entity = _collectorCancelReasonRepository.GetById(model.Id);
            if (entity == null)
            {
                return BaseApiResponse.NotFound();
            }
            entity.Content = model.Content;

            _collectorCancelReasonRepository.Update(entity);

            await UnitOfWork.CommitAsync();
            return BaseApiResponse.OK();
        }


        #endregion

        #region Get Cancel Reasons

        /// <summary>
        /// Gets the cancel reasons.
        /// </summary>
        /// <returns></returns>
        public async Task<BaseApiResponseModel> GetCancelReasons()
        {
            var dataQuery = _collectorCancelReasonRepository.GetAllAsNoTracking().OrderBy(x => x.Content);

            var totalRecord = await dataQuery.CountAsync();

            var result = dataQuery.Select(x => new CollectorCancelReasonViewModel()
            {
                Id = x.Id,
                Content = x.Content
            }).ToList();

            return BaseApiResponse.OK(totalRecord: totalRecord, resData: result);
        }

        #endregion
    }
}
