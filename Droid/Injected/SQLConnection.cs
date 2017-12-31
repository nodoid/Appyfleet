using System;
using System.IO;
using SQLite.Net;
using SQLite.Net.Interop;
using SQLite.Net.Platform.XamarinAndroid;
using Xamarin.Forms;
using GalaSoft.MvvmLight.Ioc;
using mvvmframework;
using NewAppyFleet.Droid;

[assembly: Dependency(typeof(SQLConnection))]
namespace NewAppyFleet.Droid
{
    public class SQLConnection : ISqLiteConnectionFactory
    {
        readonly string Filename = "appyfleet.db";

        public SQLiteConnection GetConnection()
        {
            var path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            path = Path.Combine(path, Filename);

            SimpleIoc.Default.Register<ISqLiteConnectionFactory, SQLConnection>();

            return new SQLiteConnection(SQLitePlatform, path);
        }

        public ISQLitePlatform SQLitePlatform
        {
            get { return new SQLitePlatformAndroid(); }
        }
    }
}
