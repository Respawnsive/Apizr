using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace Apizr.Sample.Forms
{
    public partial class App : Shiny.FrameworkApplication
    {
        protected override void Initialize()
        {
            XF.Material.Forms.Material.Init(this);
            InitializeComponent();
            base.Initialize();
        }
    }
}
