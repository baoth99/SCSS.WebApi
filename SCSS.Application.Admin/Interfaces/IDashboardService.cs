using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCSS.Application.Admin.Interfaces
{
    public interface IDashboardService
    {
        Task<int> GetAmountOfBookingInDay(DateTime dateTimeFrom, DateTime dateTimeTo);

        Task<int> GetAmountOfTransactionInDay(DateTime dateTimeFrom, DateTime dateTimeTo);

        Task<int> GetAmountOfNewUser();


    }
}
