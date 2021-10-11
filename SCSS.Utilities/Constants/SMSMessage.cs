using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCSS.Utilities.Constants
{
    public class SMSMessage
    {
        public const string Contact = "Mọi thắc mắc xin liên hệ email: vechaixanh.hotro@gmail.com hoặc số điện thoại: 0939872902";

        public static string ApprovedSMS()
        {
            var sms = "Ứng dụng VeChaiXANH Thu Gom xin thông báo: \n" +
                      "Tài khoản của bạn đã được kích hoạt thành công. Hãy sử dụng tài khoản bạn đã đăng ký để đăng nhập vào ứng dụng. " + Contact;
            return sms;
        }

        public static string BlockSMS()
        {
            var sms = "Ứng dụng VeChaiXANH Thu Gom xin thông báo: \n" +
                      "Tài khoản của bạn đã bị khóa tạm thời. " + Contact;
            return sms;
        }

        public static string UnBlockSMS()
        {
            var sms = "Ứng dụng VeChaiXANH Thu Gom xin thông báo: \n" +
                      "Tài khoản của bạn được mở khóa. " + Contact;
            return sms;
        }

        public static string RejectedSMS()
        {
            var sms = "Ứng dụng VeChaiXANH Thu Gom xin thông báo: \n" +
                      "Tài khoản của bạn đã bị từ chối kích hoạt. " + Contact;
            return sms;
        }

    }
}
