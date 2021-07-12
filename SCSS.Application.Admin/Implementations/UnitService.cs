using Microsoft.EntityFrameworkCore;
using SCSS.Application.Admin.Interfaces;
using SCSS.Application.Admin.Models.UnitModels;
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
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace SCSS.Application.Admin.Implementations
{
    public class UnitService : BaseService, IUnitService
    {
        #region Repositories

        /// <summary>
        /// The unit repository
        /// </summary>
        private readonly IRepository<Unit> _unitRepository;

        /// <summary>
        /// The account repository
        /// </summary>
        private readonly IRepository<Account> _accountRepository;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="UnitService"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <param name="userAuthSession">The user authentication session.</param>
        public UnitService(IUnitOfWork unitOfWork, IAuthSession userAuthSession) : base(unitOfWork, userAuthSession)
        {
            _unitRepository = UnitOfWork.UnitRepository;
            _accountRepository = UnitOfWork.AccountRepository;
        }

        #endregion

        #region Create Unit

        /// <summary>
        /// Creates the unit.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public async Task<BaseApiResponseModel> CreateUnit(CreateUnitModel model)
        {
            var entity = new Unit()
            {
                Name = model.Name
            };

            _unitRepository.Insert(entity);
            await UnitOfWork.CommitAsync();

            return BaseApiResponse.OK();
        }

        #endregion

        #region Remove Unit

        /// <summary>
        /// Removes the unit.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public async Task<BaseApiResponseModel> RemoveUnit(Guid id)
        {
            var entity = _unitRepository.GetById(id);
            if (entity == null)
            {
                return BaseApiResponse.NotFound(SystemMessageCode.DataNotFound);
            }

            _unitRepository.Remove(entity);
            await UnitOfWork.CommitAsync();
            return BaseApiResponse.OK();
        }

        #endregion

        #region Search Unit

        /// <summary>
        /// Searches the unit.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public async Task<BaseApiResponseModel> SearchUnit(SearchUnitModel model)
        {

            var dataQuery = _unitRepository.GetManyAsNoTracking(x => (ValidatorUtil.IsBlank(model.Name) || x.Name.Contains(model.Name)))
                                       .Join(_accountRepository.GetAllAsNoTracking(), x => x.CreatedBy, y => y.Id, (x, y) => new
                                       {
                                           Id = x.Id,
                                           Name = x.Name,
                                           CreatedBy = y.Name,
                                           CreatedTime = x.CreatedTime
                                       }).OrderBy("CreatedTime DESC");

            var totalRecord = await dataQuery.CountAsync();

            var dataRes = dataQuery.Skip((model.Page - 1) * model.PageSize).Take(model.PageSize).Select(x => new UnitViewModel()
            {
                Id = x.Id,
                Name = x.Name,
                CreatedTime = x.CreatedTime.ToStringFormat(DateTimeFormat.DD_MM_yyyy_time),
                CreatedBy = x.CreatedBy
            }).ToList();

            return BaseApiResponse.OK(resData: dataRes, totalRecord: totalRecord);
        }

        #endregion

        #region Update Unit

        /// <summary>
        /// Updates the unit.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public async Task<BaseApiResponseModel> UpdateUnit(EditUnitModel model)
        {
            var entity = _unitRepository.GetById(model.Id);
            if (entity == null)
            {
                return BaseApiResponse.NotFound(SystemMessageCode.DataNotFound);
            }

            entity.Name = model.Name;

            await UnitOfWork.CommitAsync();

            return BaseApiResponse.OK();
        }

        #endregion


    }
}
