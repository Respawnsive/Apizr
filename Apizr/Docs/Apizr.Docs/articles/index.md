## Intro

The goal of Apizr is to get all ready to use for web api requesting, with the more resiliency we can, but without the boilerplate.

Examples through this doc are mainly based on a Xamarin.Forms app working with Shiny. 
Exploring the GitHub repository, you'll find a full Xamarin.Forms sample app, implementing Apizr with Shiny, Prism and MS DI all together.
You'll find another mobile sample app with MAUI in preview.
There's also a .Net Core console sample app, implementing Apizr without anything else (static) and also with MS DI (extensions).

Feel free to take a look at the sample and test projects.

Documentation structure is quite simple.
There is first a Getting started for both classic and CRUD apis.
Then, there is almost everything we can configure and use with Apizr.

Happy coding


>[!WARNING]
>
>**Breaking changes**
>
>Apizr v4.1 brings some renaming breaking changes:
>- **Apizr static class renamed to ApizrBuilder to match its purpose** and doesn't conflict with its namespace anymore
>- **ApizrBuilder's methods renamed to match their return type** so that we know what we're about to build (e.g. CreateRegistry, AddManagerFor, CreateManagerFor)
>- **ApizrRegistry's methods renamed to match their return type** so that we know what we're about to get (e.g. GetManagerFor, GetCrudManagerFor, ContainsManagerFor)