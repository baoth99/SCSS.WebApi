using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCSS.Utilities.Constants
{
    public class NotificationMessage
    {
        public static string CollectingRequestReceviedTitle(string collectingRequestCode) => $"Yêu cầu thu gom {collectingRequestCode} của bạn đã được nhận";
        public static string CollectingRequestCancelTitle(string collectingRequestCode) => $"Yêu cầu thu gom {collectingRequestCode} của bạn đã bị người thu gom hủy bỏ";
        public static string CollectingRequestCancelBody(string cancelReason) => $"Vì lí do: {cancelReason}";
    }
}
