// Copied from https://github.com/PieEatingNinjas/MAUI_MVVM_Demo

using System.Diagnostics;
using Apizr.Sample.MAUI.Views;
using Apizr.Sample.MAUI.ViewModels;

namespace Apizr.Sample.MAUI.Services
{
    public class NavigationService : INavigationService
    {
        private readonly IServiceProvider _services;

        protected INavigation Navigation
        {
            get
            {
                var navigation = Application.Current?.MainPage?.Navigation;
                if (navigation is not null)
                    return navigation;
                else
                {
                    //This is not good!
                    if (Debugger.IsAttached)
                        Debugger.Break();
                    throw new Exception();
                }
            }
        }

        public NavigationService(IServiceProvider services)
            => _services = services;

        public Task NavigateToMainPage()
            => NavigateToPage<MainPage>();

        public Task NavigateBack()
        {
            if (Navigation.NavigationStack.Count > 1)
                return Navigation.PopAsync();

            throw new InvalidOperationException("No pages to navigate back to!");
        }

        private async Task NavigateToPage<T>(object parameter = null) where T : Page
        {
            var toPage = ResolvePage<T>();

            if (toPage is not null)
            {
                //Subscribe to the toPage's NavigatedTo event
                toPage.NavigatedTo += Page_NavigatedTo;

                //Get VM of the toPage
                var toViewModel = GetPageViewModelBase(toPage);

                //Call navigatingTo on VM, passing in the paramter
                if (toViewModel is not null)
                    await toViewModel.OnNavigatingTo(parameter);

                //Navigate to requested page
                await Navigation.PushAsync(toPage, true);

                //Subscribe to the toPage's NavigatedFrom event
                toPage.NavigatedFrom += Page_NavigatedFrom;
            }
            else
                throw new InvalidOperationException($"Unable to resolve type {typeof(T).FullName}");
        }

        private async void Page_NavigatedFrom(object sender, NavigatedFromEventArgs e)
        {
            //To determine forward navigation, we look at the 2nd to last item on the NavigationStack
            //If that entry equals the sender, it means we navigated forward from the sender to another page
            var isForwardNavigation = Navigation.NavigationStack.Count > 1
                                      && Navigation.NavigationStack[^2] == sender;

            if (sender is not Page thisPage) 
                return;

            if (!isForwardNavigation)
            {
                thisPage.NavigatedTo -= Page_NavigatedTo;
                thisPage.NavigatedFrom -= Page_NavigatedFrom;
            }

            await CallNavigatedFrom(thisPage, isForwardNavigation);
        }

        private static Task CallNavigatedFrom(Page p, bool isForward)
        {
            var fromViewModel = GetPageViewModelBase(p);

            return fromViewModel is not null ? fromViewModel.OnNavigatedFrom(isForward) : Task.CompletedTask;
        }

        private static async void Page_NavigatedTo(object sender, NavigatedToEventArgs e)
            => await CallNavigatedTo(sender as Page);

        private static Task CallNavigatedTo(Page p)
        {
            var fromViewModel = GetPageViewModelBase(p);

            return fromViewModel is not null ? fromViewModel.OnNavigatedTo() : Task.CompletedTask;
        }

        private static ViewModelBase GetPageViewModelBase(Page p)
            => p?.BindingContext as ViewModelBase;

        private T ResolvePage<T>() where T : Page
            => _services.GetService<T>();
    }
}
