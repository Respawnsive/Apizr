using System;
using Apizr.Sample.Mobile.Views;
using DryIoc;
using Newtonsoft.Json;
using Prism.Ioc;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace Apizr.Sample.Mobile
{
    public partial class App
    {
        public App() { }

        protected override async void OnInitialized()
        {
            InitializeComponent();

            var rules = JsonConvert.SerializeObject(((Container)((IContainerExtension<IContainer>)Container).Instance).Rules, Formatting.Indented, new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            await NavigationService.NavigateAsync("NavigationPage/MainPage");
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<MainPage>();
        }
    }
}
