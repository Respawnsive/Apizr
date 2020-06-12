using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Acr.UserDialogs;
using Apizr.Sample.Api;
using Apizr.Sample.Api.Models;
using Prism.Commands;
using Prism.Navigation;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Xamarin.Forms;
using Color = System.Drawing.Color;

namespace Apizr.Sample.Mobile.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        private readonly IApizrManager<IReqResService> _reqResManager;
        private readonly IApizrManager<IHttpBinService> _httpBinManager;

        public MainPageViewModel(INavigationService navigationService, IApizrManager<IReqResService> reqResManager,
            IApizrManager<IHttpBinService> httpBinManager)
            : base(navigationService)
        {
            _reqResManager = reqResManager;
            _httpBinManager = httpBinManager;
            AuthCommand = ExecutionAwareCommand.FromTask(AuthAsync);

            Title = "Main Page";
        }

        #region Properties

        [Reactive] public ObservableCollection<User>? Users { get; set; }

        public ICommand AuthCommand { get; }

        #endregion

        #region Methods
        private async Task GetUsersAsync()
        {
            IList<User>? users;
            try
            {
                var userList = await _reqResManager.ExecuteAsync((ct, api) => api.GetUsersAsync(ct), CancellationToken.None);
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

        private async Task AuthAsync()
        {
            string result;
            var succeed = false;
            try
            {
                var response = await _httpBinManager.ExecuteAsync(api => api.AuthBearerAsync());
                succeed = response.IsSuccessStatusCode;
                result = response.IsSuccessStatusCode ? "Authentication succeed :)" : "Authentication failed :(";
            }
            catch (ApizrException e)
            {
                result = e.InnerException is IOException ? "No network" : (e.Message ?? "Error");
            }

            if (!string.IsNullOrWhiteSpace(result))
                UserDialogs.Instance.Toast(new ToastConfig(result)
                    {BackgroundColor = succeed ? Color.Green : Color.Red, MessageTextColor = Color.White});
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
