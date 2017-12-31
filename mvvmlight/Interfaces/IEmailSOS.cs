namespace mvvmframework.Interfaces
{
    public interface IEmailSms
    {
        bool SendEmailFile(string to, string subject, string body);
        bool SendEmail(string to, string subject, string body);
        void SendSOSSMS(string message, string mobileNumber);
        string GenerateEmailHashingString(string key, string emailAddress);
    }
}
