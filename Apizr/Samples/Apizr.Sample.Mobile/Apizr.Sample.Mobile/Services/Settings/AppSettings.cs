using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Apizr.Sample.Mobile.Services.Settings
{
    public class AppSettings : ReactiveObject, IAppSettings
    {
        [Reactive] public string? Token { get; set; }
    }
}