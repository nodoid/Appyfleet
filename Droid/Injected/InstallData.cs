using System;
using mvvmframework.Interfaces;

namespace NewAppyFleet.Droid.Injected
{
    public class InstallData : IInstallData
    {
        public string Installed
        {
            get
            {
                var packageManager = MainActivity.Active.PackageManager;
                long installTime = 0;
                var installDate = string.Empty;

                try
                {
                    var packageInfo = packageManager.GetPackageInfo(MainActivity.Active.PackageName, 0);
                    installTime = packageInfo.FirstInstallTime;
                    var dateStamp = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
                    var timeStamp = dateStamp.AddMilliseconds(installTime).ToLocalTime();
                    installDate = timeStamp.ToString("dd.MM.yy - hh.mm");
                }
                catch
                {
                    installDate = "-";
                }

                return installDate;
            }
        }

        public string VersionNumber => MainActivity.Active.PackageManager.GetPackageInfo(MainActivity.Active.PackageName, 0).VersionName;
    }
}
