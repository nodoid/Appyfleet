namespace mvvmframework
{
    public class DriverRegistrationModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MobileNumber { get; set; }
        public string EmailAddress { get; set; }
        public string YOB { get; set; }
        public string DOBText { get; set; }
        public string CompanyName { get; set; }
        public string Position { get; set; }
        public string AccessCode { get; set; }
        public string Password { get; set; }
        public bool MarketingAgreed { get; set; }

        public string LanguageCode { get; set; }

        public RegisterRequestJSon TojSon()
        {
            RegisterRequestJSon ret = new RegisterRequestJSon {
                firstName = FirstName,
                surname = LastName,
                dateOfBirth = YOB,
                mobileNumber = MobileNumber,
                workEmail = EmailAddress,
                password = Password,
                companyName = CompanyName,
                position = Position,
                accessCode = AccessCode,
                marketingAgreed = MarketingAgreed,
                languageCode = LanguageCode
            };


            return ret;
        }
    }
}
