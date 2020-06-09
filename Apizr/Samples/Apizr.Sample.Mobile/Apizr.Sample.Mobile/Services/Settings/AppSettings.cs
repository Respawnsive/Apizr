using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Shiny.Settings;

namespace Apizr.Sample.Mobile.Services.Settings
{
    public class AppSettings : ReactiveObject, IAppSettings
    {
        public AppSettings(ISettings settings)
        {
            settings.Bind(this);
        }

        [Reactive] public string? Token { get; set; }
    }
}