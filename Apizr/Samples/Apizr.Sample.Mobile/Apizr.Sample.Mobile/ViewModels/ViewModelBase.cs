using Prism.Navigation;
using System.Threading.Tasks;
using Shiny;

namespace Apizr.Sample.Mobile.ViewModels
{
    public class ViewModelBase : ViewModel, IInitialize
    {
        protected ViewModelBase(INavigationService navigationService)
        {
            NavigationService = navigationService;
        }

        #region Properties

        protected INavigationService NavigationService { get; }

        #endregion

        #region Methods

        #endregion

        #region Lifecycle

        public virtual void Initialize(INavigationParameters parameters) { }

        public virtual Task NavigateBackAsync() => NavigationService.GoBackAsync();

        #endregion
    }
}
