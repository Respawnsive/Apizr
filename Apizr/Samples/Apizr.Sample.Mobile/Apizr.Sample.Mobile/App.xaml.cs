using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace Apizr.Sample.Mobile
{
    public partial class App : Shiny.FrameworkApplication
    {
        public App() { }

        protected override void OnInitialized()
        {
            InitializeComponent();
            base.Initialize();

            XF.Material.Forms.Material.Init(this);
        }
    }
}
