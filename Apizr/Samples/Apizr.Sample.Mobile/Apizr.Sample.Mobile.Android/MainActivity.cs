using Android.App;
using Android.Content.PM;

[assembly: Shiny.ShinyApplication(
    ShinyStartupTypeName = "global::Apizr.Sample.Mobile.Startup",
    XamarinFormsAppTypeName = "global::Apizr.Sample.Mobile.App"
)]
namespace Apizr.Sample.Mobile.Droid
{
    [Activity(Label = "Apizr.Sample.Mobile", Icon = "@mipmap/ic_launcher", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public partial class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
    }
}

