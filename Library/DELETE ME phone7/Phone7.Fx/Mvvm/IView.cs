using System;
using System.Windows;
using System.Windows.Navigation;

namespace Phone7.Fx.Mvvm
{
    public interface IView
    {
        /// <summary>
        /// Gets or sets the data context.
        /// </summary>
        /// <value>The data context.</value>
        object DataContext { get; set; }

        /// <summary>
        /// Gets the navigation service.
        /// </summary>
        /// <value>The navigation service.</value>
        NavigationService NavigationService { get; }

        /// <summary>
        /// Gets the navigation context.
        /// </summary>
        /// <value>The navigation context.</value>
        NavigationContext NavigationContext { get; }

        /// <summary>
        /// Occurs when [loaded].
        /// </summary>
        event RoutedEventHandler Loaded;

        event EventHandler LayoutUpdated;

        //void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e);
    }
}