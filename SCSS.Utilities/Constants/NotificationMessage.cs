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

        public static string CancelCollectingRequestTitleSystem(string code) => $"Yêu cầu thu gom {code} đã bị hủy bởi hệ thống";

        public static string CancelCollectingRequestBodySystem(string code) => $"Yêu cầu thu gom {code} đã bị hủy bởi hệ thống.  Chưa có người thu gom xác nhận yêu cầu của bạn trong thời gian đặt hẹn. !";
    }
}
