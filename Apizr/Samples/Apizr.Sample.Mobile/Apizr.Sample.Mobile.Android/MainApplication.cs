using System;
using Android.App;
using Android.Runtime;
using Shiny;

namespace Apizr.Sample.Mobile.Droid
{
    [Application]
    public class MainApplication : ShinyAndroidApplication<Startup>
    {
        public MainApplication(IntPtr handle, JniHandleOwnership transfer) : base(handle, transfer)
        {
        }
    }
}