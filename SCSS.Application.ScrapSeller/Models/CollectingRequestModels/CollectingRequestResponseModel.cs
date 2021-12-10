using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCSS.Application.ScrapSeller.Models.CollectingRequestModels
{
    public class CollectingRequestResponseModel
    {
        public Guid Id { get; set; }

        public string CollectingRequestCode { get; set; }

        public int? RequestType { get; set; }

        public int? Status { get; set; }
    }
}
