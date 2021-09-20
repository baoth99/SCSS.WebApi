using Microsoft.EntityFrameworkCore;
using SCSS.Application.ScrapSeller.Interfaces;
using SCSS.Application.ScrapSeller.Models.CollectingRequestModels;
using SCSS.AWSService.Interfaces;
using SCSS.Data.EF.Repositories;
using SCSS.Data.EF.UnitOfWork;
using SCSS.Data.Entities;
using SCSS.Utilities.AuthSessionConfig;
using SCSS.Utilities.BaseResponse;
using SCSS.Utilities.Constants;
using SCSS.Utilities.Extensions;
using SCSS.Utilities.Helper;
using SCSS.Utilities.ResponseModel;
using SCSS.Validations.InvalidResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCSS.Application.ScrapSeller.Imlementations
{
    public class CollectingRequestService : BaseService, ICollectingRequestService
    {
        #region Repositories

        /// <summary>
        /// The collecting request repository
        /// </summary>
        private readonly IRepository<CollectingRequest> _collectingRequestRepository;

        /// <summary>
        /// The location repository
        /// </summary>
        private readonly IRepository<Location> _locationRepository;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="CollectingRequestService"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <param name="userAuthSession">The user authentication session.</param>
        /// <param name="logger">The logger.</param>
        public CollectingRequestService(IUnitOfWork unitOfWork, IAuthSession userAuthSession, ILoggerService logger) : base(unitOfWork, userAuthSession, logger)
        {
            _collectingRequestRepository = unitOfWork.CollectingRequestRepository;
            _locationRepository = unitOfWork.LocationRepository;
        }

        #endregion

        #region Request Scrap Collecting 

        /// <summary>
        /// Requests the scrap collecting.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public async Task<BaseApiResponseModel> RequestScrapCollecting(CollectingRequestCreateModel model)
        {
            var collectingRequestFromTime = model.FromTime.ToTimeSpan().Value;
            var collectingRequestToTime = model.ToTime.ToTimeSpan().Value;

            // Validate Collecting Request Time
            var errorList = ValidateCollectingRequestTime(model.CollectingRequestDate.ToDateTime(), collectingRequestFromTime, collectingRequestToTime);

            if (errorList.Any())
            {
                return BaseApiResponse.Error(SystemMessageCode.DataInvalid, errorList);
            }

            var sellerAccountId = UserAuthSession.UserSession.Id;

            // Create new Collecting Request Location Entity
            var locationEntity = new Location()
            {
                Address = model.Address,
                AddressName = model.AddressName,
                Latitude = model.Latitude,
                Longitude = model.Longtitude
            };

            // Insert Collecting Request Location Entity
            var locationInsertEntity = _locationRepository.Insert(locationEntity);

            // Auto Generate CollectingRequestEntityCode from CollectingRequestDate, collectingRequestFromTime and collectingRequestToTime
            var collectingRequestEntityCode = await GenerateCollectingRequestCode(model.CollectingRequestDate.ToDateTime().Value, collectingRequestFromTime, collectingRequestToTime);

            // Create new Collecting Request Entity
            var collectingRequestEntity = new CollectingRequest()
            {
                CollectingRequestCode = collectingRequestEntityCode,
                CollectingRequestDate = model.CollectingRequestDate.ToDateTime(),
                TimeFrom = collectingRequestFromTime,
                TimeTo = collectingRequestToTime,
                SellerAccountId = sellerAccountId,
                IsBulky = model.IsBulky,
                ScrapImageUrl = model.CollectingRequestImageUrl,
                Note = model.Note,
                LocationId = locationInsertEntity.Id
            };

            // Insert Collecting Request Entity 
            _collectingRequestRepository.Insert(collectingRequestEntity);

            // Commit Data into Database
            await UnitOfWork.CommitAsync();

            return BaseApiResponse.OK();
        }



        /// <summary>
        /// Generates the collecting request code.
        /// </summary>
        /// <param name="collectingRequestDate">The collecting request date.</param>
        /// <param name="fromTime">From time.</param>
        /// <param name="toTime">To time.</param>
        /// <returns></returns>
        private async Task<string> GenerateCollectingRequestCode(DateTime collectingRequestDate, TimeSpan fromTime, TimeSpan toTime)
        {
            var collectingRequestCount = await _collectingRequestRepository.GetAllAsNoTracking().CountAsync();

            string collectingRequestDateCode = collectingRequestDate.ToDateCode(DateCodeFormat.DDMMYYYY);
            string fromTimeCode = fromTime.ToTimeSpanCode(TimeSpanCodeFormat.HHMM);
            string toTimeCode = toTime.ToTimeSpanCode(TimeSpanCodeFormat.HHMM);

            var collectingRequestCode = string.Format(GenerationCodeFormat.COLLECTING_REQUEST_CODE, collectingRequestDateCode,
                                                                fromTimeCode,
                                                                toTimeCode,
                                                                collectingRequestCount);
            return collectingRequestCode;
        }

        #endregion

        #region Cancel Scrap Collecting Request

        /// <summary>
        /// Cancels the collecting request.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public async Task<BaseApiResponseModel> CancelCollectingRequest(CollectingRequestCancelModel model)
        {
            // Get Collecting Request from ID, sellerAccountId and Status = CollectingRequestStatus.PENDING (1)
            var collectingRequestEntity = await _collectingRequestRepository.GetAsyncAsNoTracking(x => x.Id.Equals(model.Id) &&
                                                                                                 x.SellerAccountId.Equals(UserAuthSession.UserSession.Id) &&
                                                                                                 x.Status == CollectingRequestStatus.PENDING &&
                                                                                                 x.CollectorAccountId == null);

            // Check Collecting Request existed
            if (collectingRequestEntity == null)
            {
                return BaseApiResponse.NotFound();
            }

            // Update Status and Cancel Reason
            collectingRequestEntity.Status = CollectingRequestStatus.CANCEL_BY_SELLER;
            collectingRequestEntity.CancelReason = model.CancelReason;

            try
            {
                _collectingRequestRepository.Update(collectingRequestEntity);
                // Commit Data to Database
                await UnitOfWork.CommitAsync();
            }
            catch (Exception)
            {
                return BaseApiResponse.Error();
            }

            return BaseApiResponse.OK();
        }

        #endregion      

        #region Validate Collecting Request Time

        /// <summary>
        /// Validates the collecting request time.
        /// </summary>
        /// <param name="fromTime">From time.</param>
        /// <param name="toTime">To time.</param>
        /// <returns></returns>
        private List<ValidationError> ValidateCollectingRequestTime(DateTime? collectingRequestDate, TimeSpan? fromTime, TimeSpan? toTime)
        {
            var errorList = new List<ValidationError>();

            //  Check CollectingRequestFromTime with CollectingRequestToTime
            //  If CollectingRequestFromTime is greater than CollectingRequestFromTime 
            if (fromTime.IsCompareTimeSpanGreaterThan(toTime))
            {
                errorList.Add(new ValidationError("collectingRequestDate", InvalidCollectingRequestCode.FromTimeGreaterThanToTime));
            }

            // Check CollectingRequestFromTime and CollectingRequestToTime in day
            if (collectingRequestDate.IsCompareDateTimeEqual(DateTimeInDay.DATE_NOW))
            {
                if (fromTime.IsCompareTimeSpanLessThan(DateTimeInDay.TIMESPAN_NOW.StripMilliseconds()))
                {
                    errorList.Add(new ValidationError("fromTime", InvalidCollectingRequestCode.FromTimeGreaterThanToTime));
                }
                if (toTime.IsCompareTimeSpanLessThan(DateTimeInDay.TIMESPAN_NOW.StripMilliseconds()))
                {
                    errorList.Add(new ValidationError("toTime", InvalidCollectingRequestCode.FromTimeGreaterThanToTime));
                }
            }

            // Check minutes between fromTime and toTime. if it less than 15 minutes => error
            if (!DateTimeUtils.IsMoreThanMinutes(fromTime, toTime))
            {
                errorList.Add(new ValidationError("between", InvalidCollectingRequestCode.LessThan15Minutes));
            }

            return errorList;
        }

        #endregion
    }
}
