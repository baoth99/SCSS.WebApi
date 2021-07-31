using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCSS.WebApi.SignalR.AdminHubs.IHubs
{
    public interface IAmountOfBookingHub
    {
        /// <summary>
        /// Gets the amount of booking in day.
        /// </summary>
        /// <param name="amount">The amount.</param>
        /// <returns></returns>
        Task GetAmountOfBookingInDay(int amount);
    }

    public interface IAmountOfTransactionHub
    {
        /// <summary>
        /// Gets the amount of transaction in day.
        /// </summary>
        /// <param name="amount">The amount.</param>
        /// <returns></returns>
        Task GetAmountOfTransactionInDay(int amount);
    }
}
