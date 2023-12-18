using Android.App;
using Android.App.Roles;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Telecom;
using Android.Telephony;
using AndroidX.Core.App;
using LERSApp.Platforms.Android;
using LERSApp.Platforms.Android.Backup;

namespace LERSApp;

[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
public class MainActivity : MauiAppCompatActivity
{
    public static MainActivity ActivityCurrent { get; set; }
    public static CallLogEntry LastIncomingCall;
    private const int ROLE_CALL = 1;
    private const int READ_CALL_LOG = 2;
    private const int READ_PHONE_STATE = 3;
    private PhoneStateReceiver phoneStateReceiver { get; set; }


    public MainActivity()
    {
        ActivityCurrent = this;
    }
    protected override void OnCreate(Bundle savedInstanceState)
    {
        base.OnCreate(savedInstanceState);
        RequestCallScreeningRole();
        CheckAndRequestRequiredPermissions();
        var phoneStateReceiver = new PhoneStateReceiver();
        IntentFilter intent = new IntentFilter(TelephonyManager.ActionPhoneStateChanged);
        RegisterReceiver(phoneStateReceiver, intent);
    }

    private void RequestCallScreeningRole()
    {
        RoleManager roleManager = (RoleManager)GetSystemService(RoleService);
        Intent intent = roleManager.CreateRequestRoleIntent(RoleManager.RoleCallScreening);
        StartActivityForResult(intent, ROLE_CALL);
    }

    private void CheckAndRequestRequiredPermissions()
    {
        Permission readCallLogPermssionResult = CheckSelfPermission(Android.Manifest.Permission.ReadCallLog);
        if (readCallLogPermssionResult == Permission.Denied)
        {
            RequestPermissions(new string[] { Android.Manifest.Permission.ReadCallLog }, READ_CALL_LOG);
        }
        Permission readPhoneStatePermissionResult = CheckSelfPermission(Android.Manifest.Permission.ReadPhoneState);
        if (readPhoneStatePermissionResult == Permission.Denied)
        {
            RequestPermissions(new string[] { Android.Manifest.Permission.ReadPhoneState }, READ_PHONE_STATE);
        }
    }

    public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
    {
        Permission grantResult = grantResults[0];
        if (requestCode == READ_CALL_LOG && grantResult == Permission.Denied)
        {
            CloseApplicationWithAlert("Permission Required", "You must grant access to the call logs to use this app.", "OK");
        }
    }

    protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
    {
        base.OnActivityResult(requestCode, resultCode, data);
        if (requestCode == ROLE_CALL)
        {
            if (resultCode != Result.Ok)
            {
                CloseApplicationWithAlert("Permission Required", "You must grant the Call Screening Role permission to use this app.", "OK");
            }
        }

    }

    private async void CloseApplicationWithAlert(string title, string message, string cancel)
    {
        if (MainThread.IsMainThread)
        {
            await App.Current.MainPage.DisplayAlert(title, message, cancel);
            App.Current.Quit();
        }
        else
        {
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                await App.Current.MainPage.DisplayAlert(title, message, cancel);
                App.Current.Quit();
            });
        }


    }



}
