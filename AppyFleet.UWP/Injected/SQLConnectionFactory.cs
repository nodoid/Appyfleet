using AppyFleet.UWP.Injected;
using GalaSoft.MvvmLight.Ioc;
using mvvmframework;
using SQLite.Net;
using SQLite.Net.Interop;
using Windows.Storage;
using Xamarin.Forms;

[assembly: Dependency(typeof(SQLConnectionFactory))]
namespace AppyFleet.UWP.Injected
{
    public class SQLConnectionFactory : ISqLiteConnectionFactory
    {
        readonly string Filename = "appyfleet.db";

        public SQLiteConnection GetConnection()
        {
            var path = ApplicationData.Current.LocalFolder.Path;
            var connect = System.IO.Path.Combine(path, Filename);

            SimpleIoc.Default.Register<ISqLiteConnectionFactory, SQLConnectionFactory>();

            return new SQLiteConnection(SQLitePlatform, connect);
        }

        public ISQLitePlatform SQLitePlatform
        {
            get
            {
                return new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT();
            }
        }
    }
}

