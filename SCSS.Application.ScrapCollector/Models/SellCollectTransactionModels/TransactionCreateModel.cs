using System;
using System.Collections.Generic;

namespace SCSS.Application.ScrapCollector.Models.SellCollectTransactionModels
{
    public class SellCollectTransactionCreateModel
    {
        public Guid CollectingRequestId { get; set; }

        public long TransactionServiceFee { get; set; }

        public long Total { get; set; }

        public List<ScrapCategoryTransactionDetailCreateModel> ScrapCategoryItems { get; set; }
    }

    public class ScrapCategoryTransactionDetailCreateModel
    {
        public Guid? CollectorCategoryDetailId { get; set; }

        public float? Quantity { get; set; }

        public long? Total { get; set; }

        public long? Price { get; set; }
    }

    public class PushAndSaveNotificationModel
    {
        public Guid? SellerId { get; set; }

        public string DeviceId { get; set; }

        public string Title { get; set; }

        public string Body { get; set; }

        public Dictionary<string, string> DictionaryData { get; set; }
    }
}
