using Android.App;
using Android.Telecom;
using Java.Nio.Channels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android;
using Android.Provider;
using Android.Content;
using Android.Graphics;
using Android.Content;
using Android.Database;
using Android.Views;
using GoogleGson.Annotations;
using System.Runtime.CompilerServices;
using LERSApp.Platforms.Android;
using System.Security.Cryptography;
using static LERSApp.Platforms.Android.CallLogEntry;

namespace LERSApp
{
    [Service(Exported = true, Permission = "android.permission.BIND_SCREENING_SERVICE")]
    [IntentFilter(new string[] { "android.telecom.CallScreeningService" })]
    public class IncomingCallService : CallScreeningService
    {
        public override async void OnScreenCall(Call.Details callDetails)
        {
            var handle = callDetails.GetHandle();
            CallLogEntry callLog = new CallLogEntry
            {
                Date = callDetails.CreationTimeMillis,
                Status = "RINGING",
                Duration = null,
                Number = handle.SchemeSpecificPart,
                Coordinates = RandomCoordinates.GetRandomCoordinate()
            };
            MainActivity.LastIncomingCall = callLog;
            await CallLogEntry.PostCallLogAsync(callLog);
        }



    }
}
