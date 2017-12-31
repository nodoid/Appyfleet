namespace NewAppyFleet
{
    public interface ILocalize
    {
        string GetCurrent();

        void SetLocale(System.Globalization.CultureInfo ci = null);
    }
}

