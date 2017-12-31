namespace mvvmframework
{
    public interface IUserSettings
    {
        void SaveSetting<T>(string name, T value, SettingType setting);

        T LoadSetting<T>(string name, SettingType setting);
    }
}
