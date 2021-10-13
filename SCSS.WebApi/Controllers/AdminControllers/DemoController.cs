using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SCSS.AWSService.Interfaces;
using SCSS.AWSService.Models;
using SCSS.AWSService.Models.SQSModels;
using SCSS.QueueEngine.QueueEngines;
using SCSS.TwilioService.Interfaces;
using SCSS.TwilioService.Models;
using SCSS.Utilities.Constants;
using SCSS.WebApi.SystemConstants;
using System;
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

        //[AllowAnonymous]
        //[HttpGet]
        //[Route(AdminApiUrlDefinition.AccountApiUrl.Search + "/demo-sms")]
        //public async Task<string> TestDemo([FromQuery] string phone, [FromQuery] string name)
        //{
        //    string Contact = "Moi thac mac xin lien he email: vechaixanh.hotro@gmail.com hoac so dien thoai: 0939872902";
        //    var sms = "Ung dung VeChaiXANH Thu Gom xin thong bao: \n" +
        //             $"Chao mung {name}. Tai khoan cua ban da duoc kich hoat thanh cong. Hay su dung tai khoan ban da dang ky de dang nhap vao ung dung. " + Contact;

        //    var model = new SMSMessageQueueModel()
        //    {
        //        Phone = phone,
        //        Content = name,
        //    };

        //    await _SQSPublisherService.SMSMessageQueuePublisher.SendMessageAsync(model);

        //    return "DEMO";
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
