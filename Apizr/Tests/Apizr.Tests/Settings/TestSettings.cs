namespace Apizr.Tests.Settings
{
    public class TestSettings
    {
        /// <summary>
        /// Bound from appsettings.json
        /// </summary>
        public string TestJsonString { get; private set; }

        public string TestSessionString { get; set; }
    }
}
