using Microsoft.AspNetCore.Mvc;
using SCSS.AWSService.Interfaces;
using SCSS.AWSService.Models;
using SCSS.AWSService.Models.SQSModels;
using SCSS.QueueEngine.QueueEngines;
using SCSS.Utilities.Constants;
using SCSS.WebApi.SystemConstants;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCSS.WebApi.Controllers.AdminControllers
{
    [ApiVersion(ApiVersions.ApiVersionV1)]
    public class DemoController : BaseAdminController
    {
        //private ISQSPublisherService _SQSPublisherService;

        //private IQueueEngineFactory _queueEngineFactory;

        //private IStringCacheService _cacheService;

        //private ICacheListService _cacheListService;

        //public DemoController(ISQSPublisherService sQSPublisherService, IQueueEngineFactory queueEngineFactory, IStringCacheService cacheService, ICacheListService cacheListService)
        //{
        //    _SQSPublisherService = sQSPublisherService;
        //    _queueEngineFactory = queueEngineFactory;
        //    _cacheService = cacheService;
        //    _cacheListService = cacheListService;
        //}

        //[HttpGet]
        //[Route(AdminApiUrlDefinition.AccountApiUrl.Search + "/add-queue")]
        //public string TestQueue([FromQuery] string text)
        //{
        //    _queueEngineFactory.StringEngineRepo.PushQueue(text);

        //    return "DOne";
        //}

        //[HttpGet]
        //[Route(AdminApiUrlDefinition.AccountApiUrl.Search + "/get-queue")]
        //public List<string> TestGetQueue()
        //{
        //    return _queueEngineFactory.StringEngineRepo.GetAllQueue().ToList();
        //}

        //[HttpGet]
        //[Route(AdminApiUrlDefinition.AccountApiUrl.Search + "/demo-aws-queue")]
        //public async Task<List<string>> TestAWSQueue()//[FromQuery] NotificationMessageQueueModel model)
        //{
        //    //await _SQSPublisherService.NotificationMessageQueuePublisher.SendMessageAsync(model);

        //    var res = await _cacheListService.PendingCollectingRequestCache.GetAllAsync();

        //    var maxNumberOfRequestDays = await _cacheService.GetStringCacheAsync(CacheRedisKey.MaxNumberOfRequestDays);
        //    var receiveQuantity = await _cacheService.GetStringCacheAsync(CacheRedisKey.ReceiveQuantity);
        //    var requestQuantity = await _cacheService.GetStringCacheAsync(CacheRedisKey.RequestQuantity);

        //    var list = new List<string>() { maxNumberOfRequestDays, receiveQuantity, requestQuantity };
        //    return list;
        //}
    }
}
