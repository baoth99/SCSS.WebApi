using Microsoft.EntityFrameworkCore;
using SCSS.Application.Admin.Interfaces;
using SCSS.AWSService.Interfaces;
using SCSS.Data.EF.Repositories;
using SCSS.Data.EF.UnitOfWork;
using SCSS.Data.Entities;
using SCSS.Utilities.AuthSessionConfig;
using SCSS.Utilities.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCSS.Application.Admin.Implementations
{
    public class DashboardService : BaseService, IDashboardService
    {
        #region Repositories

        /// <summary>
        /// The booking repository
        /// </summary>
        private readonly IRepository<Booking> _bookingRepository;

        /// <summary>
        /// The sell collect transaction repository
        /// </summary>
        private readonly IRepository<SellCollectTransaction> _sellCollectTransactionRepository;

        /// <summary>
        /// The account repository
        /// </summary>
        private readonly IRepository<Account> _accountRepository;

        #endregion

        #region Constructor

        public DashboardService(IUnitOfWork unitOfWork, IAuthSession userAuthSession, ILoggerService logger) : base(unitOfWork, userAuthSession, logger)
        {
            _bookingRepository = unitOfWork.BookingRepository;
            _sellCollectTransactionRepository = unitOfWork.SellCollectTransactionRepository;
            _accountRepository = unitOfWork.AccountRepository;

        }

        #endregion

        #region Get Amount Of Booking In Day

        public async Task<int> GetAmountOfBookingInDay(DateTime dateTimeFrom, DateTime dateTimeTo)
        {
            var dataQuery = UnitOfWork.UnitRepository.GetManyAsNoTracking(x => x.CreatedTime.Value.CompareTo(dateTimeFrom) >= 0 &&
                                                                        x.CreatedTime.Value.CompareTo(dateTimeTo) <= 0);
            return await dataQuery.CountAsync();
        }

        #endregion



        public Task<int> GetAmountOfNewUser()
        {
            return null;
        }

        #region Get Amount Of Transaction In Day

        public async Task<int> GetAmountOfTransactionInDay(DateTime dateTimeFrom, DateTime dateTimeTo)
        {
            var dataQuery = _sellCollectTransactionRepository.GetManyAsNoTracking(x => x.CreatedTime.Value.CompareTo(dateTimeFrom) >= 0 &&
                                                                                        x.CreatedTime.Value.CompareTo(dateTimeTo) <= 0);
            return await dataQuery.CountAsync();
        }

        #endregion

    }
}
