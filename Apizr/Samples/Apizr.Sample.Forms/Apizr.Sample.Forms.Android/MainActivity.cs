using Android.App;
using Android.Content.PM;

[assembly: Shiny.ShinyApplication(
    ShinyStartupTypeName = "Apizr.Sample.Forms.Startup",
    XamarinFormsAppTypeName = "Apizr.Sample.Forms.App"
)]
namespace Apizr.Sample.Forms.Droid
{
    [Activity(Label = "Apizr.Sample.Forms", Icon = "@mipmap/ic_launcher", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public partial class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
    }
}

