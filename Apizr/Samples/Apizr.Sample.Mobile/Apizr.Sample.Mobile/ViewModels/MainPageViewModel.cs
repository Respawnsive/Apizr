using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Acr.UserDialogs;
using Apizr.Requesting;
using Apizr.Sample.Api;
using Apizr.Sample.Api.Models;
using Prism.Commands;
using Prism.Navigation;
using ReactiveUI.Fody.Helpers;
using Color = System.Drawing.Color;

namespace Apizr.Sample.Mobile.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        private readonly IApizrManager<IReqResService> _reqResManager;
        private readonly IApizrManager<ICrudApi<UserDetails, int>> _userDetailsCrudManager;
        private readonly IApizrManager<IHttpBinService> _httpBinManager;

        public MainPageViewModel(INavigationService navigationService, IApizrManager<IReqResService> reqResManager, 
            IApizrManager<ICrudApi<UserDetails, int>> userDetailsCrudManager,
            IApizrManager<IHttpBinService> httpBinManager)
            : base(navigationService)
        {
            _reqResManager = reqResManager;
            _userDetailsCrudManager = userDetailsCrudManager;
            _httpBinManager = httpBinManager;
            GetUsersCommand = ExecutionAwareCommand.FromTask(GetUsersAsync).OnIsExecutingChanged(isExecuting => IsRefreshing = isExecuting);
            //GetUserDetailsCommand = ExecutionAwareCommand.FromTask<User>(GetUserDetails);
            GetUserDetailsCommand = new DelegateCommand<User>(async user => await GetUserDetails(user));
            AuthCommand = ExecutionAwareCommand.FromTask(AuthAsync);

            Title = "Main Page";
        }

        #region Properties

        [Reactive] public ObservableCollection<User> Users { get; set; } = new ObservableCollection<User>();

        [Reactive] public bool IsRefreshing { get; set; }

        public ICommand GetUsersCommand { get; }

        public ICommand GetUserDetailsCommand { get; }

        public ICommand AuthCommand { get; }

        #endregion

        #region Methods
        private async Task GetUsersAsync()
        {
            if (IsRefreshing)
                return;

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

        private async Task GetUserDetails(User user)
        {
            User? fetchedUser;
            try
            {
                var userDetails = await _userDetailsCrudManager.ExecuteAsync((ct, api) => api.Read(user.Id, ct), CancellationToken.None);
                fetchedUser = userDetails?.User;
            }
            catch (ApizrException<UserDetails> e)
            {
                var message = e.InnerException is IOException ? "No network" : (e.Message ?? "Error");
                UserDialogs.Instance.Toast(new ToastConfig(message) { BackgroundColor = Color.Red, MessageTextColor = Color.White });

                fetchedUser = e.CachedResult?.User;
            }

            if (fetchedUser != null)
                UserDialogs.Instance.Alert(
                    $"{fetchedUser.FirstName} {fetchedUser.LastName}\n {fetchedUser.Email}", fetchedUser.FirstName);
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

            GetUsersCommand.Execute(null);
        }

        #endregion
    }
}
