using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using Acr.UserDialogs;
using Apizr.Sample.Api;
using Apizr.Sample.Api.Models;
using Prism.Navigation;
using ReactiveUI.Fody.Helpers;
using Xamarin.Forms;
using Color = System.Drawing.Color;

namespace Apizr.Sample.Mobile.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        private readonly IApizrManager<IReqResService> _reqResManager;

        public MainPageViewModel(INavigationService navigationService, IApizrManager<IReqResService> reqResManager)
            : base(navigationService)
        {
            _reqResManager = reqResManager;
            Title = "Main Page";
        }

        #region Properties

        [Reactive] public ObservableCollection<User>? Users { get; set; }

        #endregion

        #region Methods
        private async Task GetUsersAsync()
        {
            IList<User>? users;
            try
            {
                var userList = await _reqResManager.ExecuteAsync(api => api.GetUsersAsync());
                users = userList.Data;
            }
            catch (ApizrException<UserList> e)
            {
                var message = e.InnerException is IOException ? "No network" : (e.Message ?? "Error");
                UserDialogs.Instance.Toast(new ToastConfig(message) { BackgroundColor = Color.Red, MessageTextColor = Color.White });

                users = e.CachedResult?.Data;
            }

            if(users != null)
                Users = new ObservableCollection<User>(users);
        }

        #endregion

        #region Lifecycle

        public override void OnAppearing()
        {
            base.OnAppearing();

            Device.InvokeOnMainThreadAsync(GetUsersAsync);
        }

        #endregion
    }
}
