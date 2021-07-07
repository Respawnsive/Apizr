using Apizr.Sample.Mobile;
using Foundation;

[assembly: Shiny.ShinyApplication(
    ShinyStartupTypeName = nameof(Startup),
    XamarinFormsAppTypeName = nameof(App)
)]
namespace Apizr.Sample.Mobile.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
    }
}
