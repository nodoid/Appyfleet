namespace mvvmframework
{
    public class RegisterResponseJSon
    {
        public RegisterModel RegisterV3Result { get; set; }
    }

    public class RegisterModel
    {
        public int DriverId { get; set; }
        public StatusModel Status { get; set; }
    }
}
