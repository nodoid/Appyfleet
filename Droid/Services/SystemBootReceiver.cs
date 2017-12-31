using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AppyFleet.Droid.Services;
using AppyFleet.Controlers;
using AppyFleet.Droid.Dependence;

namespace AppyFleet.Droid.BroadcastReceivers
{
    [BroadcastReceiver]
    [IntentFilter(new[] { Android.Content.Intent.ActionBootCompleted })]
    class SystemBootReceiver : BroadcastReceiver
    {
        public static int count = 0;
        public override void OnReceive(Context context, Intent intent)
        {
            count++;
            try
            {
                if (intent.Action == Intent.ActionBootCompleted)
                {
                    Intent startIntent = new Intent(context, typeof(AppyFleetService));
                    context.StartService(startIntent);
                }
            }
            catch(Exception ex)
            {
                LogFileService.Instance.WriteLog("Catch all", ex.ToString());
            }
        }
    }
}