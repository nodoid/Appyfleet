using System;
using Android.Content.PM;
using Xamarin.Forms;

namespace NewAppyFleet.Droid
{
    namespace RuntimePermissions
    {
        public abstract class PermissionUtil
        {
            public static bool VerifyPermissions(Permission[] grantResults)
            {
                if (grantResults.Length < 1) return false;

                foreach (var result in grantResults)
                {
                    if (result != Permission.Granted)
                    {
                        return false;
                    }
                }
                return true;
            }
        }
    }
}

