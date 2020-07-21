using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Acr.UserDialogs;
using Apizr.Mediation.Cruding;
using Apizr.Optional.Cruding;
using Apizr.Requesting;
using Apizr.Sample.Api;
using Apizr.Sample.Api.Models;
using MediatR;
using Prism.Commands;
using Prism.Navigation;
using ReactiveUI.Fody.Helpers;
using Shiny;
using Color = System.Drawing.Color;

namespace Apizr.Sample.Mobile.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        private readonly IApizrManager<IReqResService> _reqResManager;
        private readonly IApizrManager<IHttpBinService> _httpBinManager;
        private readonly IApizrManager<ICrudApi<User, int, PagedResult<User>, IDictionary<string, object>>> _userCrudManager;
        private readonly IApizrManager<ICrudApi<UserDetails, int, IEnumerable<UserDetails>, IDictionary<string, object>>> _userDetailsCrudManager;
        private readonly IMediator _mediator;

        public MainPageViewModel(INavigationService navigationService, 
                IApizrManager<IHttpBinService> httpBinManager,
                IApizrManager<IReqResService> reqResManager,
            IApizrManager<ICrudApi<User, int, PagedResult<User>, IDictionary<string, object>>> userCrudManager,
            IApizrManager<ICrudApi<UserDetails, int, IEnumerable<UserDetails>, IDictionary<string, object>>> userDetailsCrudManager,
            IMediator mediator)
            : base(navigationService)
        {
            _httpBinManager = httpBinManager;
            _reqResManager = reqResManager;
            _userCrudManager = userCrudManager;
            _userDetailsCrudManager = userDetailsCrudManager;
            _mediator = mediator;
            GetUsersCommand = ExecutionAwareCommand.FromTask(GetUsersAsync);
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

        /// <summary>
        /// Managing crud entities comes with 3 web api call flavors
        /// Choosing one of it depends of your registration settings
        /// and how you like to play with api calls
        /// </summary>
        /// <returns></returns>
        private async Task GetUsersAsync()
        {
            if (IsRefreshing)
                return;

            IsRefreshing = true;

            //IList<User>? users = null;
            //try
            //{
            //    // This is a manually defined web api call into IReqResService (classic actually)
            //    //var userList = await _reqResManager.ExecuteAsync((ct, api) => api.GetUsersAsync(ct), CancellationToken.None);
            //    //users = userList?.Data;

            //    // This is the Crud way, with or without Crud attribute auto registration, but without mediation
            //    var pagedUsers = await _userCrudManager.ExecuteAsync((ct, api) => api.ReadAll(ct), CancellationToken.None);
            //    users = pagedUsers?.Data?.ToList();
            //}
            //catch (ApizrException<UserList> e)
            //{
            //    var message = e.InnerException is IOException ? "No network" : (e.Message ?? "Error");
            //    UserDialogs.Instance.Toast(new ToastConfig(message) { BackgroundColor = Color.Red, MessageTextColor = Color.White });

            //    users = e.CachedResult?.Data;
            //}
            //catch (ApizrException<PagedResult<User>> e)
            //{
            //    var message = e.InnerException is IOException ? "No network" : (e.Message ?? "Error");
            //    UserDialogs.Instance.Toast(new ToastConfig(message) { BackgroundColor = Color.Red, MessageTextColor = Color.White });

            //    users = e.CachedResult?.Data?.ToList();
            //}
            //finally
            //{
            //    if (users != null && users.Any())
            //        Users = new ObservableCollection<User>(users);

            //    IsRefreshing = false;
            //}

            try
            {
                // The same as before but with auto mediation handling and with optional result
                var result = await _mediator.Send(new ReadAllOptionalQuery<PagedResult<User>>(), CancellationToken.None);
                result.Match(pagedUsers =>
                {
                    if (pagedUsers.Data != null && pagedUsers.Data.Any())
                        Users = new ObservableCollection<User>(pagedUsers.Data);
                }, e =>
                {
                    var message = e.InnerException is IOException ? "No network" : (e.Message ?? "Error");
                    UserDialogs.Instance.Toast(new ToastConfig(message) { BackgroundColor = Color.Red, MessageTextColor = Color.White });

                    if (e.CachedResult?.Data != null && e.CachedResult.Data.Any())
                        Users = new ObservableCollection<User>(e.CachedResult.Data);
                });
            }
            catch (Exception e)
            {
            }

            IsRefreshing = false;
        }

        /// <summary>
        /// Managing crud entities comes with 3 web api call flavors
        /// Choosing one of it depends of your registration settings
        /// and how you like to play with api calls.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        private async Task GetUserDetails(User user)
        {
            User? fetchedUser;
            try
            {
                // This is a manually defined web api call into IReqResService (classic actually)
                //var userDetails = await _reqResManager.ExecuteAsync((ct, api) => api.GetUserAsync(user.Id, ct), CancellationToken.None);
                //fetchedUser = userDetails?.User;

                // This is the Crud way, with or without Crud attribute auto registration, but without mediation
                //var userDetails = await _userDetailsCrudManager.ExecuteAsync((ct, api) => api.Read(user.Id, ct), CancellationToken.None);
                //fetchedUser = userDetails?.User;

                // The same as before but with auto mediation handling and without optional result this time
                var userDetails = await _mediator.Send(new ReadQuery<UserDetails>(user.Id), CancellationToken.None);
                fetchedUser = userDetails?.User;
            }
            catch (ApizrException<UserDetails> e)
            {
                var message = e.InnerException is IOException ? "No network" : (e.Message ?? "Error");
                UserDialogs.Instance.Toast(new ToastConfig(message)
                    {BackgroundColor = Color.Red, MessageTextColor = Color.White});

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
