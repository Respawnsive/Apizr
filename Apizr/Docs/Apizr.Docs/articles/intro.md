## Intro

Clearly inspired by [Refit.Insane.PowerPack](https://github.com/thefex/Refit.Insane.PowerPack) but extended with a lot more features, the goal of Apizr is to get all ready to use for web api requesting, with the more resiliency we can, but without the boilerplate.

Apizr v3+ relies on Refit v6+ witch makes System.Text.Json the default JSON serializer instead of Newtonsoft.Json. 
If you'd like to continue to use Newtonsoft.Json, add the Refit.Newtonsoft.Json NuGet package and set your ContentSerializer to NewtonsoftJsonContentSerializer on your RefitSettings instance. You can do it by calling the ```WithRefitSettings(...)``` options builder method.

Examples here are based on a Xamarin.Forms app working with Shiny. 
You'll find a sample Xamarin.Forms app browsing code, implementing Apizr with Shiny, Prism and MS DI all together.
You'll find another sample app but .Net Core console this time, implementing Apizr without anything else (static) and also with MS DI (extensions).

So please, take a look at the samples :)