using Microsoft.EntityFrameworkCore;
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
        #region Get Collecting Requests

        /// <summary>
        /// Gets the collecting requests.
        /// </summary>
        /// <returns></returns>
        public async Task<BaseApiResponseModel> GetCollectingRequests()
        {
            var dataQuery = _collectingRequestRepository.GetManyAsNoTracking(x => x.SellerAccountId.Equals(UserAuthSession.UserSession.Id))
                                                        .Join(_locationRepository.GetAllAsNoTracking(), x => x.LocationId, y => y.Id,
                                                             (x, y) => new
                                                             {
                                                                 x.Id,
                                                                 x.CollectingRequestCode,
                                                                 x.CollectingRequestDate,
                                                                 x.CreatedTime,
                                                                 x.Status,
                                                                 y.AddressName
                                                             });

            var totalRecord = await dataQuery.CountAsync();

            var dataResult = dataQuery.Select(x => new CollectingRequestViewModel()
            {
                Id = x.Id,
                AddressName = x.AddressName,
                CollectingRequestCode = x.CollectingRequestCode,
                CollectingRequestDate = x.CollectingRequestDate.ToStringFormat(DateTimeFormat.DD_MM_yyyy),
                CreatedTime = x.CreatedTime.ToStringFormat(DateTimeFormat.DD_MM_yyyy_time),
                Status = x.Status
            }).ToList();

            return BaseApiResponse.OK(totalRecord: totalRecord, resData: dataResult);
        }

        #endregion


        #region Get Collecting Request Detail 

        public async Task<BaseApiResponseModel> GetCollectingRequestDetail(Guid id)
        {

            return BaseApiResponse.OK();
        }

        #endregion

    }
}
