using MetroLog.Maui;

namespace Apizr.Sample.MAUI
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            LogController.InitializeNavigation(
                page => MainPage!.Navigation.PushModalAsync(page),
                () => MainPage!.Navigation.PopModalAsync());
        }
    }
}