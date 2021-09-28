using SCSS.Application.ScrapSeller.Models.CollectingRequestModels;
using SCSS.Utilities.BaseResponse;
using SCSS.Utilities.Constants;
using SCSS.Utilities.Extensions;
using SCSS.Utilities.ResponseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCSS.Application.ScrapSeller.Imlementations
{
    public partial class CollectingRequestService
    {
        #region Get Number Of Days Remaining that seller can request

        /// <summary>
        /// Gets the number of remaining days.
        /// </summary>
        /// <returns></returns>
        public async Task<BaseApiResponseModel> GetRemainingDays()
        {
            var maxNumberOfDays = await MaxNumberDaysSellerRequestAdvance();

            var days = new List<DateTime>();

            for (int i = 0; i < maxNumberOfDays; i++)
            {
                days.Add(DateTimeVN.DATE_NOW.AddDays(i).Date);
            }

            var maxNumberOfRequest = await MaxNumberCollectingRequestSellerRequest();

            var dataQuery = _collectingRequestRepository.GetManyAsNoTracking(x => CollectionConstants.RemainingCollectingRequest.Contains(x.Status.Value) &&
                                                                                          x.SellerAccountId.Equals(UserAuthSession.UserSession.Id))
                                                                                    .Select(x => x.CollectingRequestDate.Value.Date)
                                                                                    .GroupBy(x => x).Select(x => new
                                                                                    {
                                                                                        Count = x.Count(),
                                                                                        Date = x.Key
                                                                                    }).ToList();


            var collectingRequest = days.GroupJoin(dataQuery, x => x, y => y.Date, (x, y) => new
            {
                Date = x,
                ColletingRequests = y
            }).SelectMany(x => x.ColletingRequests.DefaultIfEmpty(), (x, y) => new
            {
                x.Date,
                y?.Count
            }).ToList();

            var remainingdays = new List<CollectingRequestRemainingDaysViewModel>();

            foreach (var item in collectingRequest)
            {
                if (item.Count == null)
                {
                    remainingdays.Add(new CollectingRequestRemainingDaysViewModel()
                    {
                        Count = maxNumberOfRequest,
                        Date = item.Date,
                    });
                    continue;
                }

                if (item.Count < maxNumberOfRequest)
                {
                    remainingdays.Add(new CollectingRequestRemainingDaysViewModel()
                    {
                        Count = maxNumberOfRequest - item.Count.ToIntValue(),
                        Date = item.Date,
                    });
                    continue;
                }
            }
            
            var total = remainingdays.Count;

            return BaseApiResponse.OK(totalRecord: total, resData: remainingdays);

        }

        #endregion Get Number Of Days Remaining that seller can request

        #region Check Seller Request Ability

        /// <summary>
        /// Checks the seller request ability.
        /// </summary>
        /// <returns></returns>
        public async Task<BaseApiResponseModel> CheckSellerRequestAbility()
        {
            var maxNumberOfDays = await MaxNumberDaysSellerRequestAdvance();

            var days = new List<DateTime>();

            for (int i = 0; i < maxNumberOfDays; i++)
            {
                days.Add(DateTimeVN.DATE_NOW.AddDays(i).Date);
            }

            var maxNumberOfRequest = await MaxNumberCollectingRequestSellerRequest();

            var dataQuery = _collectingRequestRepository.GetManyAsNoTracking(x => CollectionConstants.RemainingCollectingRequest.Contains(x.Status.Value) &&
                                                                                         x.SellerAccountId.Equals(UserAuthSession.UserSession.Id))
                                                                                   .Select(x => x.CollectingRequestDate.Value.Date)
                                                                                   .GroupBy(x => x).Select(x => new
                                                                                   {
                                                                                       Count = x.Count(),
                                                                                       Date = x.Key
                                                                                   }).ToList();


            var collectingRequest = days.GroupJoin(dataQuery, x => x, y => y.Date, (x, y) => new
            {
                Date = x,
                ColletingRequests = y
            }).SelectMany(x => x.ColletingRequests.DefaultIfEmpty(), (x, y) => new
            {
                x.Date,
                Count = y == null ? maxNumberOfRequest : maxNumberOfRequest - y.Count
            }).Select(x => x.Count).ToList();

            var total = collectingRequest.Sum();

            var dataResult = new CollectingRequestAbilityInfoViewModel
            {
                IsFull = total <= 0,
                NumberOfRemainingRequest = total,
            };

            return BaseApiResponse.OK(dataResult);
        }

        #endregion

    }
}
