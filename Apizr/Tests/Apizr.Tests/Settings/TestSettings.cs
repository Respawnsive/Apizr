namespace Apizr.Tests.Settings
{
    public class TestSettings
    {
        public TestSettings() { }

        public TestSettings(string testJsonString)
        {
            TestJsonString = testJsonString;
        }

        /// <summary>
        /// Bound from appsettings.json
        /// </summary>
        public string TestJsonString { get; private set; }

        public string TestSessionString { get; set; }
    }
}
