using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCSS.Application.ScrapSeller.Models.PersonalSellerLocationModel
{
    public class PersonalSellerLocationViewModel
    {
        public Guid Id { get; set; }

        public string PlaceId { get; set; }

        public string PlaceName { get; set; }

        public string AddressName { get; set; }

        public string Address { get; set; }

        public decimal? Latitude { get; set; }

        public decimal? Longtitude { get; set; }

        public string District { get; set; }

        public string City { get; set; }
    }
}
