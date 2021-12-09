namespace SCSS.Utilities.Constants
{
    public class SMSMessage
    {
        public const string HeaderMessage = "Ung dung VeChaiXANH xin thong bao: \n";
        public const string Contact = "Moi thac mac xin lien he email: vechaixanh.hotro@gmail.com hoac so dien thoai: 0939872902";

        public static string OtpSMS(string otp)
        {
            var sms = HeaderMessage +
                      $"Ma OTP cua ban la {otp}. OTP co hieu luc trong 3 phut. Tran trong !";
            return sms;
        }

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

        public static string CreateAccountMessage(string phone, string password)
        {
            return HeaderMessage + "Tai khoan cua ban da duoc dang ky thanh cong. \n" + 
                   $"Tai Khoan {phone} \n" +
                   $"Mat Khau: {password} \n" +
                   "Hay dang nhap vao ung dung va doi mat khau ngay, vui long khong tiet lo mat khau cho nguoi khac. \n" +
                   Contact;
        }

    }
}
