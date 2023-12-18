using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Android.Content;
using Android.Database;
using Android.Provider;
using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Diagnostics.Tracing;
using LERSApp.Platforms.Android.Resources;

namespace LERSApp.Platforms.Android
{
    public class CallLogEntry
    {
        public long Date { get; set; }
        public int? Duration { get; set; }
        public string Status { get; set; }
        public string Number { get; set; }
        public string Coordinates  { get; set; }


        private static string GetCallTypeAsString(int callType)
        {
            switch (callType)
            {
                case (int)CallType.Incoming:
                    return "INCOMING";
                case (int)CallType.Outgoing:
                    return "OUTGOING";
                case (int)CallType.Missed:
                    return "MISSED";
                case (int)CallType.Rejected:
                    return "REJECTED";
                default:
                    return "UNKNOWN"; // Handle other call types if necessary
            }
        }

        public static CallLogEntry GetCallLogById(long id)
        {
            CallLogEntry callLogEntry = null;

            string[] projection = {
                CallLog.Calls.Date,
                CallLog.Calls.Duration,
                CallLog.Calls.Type,
                CallLog.Calls.Number,
            };
            string selection = $"{CallLog.Calls.Date} = ?";
            string[] selectionArgs = { id.ToString() };

            ContentResolver contentResolver = MainApplication.Context.ContentResolver;
            ICursor cursor = contentResolver.Query(CallLog.Calls.ContentUri, projection, selection, selectionArgs, null);
            if (cursor != null)
            {
                while (cursor.MoveToNext())
                {
                    callLogEntry = new CallLogEntry
                    {
                        Date = cursor.GetLong(cursor.GetColumnIndex(CallLog.Calls.Date)),
                        Duration = cursor.GetInt(cursor.GetColumnIndex(CallLog.Calls.Duration)),
                        Status = GetCallTypeAsString(cursor.GetInt(cursor.GetColumnIndex(CallLog.Calls.Type))),
                        Number = cursor.GetString(cursor.GetColumnIndex(CallLog.Calls.Number))
                    };
                }
                cursor.Close();
            }
            return callLogEntry;
        }
        public static List<CallLogEntry> GetCallLogs(Context context)
        {
            List<CallLogEntry> callLogs = new List<CallLogEntry>();
            string[] projection = {
                CallLog.Calls.Date,
                CallLog.Calls.Duration,
                CallLog.Calls.Type,
                CallLog.Calls.Number,
            };

            ContentResolver contentResolver = context.ContentResolver;
            ICursor cursor = contentResolver.Query(CallLog.Calls.ContentUri, projection, null, null, null);
            if (cursor != null)
            {
                while (cursor.MoveToNext())
                {
                    CallLogEntry callLogEntry = new CallLogEntry
                    {
                        Date = cursor.GetLong(cursor.GetColumnIndex(CallLog.Calls.Date)),
                        Duration = cursor.GetInt(cursor.GetColumnIndex(CallLog.Calls.Duration)),
                        Status = GetCallTypeAsString(cursor.GetInt(cursor.GetColumnIndex(CallLog.Calls.Type))),
                        Number = cursor.GetString(cursor.GetColumnIndex(CallLog.Calls.Number))
                    };

                    callLogs.Add(callLogEntry);
                }
                cursor.Close();
            }
            return callLogs;
        }
        public static async Task<HttpResponseMessage> PostCallLogsAsync(List<CallLogEntry> callLogs)
        {
            if (callLogs.Count == 0)
            {
                return null;
            } 
            string callLogsJson = JsonConvert.SerializeObject(callLogs);
            using(var client = new HttpClient())
            {
                var jsonContent = new StringContent(callLogsJson, Encoding.UTF8, "application/json");
                Uri saveUri = new Uri(new Uri(Globals.ServerAddress), "save/");
                var response = await client.PostAsync(saveUri.ToString(), jsonContent);
                return response;
            };

        }

        public static async Task<HttpResponseMessage> PostCallLogAsync(CallLogEntry callLog)
        {
            if (callLog == null)
            {
                return null;
            }
            string callLogJson = JsonConvert.SerializeObject(callLog);
            using (var client = new HttpClient())
            {
                var jsonContent = new StringContent(callLogJson, Encoding.UTF8, "application/json");
                Uri saveUri = new Uri(new Uri(Globals.ServerAddress), "save/");
                var response = await client.PostAsync(saveUri.ToString(), jsonContent);
                return response;
            };

        }
        public static string HashCallCreationTime(string time)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(time));
                string id = BitConverter.ToString(hash).Replace("-", "").ToLower();
                return id;
            }
        }
    }
}
