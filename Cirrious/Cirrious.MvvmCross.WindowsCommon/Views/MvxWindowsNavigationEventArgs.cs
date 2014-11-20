using System;
using Windows.UI.Xaml.Navigation;

namespace Cirrious.MvvmCross.WindowsCommon.Views
{
    /// <summary>
    /// Event arguments for the navigated to and from event. 
    /// </summary>
    public class MvxWindowsNavigationEventArgs
    {
        /// <summary>
        /// Gets the page object which is involved in the navigation. 
        /// </summary>
        public object Content { get; internal set; }

        /// <summary>
        /// Gets the navigation parameter. 
        /// </summary>
        public object Parameter { get; internal set; }

        /// <summary>
        /// Gets the type of the page. 
        /// </summary>
        public Type SourcePageType { get; internal set; }

        /// <summary>
        /// Gets the navigation mode. 
        /// </summary>
        public NavigationMode NavigationMode { get; internal set; }

        /// <summary>
        /// Gets the parameter object as object array or null if it is not an object array. 
        /// </summary>
        public object[] Parameters
        {
            get { return Parameter as object[]; }
        }

        /// <summary>Gets a typed parameter from index assuming the parameter object is an object[]. </summary>
        /// <typeparam name="T">The parameter type. </typeparam>
        /// <param name="index">The parameter index. </param>
        /// <returns>The parameter value. </returns>
        public T GetParameter<T>(int index)
        {
            return (T)Parameters[index];
        }

        /// <summary>Gets a typed parameter. </summary>
        /// <typeparam name="T">The parameter type. </typeparam>
        /// <returns>The parameter value. </returns>
        public T GetParameter<T>()
        {
            return (T)Parameter;
        }
    }
}