// ---------------------------------------------------------------
// <author>Paul Datsyuk</author>
// <url>https://www.linkedin.com/in/pauldatsyuk/</url>
// ---------------------------------------------------------------

using System;
using Xamarin.UITest;
using Xamarin.UITest.Utils;

namespace Zero.UITests
{
    public class AppInitializer
    {
        public static IApp StartApp(Platform platform)
        {
            if (platform == Platform.Android)
            {
                return ConfigureApp
                    .Android
                    //.ApkFile("../../../path/to/Android.apk")
                    .WaitTimes(new WaitTimes())
                    .StartApp();
            }

            return ConfigureApp
                .iOS
                //.AppBundle("../../../path/to/iOS.app")
                .WaitTimes(new WaitTimes())
                .StartApp();
        }
    }

    /// <summary>
    /// Custom implementation of IWaitTimes in order to avoid test failures due to slow emulators.
    /// </summary>
    internal class WaitTimes : IWaitTimes
    {
        public TimeSpan GestureWaitTimeout => TimeSpan.FromMinutes(1);

        public TimeSpan WaitForTimeout => TimeSpan.FromMinutes(1);

        public TimeSpan GestureCompletionTimeout => TimeSpan.FromMinutes(1);
    }
}