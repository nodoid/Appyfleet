using System;
using mvvmframework.Interfaces;
using Windows.ApplicationModel;

namespace AppyFleet.UWP.Injected
{
    public class InstallData : IInstallData
    {
        public string VersionNumber
        {
            get
            {
                var pv = Package.Current.Id.Version;
                return $"{pv.Major}.{pv.Minor}.{pv.Revision}";
            }
        }

        public string Installed => Package.Current.InstallDate.ToString("dd-mm-yyyy");
    }
}
