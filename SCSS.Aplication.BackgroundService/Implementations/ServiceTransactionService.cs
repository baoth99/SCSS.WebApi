using SCSS.Aplication.BackgroundService.Interfaces;
using SCSS.Data.EF.Repositories;
using SCSS.Data.EF.UnitOfWork;
using SCSS.Data.Entities;
using SCSS.Utilities.Constants;
using SCSS.Utilities.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCSS.Aplication.BackgroundService.Implementations
{
    public class ServiceTransactionService : BaseService, IServiceTransactionService
    {
        #region Repositories

        /// <summary>
        /// The service transaction repository
        /// </summary>
        private readonly IRepository<ServiceTransaction> _serviceTransactionRepository;

        /// <summary>
        /// The account repository
        /// </summary>
        private readonly IRepository<Account> _accountRepository;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceTransactionService"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        public ServiceTransactionService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _accountRepository = unitOfWork.AccountRepository;
            _serviceTransactionRepository = unitOfWork.ServiceTransactionRepository;
        }

        #endregion

        #region Summary Service Fee

        /// <summary>
        /// Summaries the sevice fee.
        /// </summary>
        public async Task SummarySeviceFee()
        {
            var dayInMonthNow = DateTimeVN.DATE_NOW.Day;

            var lastDayOfMonth = DateTime.DaysInMonth(DateTimeVN.DATE_NOW.Year, DateTimeVN.DATE_NOW.Month);

            if (dayInMonthNow == lastDayOfMonth)
            {
                var finshedServiceTransactions = _serviceTransactionRepository.GetAllAsNoTracking().ToList()
                                                                       .Select(x =>
                                                                       {
                                                                           x.IsFinished = BooleanConstants.TRUE;
                                                                           return x;
                                                                       }).ToList();

                _serviceTransactionRepository.UpdateRange(finshedServiceTransactions);

                // Create New SericeTransaction

                // Get Collector Role
                var collectorRole = UnitOfWork.RoleRepository.GetAsNoTracking(x => x.Key == AccountRole.COLLECTOR);

                var collectorAccounts = _accountRepository.GetManyAsNoTracking(x => x.RoleId == collectorRole.Id);

                var dateTimeFrom = DateTimeVN.DATE_NOW.AddMonths(1).GetFirstDayOfMonth();
                var dateTimeTo = dateTimeFrom.GetLastDayOfMonth();

                var newServiceTransactions = collectorAccounts.Select(x => new ServiceTransaction()
                {
                    CollectorId = x.Id,
                    Amount = 0,
                    IsFinished = BooleanConstants.FALSE,
                    Period = dateTimeFrom.ToStringFormat(DateTimeFormat.MMMM_yyyy),
                    DateTimeFrom = dateTimeFrom,
                    DateTimeTo = dateTimeTo,
                }).ToList();

                _serviceTransactionRepository.InsertRange(newServiceTransactions);

                await UnitOfWork.CommitAsync();
            }
        }

        #endregion


    }
}
