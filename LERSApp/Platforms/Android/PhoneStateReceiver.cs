using Android.Content;
using Android.Telephony;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android;
using Android.Util;
using Android.Telecom;
using Android.App;
using Android.Database;
using Android.Provider;
using Newtonsoft;

namespace LERSApp.Platforms.Android.Backup
{
    public class PhoneStateReceiver : BroadcastReceiver
    {

        public override async void OnReceive(Context context, Intent intent)
        {
            if (intent.Action == TelephonyManager.ActionPhoneStateChanged)
            {
                string state = intent.GetStringExtra(TelephonyManager.ExtraState);
                bool post = false;
                if (state == TelephonyManager.ExtraStateOffhook)
                {
                    MainActivity.LastIncomingCall.Status = "ONGOING";
                    post = true;
                }
                if (state == TelephonyManager.ExtraStateIdle)
                {
                    CallLogEntry callLogEntry = CallLogEntry.GetCallLogById(MainActivity.LastIncomingCall.Date);
                    MainActivity.LastIncomingCall.Status = callLogEntry.Status;
                    MainActivity.LastIncomingCall.Duration = callLogEntry.Duration;
                    post = true;
                }
                if (post)
                {
                    await CallLogEntry.PostCallLogAsync(MainActivity.LastIncomingCall);
                }

            }
        }

       
    }

}
