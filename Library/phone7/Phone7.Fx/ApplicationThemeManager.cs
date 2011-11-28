using System.Windows;

namespace Phone7.Fx
{
    public enum ApplicationTheme
    {
        Dark,
        Light
    }

    public class ApplicationThemeManager
    {
        /// <summary>
        /// Get the current theme of the Phone
        /// </summary>
        public static ApplicationTheme Theme
        {
            get
            {
                var color = Application.Current.Host.Background;
                if (color.A == 255)
                    return ApplicationTheme.Dark;
                return ApplicationTheme.Light;
            }
        }
    }
}