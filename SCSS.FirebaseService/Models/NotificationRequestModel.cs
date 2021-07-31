using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCSS.FirebaseService.Models
{
    public class NotificationRequestModel
    {
        public string Title { get; set; }

        public string Body { get; set; }

        public IReadOnlyDictionary<string, string> Data { get; set; }

        public string DeviceId { get; set; }

    }
}
