using Apizr.Sample.Forms;
using Foundation;

[assembly: Shiny.ShinyApplication(
    ShinyStartupTypeName = "Apizr.Sample.Forms.Startup",
    XamarinFormsAppTypeName = "Apizr.Sample.Forms.App"
)]
namespace Apizr.Sample.Forms.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
    }
}
