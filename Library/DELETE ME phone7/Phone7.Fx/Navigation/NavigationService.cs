using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Phone7.Fx.Extensions;
using Phone7.Fx.Mvvm;
using Phone7.Fx.Ioc;

namespace Phone7.Fx.Navigation
{
    public class NavigationService : INavigationService
    {
        private readonly PhoneApplicationFrame _frame;

        public NavigationService(PhoneApplicationFrame frame)
        {
            _frame = frame;
            _frame.Navigated += OnNavigated;
        }

        protected void OnNavigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            if (e.Content is PhoneApplicationPage)
            {
                var page = (PhoneApplicationPage)e.Content;

                ViewModelBase viewModel = ((ViewModelBase)page.DataContext);

                TryInjectQueryString(viewModel, page);

                viewModel.InitalizeData();

                EventHandler onLayoutUpdate = null;
                onLayoutUpdate = delegate
                {
                    viewModel.OnViewReady();

                    page.LayoutUpdated -= onLayoutUpdate;
                };
                page.LayoutUpdated += onLayoutUpdate;
            }
        }

        private void TryInjectQueryString(ViewModelBase viewModel, PhoneApplicationPage page)
        {
            var viewModelType = viewModel.GetType();

            foreach (var pair in page.NavigationContext.QueryString)
            {
                var property = viewModelType.GetPropertyCaseInsensitive(pair.Key);
                if (property == null)
                    continue;

                //property.SetValue(viewModel,MessageBinder.CoerceValue(property.PropertyType, pair.Value, page.NavigationContext),
                //    null
                //    );
                if (property.PropertyType == typeof(int))
                {
                    property.SetValue(viewModel, int.Parse(pair.Value), null);
                }
                else
                {
                    property.SetValue(viewModel, pair.Value, null);
                }
            }
        }

        public JournalEntry RemoveBackEntry()
        {
            return _frame.RemoveBackEntry();
        }

        /// <summary>
        /// Gets the back stack.
        /// </summary>
        public IEnumerable<JournalEntry> BackStack { get { return _frame.BackStack; } }

        /// <summary>
        /// Goes the back.
        /// </summary>
        public void GoBack()
        {
            _frame.GoBack();
        }

        /// <summary>
        /// Goes the forward.
        /// </summary>
        public void GoForward()
        {
            _frame.GoForward();
        }

        /// <summary>
        /// Gets a value indicating whether this instance can go back.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance can go back; otherwise, <c>false</c>.
        /// </value>
        public bool CanGoBack
        {
            get { return _frame.CanGoBack; }
        }
        /// <summary>
        /// Gets a value indicating whether this instance can go forward.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance can go forward; otherwise, <c>false</c>.
        /// </value>
        public bool CanGoForward
        {
            get { return _frame.CanGoForward; }
        }

        public void Navigate(Uri uri)
        {
            _frame.Navigate(uri);
        }

        public void Navigate(string pageName)
        {
            Navigate(new Uri(pageName, UriKind.Relative));
        }
    }
}