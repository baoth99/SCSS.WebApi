using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCSS.WebApi.SignalR.AdminHubs.IHubs
{
    public interface IAmountOfCollectingRequestHub
    {
        /// <summary>
        /// Gets the amount of collecting request in day.
        /// </summary>
        /// <param name="amount">The amount.</param>
        /// <returns></returns>
        Task GetAmountOfCollectingRequestInDay(int amount);
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
