using SCSS.AWSService.Models;
using SCSS.AWSService.RedisCacheHandler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCSS.AWSService.Interfaces
{
    public interface ICacheListService
    {
        #region Handlers

        ICacheListHandler<PendingCollectingRequestCacheModel> PendingCollectingRequestCache { get; }

        ICacheListHandler<ImageCacheModel> ImageSliderCache { get; }

        #endregion
    }
}
