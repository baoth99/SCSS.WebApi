using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCSS.Application.ScrapDealer.Models.DealerInformationModels
{
    public class DealerInformationDetailViewModel
    {
        public Guid Id { get; set; }

        public string DealerName { get; set; }

        public string DealerImageUrl { get; set; }

        public decimal? DealerLatitude { get; set; }

        public decimal? DealerLongtitude { get; set; }

        public string DealerAddress { get; set; }

        public string DealerPhone { get; set; }

        public string OpenTime { get; set; }

        public string CloseTime { get; set; }

    }
}
