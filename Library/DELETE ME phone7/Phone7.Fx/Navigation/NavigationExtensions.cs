namespace Phone7.Fx.Navigation
{
    public static class NavigationExtensions
    {
        /// <summary>
        /// Creates a Uri builder based on a view model type.
        /// </summary>
        /// <typeparam name="TViewModel">The type of the view model.</typeparam>
        /// <param name="navigationService">The navigation service.</param>
        /// <returns>The builder.</returns>
        public static UriBuilder<TViewModel> UriFor<TViewModel>(this INavigationService navigationService) where TViewModel : class
        {
            return new UriBuilder<TViewModel>().AttachTo(navigationService);
        }
    }
}