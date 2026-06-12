namespace App.UI.Web.Models
{
    public class OtpModel
    {
        public string Otp1 { get; set; }
        public string Otp2 { get; set; }
        public string Otp3 { get; set; }
        public string Otp4 { get; set; }
        public string Otp5 { get; set; }
        public string Otp6 { get; set; }

        public string GetOtp()
        {
            return $"{Otp1}{Otp2}{Otp3}{Otp4}{Otp5}{Otp6}";
        }
    }
}
