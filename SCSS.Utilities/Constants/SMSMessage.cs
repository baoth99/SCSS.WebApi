using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCSS.Utilities.Constants
{
    public class SMSMessage
    {
        public const string HeaderMessage = "Ung dung VeChaiXANH xin thong bao: \n";
        public const string Contact = "Moi thac mac xin lien he email: vechaixanh.hotro@gmail.com hoac so dien thoai: 0939872902";

        public static string ApprovedSMS()
        {
            var sms =  HeaderMessage +
                      "Tai khoan cua ban da duoc kich hoat thanh cong. Hay su dung tai khoan ban da dang ky de dang nhap vao ung dung. " + Contact;
            return sms;
        }

        public static string BlockSMS()
        {
            var sms = HeaderMessage +
                      "Tai khoan cua ban da bi khoa tam thoi. " + Contact;
            return sms;
        }

        public static string UnBlockSMS()
        {
            var sms = HeaderMessage +
                      "Tai khoan cua ban duoc mo khoa. " + Contact;
            return sms;
        }

        public static string RejectedSMS()
        {
            var sms = HeaderMessage +
                      "Tai khoan cua ban da bi tu choi kich hoat. " + Contact;
            return sms;
        }

    }
}
