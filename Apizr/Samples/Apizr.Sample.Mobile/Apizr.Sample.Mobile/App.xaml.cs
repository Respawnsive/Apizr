using Apizr.Sample.Api;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace Apizr.Sample.Mobile
{
    public partial class App : Shiny.FrameworkApplication
    {
        protected override void Initialize()
        {
            InitializeComponent();
            base.Initialize();
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            var test = Container.Resolve(typeof(IApizrManager<IReqResService>));
        }
    }
}
