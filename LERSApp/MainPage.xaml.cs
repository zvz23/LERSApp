using LERSApp.Platforms.Android;
using LERSApp.Platforms.Android.Resources;
using static Android.Renderscripts.ScriptGroup;

namespace LERSApp;


public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
    }
    private string IsValidServerAddress(string serverAddress)
    {
        if (Uri.TryCreate(serverAddress, UriKind.Absolute, out Uri resultUri)
            && (resultUri.Scheme == Uri.UriSchemeHttp || resultUri.Scheme == Uri.UriSchemeHttps))
        {
            return $"{resultUri.Scheme}://{resultUri.Host}:{resultUri.Port}";
        }
        return string.Empty;
    }

    private async void SetServerAddressButtonClicked(object sender, EventArgs e)
    {
        if (SetServerAddressButton.Text == "Change")
        {
            Globals.ServerAddress = String.Empty;
            ServerAddressEntry.Text = String.Empty;
            SetServerAddressButton.Text = "Start Listening";
            return;
        }
        string serverAddress = IsValidServerAddress(ServerAddressEntry.Text);
        if (serverAddress == string.Empty)
        {
            await DisplayAlert("Invalid Server Address", "Please enter a valid server address", "OK");
            return;
        }
        Globals.ServerAddress = serverAddress;
        SetServerAddressButton.Text = "Change";
        await DisplayAlert("Server Address Set", "The server address is set.", "OK");
    }

}

