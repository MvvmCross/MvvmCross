using System.Collections.Generic;
using System.Collections.ObjectModel;
using System;
using System.Windows;
using Microsoft.Phone.Controls;
using System.Linq.Expressions;
using System.ComponentModel;
using System.Windows.Navigation;
using System.Windows.Media;
using System.Linq;

namespace Phone7.Fx.Extensions
{
    public enum Theme
    {
        Light,
        Dark
    }

    public static class ApplicationExtensions
    {
        private static readonly string PhoneLightThemeVisibility = "PhoneLightThemeVisibility";

        private static bool _listeningToNavEvents;
        private static bool _isNavigating;

        public static Theme GetTheme(this Application application)
        {
            var visibility = (Visibility)Application.Current.Resources[PhoneLightThemeVisibility];
            return (visibility == Visibility.Visible) ? Theme.Light : Theme.Dark;
        }

        public static bool IsDesignTime(this Application application)
        {
            return DesignerProperties.GetIsInDesignMode(Application.Current.RootVisual);
        }

        public static void GoBack(this Application application)
        {
            var frame = application.RootVisual as PhoneApplicationFrame;
            EnsureListeningToNaviation(frame);

            if (frame == null)
                return;
            else if (_isNavigating)
                return;
            else if (frame.CanGoBack)
                frame.GoBack(); 
        }

        public static void GoForward(this Application application)
        {
            var frame = application.RootVisual as PhoneApplicationFrame;
            EnsureListeningToNaviation(frame);

            if (frame == null)
                return;
            else if (_isNavigating)
                return;
            else if (frame.CanGoForward)
                frame.GoForward();
        }

        public static void Navigate(this Application application, Uri uri)
        {
            var frame = application.RootVisual as PhoneApplicationFrame;
            EnsureListeningToNaviation(frame);

            //System.Diagnostics.Debug.WriteLine(string.Format("Navigating in App Extension. Frame Content: {0}, Current Source: {1}, Target: {2}, IsNavigating: {3}", frame.Content.GetType().ToString(), frame.CurrentSource, uri, _isNavigating));

            if (frame == null)
                return;
            if (uri == null)
                return;
            if (uri.OriginalString == frame.CurrentSource.OriginalString)
                return;
            if (_isNavigating)
            {
                //System.Diagnostics.Debug.WriteLine("Cancelling nav b/c we're currently navigating");
                return;
            }
            else
            {
                //System.Diagnostics.Debug.WriteLine("Calling frame.nav");
                frame.Navigate(uri);
            }
        }

        public static void Navigate(this Application application, string uri)
        {
            Navigate(application, new Uri(uri, UriKind.Relative));
        }

        public static void Navigate(this Application application, string uri, object context)
        {
            FrameworkElement root = Application.Current.RootVisual as FrameworkElement;
            root.DataContext = context;
            Navigate(application, new Uri(uri, UriKind.Relative));
        }

        public static void Navigate(this Application application, Uri uri, object context)
        {
            FrameworkElement root = Application.Current.RootVisual as FrameworkElement;
            root.DataContext = context;
            Navigate(application, uri);
        }

        private static void EnsureListeningToNaviation(PhoneApplicationFrame frame)
        {
            if (!_listeningToNavEvents)
            {
                frame.Navigating += (sender, e) => _isNavigating = true;
                frame.Navigated += (sender, e) => _isNavigating = false;
                frame.NavigationStopped += (sender, e) => _isNavigating = false;
                frame.NavigationFailed += (sender, e) => _isNavigating = false;
                _listeningToNavEvents = true;
            }
        }

        public static PhoneApplicationPage GetActivePage(this Application application)
        {
            PhoneApplicationPage content = null;
            if (application != null)
            {
                PhoneApplicationFrame rootVisual = application.RootVisual as PhoneApplicationFrame;
                if (rootVisual != null)
                {
                    content = rootVisual.Content as PhoneApplicationPage;
                }
            }
            return content;
        }


        /// <summary>
        /// Returns the first instance of a type implementing T from the Application's 
        /// ApplicationLifetimeObjects list, or null if one cannot be found.
        /// </summary>
        /// <typeparam name="T">The service's type.</typeparam>
        /// <param name="application"></param>
        public static T GetService<T>(this Application application)
        {
            return application.ApplicationLifetimeObjects.OfType<T>().FirstOrDefault();
        }
    }
}
