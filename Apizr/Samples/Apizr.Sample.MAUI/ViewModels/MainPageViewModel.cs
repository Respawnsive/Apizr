using System.Collections.ObjectModel;
using System.Windows.Input;
using Apizr.Extending.Configuring.Registry;
using Apizr.Optional.Cruding.Sending;
using Apizr.Requesting;
using Apizr.Sample.Models;
using Fusillade;
using MediatR;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Apizr.Sample.MAUI.ViewModels
{

    public class MainPageViewModel : ReactiveObject, IPageLifecycleAware
    {
        private readonly INavigationService _navigationService;
        private readonly IPageDialogService _dialogService;
        private readonly IApizrManager<IReqResService> _reqResManager;
        private readonly IApizrManager<IHttpBinService> _httpBinManager;
        private readonly IApizrManager<ICrudApi<User, int, PagedResult<User>, IDictionary<string, object>>> _userCrudManager;
        private readonly IApizrManager<ICrudApi<UserDetails, int, IEnumerable<UserDetails>, IDictionary<string, object>>> _userDetailsCrudManager;
        private readonly IMediator _mediator;
        private readonly IApizrCrudOptionalMediator<User, int, PagedResult<User>, IDictionary<string, object>> _userOptionalMediator;

        public MainPageViewModel(INavigationService navigationService, 
                IPageDialogService dialogService, 
                IApizrExtendedRegistry apizrRegistry)
        //IApizrManager<IReqResService> reqResManager),
        //IApizrManager<ICrudApi<User, int, PagedResult<User>, IDictionary<string, object>>> userCrudManager,
        //IApizrManager<IHttpBinService> httpBinManager,
        //IApizrManager<ICrudApi<UserDetails, int, IEnumerable<UserDetails>, IDictionary<string, object>>> userDetailsCrudManager,
        //IMediator mediator,
        //ICrudOptionalMediator<User, int, PagedResult<User>, IDictionary<string, object>> userOptionalMediator)
        {
            _navigationService = navigationService;
            _dialogService = dialogService;
            _reqResManager = apizrRegistry.GetManagerFor<IReqResService>(); //reqResManager;
            _userCrudManager = apizrRegistry.GetCrudManagerFor<User, int, PagedResult<User>, IDictionary<string, object>>(); //userCrudManager;
            _httpBinManager = apizrRegistry.GetManagerFor<IHttpBinService>(); //httpBinManager;
            //_userDetailsCrudManager = userDetailsCrudManager;
            //_mediator = mediator;
            //_userOptionalMediator = userOptionalMediator;
            GetUsersCommand = ReactiveCommand.CreateFromTask(GetUsersAsync);
            GetUserDetailsCommand = ReactiveCommand.CreateFromTask<User>(GetUserDetailsAsync);
            AuthCommand = ReactiveCommand.CreateFromTask(AuthAsync);
            Users = new ObservableCollection<User>();
        }

        #region Properties

        [Reactive] public ObservableCollection<User> Users { get; set; }

        [Reactive] public bool IsRefreshing { get; set; }

        public ICommand GetUsersCommand { get; }

        public ICommand GetUserDetailsCommand { get; }

        public ICommand AuthCommand { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Managing crud entities comes with many web api call flavors
        /// Choosing one of it depends on your registration settings
        /// and how you'd like to play with api calls
        /// </summary>
        /// <returns></returns>
        private async Task GetUsersAsync()
        {
            IsRefreshing = true;

            IList<User>? users = null;
            try
            {
                await SecureStorage.SetAsync("IdentityKey", "123456789");

                // This is a manually defined web api call into IReqResService (classic actually)
                var userList = await _reqResManager.ExecuteAsync(api => api.GetUsersAsync());
                users = userList?.Data;

                // This is the Crud way, with or without Crud attribute auto registration, but without mediation
                //var pagedUsers = await _userCrudManager.ExecuteAsync(api => api.ReadAll());
                //users = pagedUsers?.Data?.ToList();

                // The same as before but with auto mediation handling
                //var pagedUsers = await _mediator.Send(new ReadAllQuery<PagedResult<User>>());
                //users = pagedUsers?.Data?.ToList();
            }
            catch (ApizrException<UserList> e)
            {
                var message = e.InnerException is IOException ? "No network" : (e.Message ?? "Error");
                await _dialogService.DisplayAlertAsync("Error", message, "Ok");

                users = e.CachedResult?.Data;
            }
            catch (ApizrException<PagedResult<User>> e)
            {
                var message = e.InnerException is IOException ? "No network" : (e.Message ?? "Error");
                await _dialogService.DisplayAlertAsync("Error", message, "Ok");

                users = e.CachedResult?.Data?.ToList();
            }
            finally
            {
                if (users != null && users.Any())
                    Users = new ObservableCollection<User>(users);
                else
                    Users.Clear();

                IsRefreshing = false;
            }

            // The same as before but with optional result
            //var result = await _mediator.Send(new ReadAllOptionalQuery<PagedResult<User>>());
            //result.Match(pagedUsers =>
            //{
            //    if (pagedUsers.Data != null && pagedUsers.Data.Any())
            //        Users = new ObservableCollection<User>(pagedUsers.Data);
            //}, e =>
            //{
            //    var message = e.InnerException is IOException ? "No network" : (e.Message ?? "Error");
            //    await Dialogs.Snackbar(message);

            //    if (e.CachedResult?.Data != null && e.CachedResult.Data.Any())
            //        Users = new ObservableCollection<User>(e.CachedResult.Data);
            //});

            // The same as before but with fluent result handling (fetched or cached, doesn't matter) and global exception handling (AsyncErrorHandler)
            //await _userOptionalMediator.SendReadAllOptionalQuery().OnResultAsync(pagedUsers =>
            //{
            //    if (!pagedUsers.Data.IsEmpty())
            //        Users = new ObservableCollection<User>(pagedUsers.Data);

            //    IsRefreshing = false;
            //});

            //var pagedUsers = await _userOptionalMediator.SendReadAllOptionalQuery().CatchAsync(AsyncErrorHandler.HandleException);
            //if (!pagedUsers.Data.IsEmpty())
            //    Users = new ObservableCollection<User>(pagedUsers.Data);

            //IsRefreshing = false;
        }

        /// <summary>
        /// Managing crud entities comes with 3 web api call flavors
        /// Choosing one of it depends of your registration settings
        /// and how you like to play with api calls.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        private async Task GetUserDetailsAsync(User user)
        {

            User? fetchedUser = null;

            try
            {
                // This is a manually defined web api call into IReqResService (classic actually)
                var userDetails = await _reqResManager.ExecuteAsync((opt, api) => api.GetUserAsync(user.Id, opt), options => options.WithPriority(Priority.UserInitiated));
                fetchedUser = userDetails?.User;

                // This is the Crud way, with or without Crud attribute auto registration, but without mediation
                //var userDetails = await _userDetailsCrudManager.ExecuteAsync(api => api.Read(user.Id));
                //fetchedUser = userDetails?.User;

                // The same as before but with auto mediation handling and without optional result this time
                //var userDetails = await _mediator.Send(new ReadQuery<UserDetails>(user.Id));
                //fetchedUser = userDetails?.User;
            }
            catch (ApizrException<UserDetails> e)
            {
                var message = e.InnerException is IOException ? "No network" : (e.Message ?? "Error");
                await _dialogService.DisplayAlertAsync("Error", message, "Ok");

                fetchedUser = e.CachedResult?.User;
            }

            // The same as before but with auto mediation handling and with optional result
            //var result = await _mediator.Send(new ReadOptionalQuery<UserDetails>(user.Id), CancellationToken.None);
            //result.Match(userDetails =>
            //{
            //    fetchedUser = userDetails?.User;
            //}, e =>
            //{
            //    var message = e.InnerException is IOException ? "No network" : (e.Message ?? "Error");
            //    await Dialogs.Snackbar(message);

            //    fetchedUser = e.CachedResult?.User;
            //});

            if (fetchedUser != null)
                await _dialogService.DisplayAlertAsync(fetchedUser.FirstName, $"{fetchedUser.FirstName} {fetchedUser.LastName}\n {fetchedUser.Email}", "Ok");
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
                await _dialogService.DisplayAlertAsync("Auth", result, "Ok");
        }

        #endregion

        #region Lifecycle

        public void OnAppearing()
        {
            IsRefreshing = true;
        }

        /// <inheritdoc />
        public void OnDisappearing()
        {
            
        }

        #endregion
    }
}
