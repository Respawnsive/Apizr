using Prism.Navigation;
using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Prism.AppModel;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Apizr.Sample.Mobile.ViewModels
{
    public class ViewModelBase : ReactiveObject,
        IAutoInitialize,
        IInitialize,
        IInitializeAsync,
        INavigatedAware,
        IPageLifecycleAware,
        IDestructible,
        IConfirmNavigationAsync
    {
        protected ViewModelBase(INavigationService navigationService)
        {
            NavigationService = navigationService;

            // Set IsBusy to true while BusyCounter > 0
            this.WhenAnyValue(x => x.BusyCounter)
                .Select(busyCounter => busyCounter > 0)
                .ObserveOn(RxApp.MainThreadScheduler)
                .ToPropertyEx(this, vm => vm.IsBusy)
                .DisposeWith(DestroyWith);
        }

        #region Properties

        protected INavigationService NavigationService { get; }

        [Reactive] public int BusyCounter { get; protected set; }

        public bool IsBusy { [ObservableAsProperty] get; }

        [Reactive]
        public string Title { get; protected set; }

        #endregion

        #region Methods

        protected virtual void ToggleIsBusy(bool isExecuting)
        {
            if (isExecuting)
                BusyCounter++;
            else
                BusyCounter--;
        }

        #endregion

        #region Lifecycle

        private CompositeDisposable _deactivateWith;
        protected CompositeDisposable DeactivateWith => _deactivateWith ??= new CompositeDisposable();

        protected CompositeDisposable DestroyWith { get; } = new CompositeDisposable();

        protected virtual void Deactivate()
        {
            _deactivateWith?.Dispose();
            _deactivateWith = null;
        }

        public virtual void OnNavigatedFrom(INavigationParameters parameters) => this.Deactivate();

        public virtual void Initialize(INavigationParameters parameters) { }

        public virtual Task InitializeAsync(INavigationParameters parameters) => Task.CompletedTask;

        public virtual void OnNavigatedTo(INavigationParameters parameters) { }

        public virtual void OnAppearing() { }

        public virtual void OnDisappearing() { }

        public virtual void Destroy() => this.DestroyWith?.Dispose();

        public virtual Task<bool> CanNavigateAsync(INavigationParameters parameters) => Task.FromResult(true);

        public virtual Task NavigateBackAsync() => NavigationService.GoBackAsync();

        #endregion
    }
}
