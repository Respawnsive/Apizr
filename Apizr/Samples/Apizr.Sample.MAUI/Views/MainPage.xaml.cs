using Apizr.Sample.MAUI.ViewModels;

namespace Apizr.Sample.MAUI.Views
{
    public partial class MainPage : ContentPage
    {
        public MainPage(MainPageViewModel viewModel)
        {
            BindingContext = viewModel;
            InitializeComponent();
        }
    }
}