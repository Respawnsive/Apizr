namespace Apizr.Integrations.Fusillade
{
    public static class OptionsBuilderExtensions
    {
        public static T WithPriorityManagement<T>(this T builder) where T : IApizrOptionsBuilderBase
        {
            builder.SetPrimaryHttpMessageHandler((innerHandler, logHandler) => new PriorityHttpMessageHandler(innerHandler, logHandler));

            return builder;
        }
    }
}
