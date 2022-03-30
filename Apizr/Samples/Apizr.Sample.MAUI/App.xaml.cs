using Apizr.Sample.MAUI.Services;

namespace Apizr.Sample.MAUI
{
    public partial class App : Application
    {
        public App(INavigationService navigationService)
        {
            InitializeComponent();

            MainPage = new NavigationPage();
            navigationService.NavigateToMainPage();
        }
    }
}