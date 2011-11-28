using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Phone7.Fx.Extensions;
using Phone7.Fx.Mvvm;

namespace Phone7.Fx.Navigation
{
    public class UriBuilder<TViewModel> where TViewModel : class
    {
        readonly Dictionary<string, string> _queryString = new Dictionary<string, string>();
        private INavigationService _navigationService;

        public UriBuilder<TViewModel> AttachTo(INavigationService navigationService)
        {
            _navigationService = navigationService;
            return this;
        }

        public UriBuilder<TViewModel> WithParam<TValue>(Expression<Func<TViewModel, TValue>> property, TValue value)
        {
            if (!Equals(default(TValue), value))
            {
                _queryString[property.GetMemberInfo().Name] = value.ToString();
            }

            //queryString[property.GetMemberInfo().Name] = value.ToString();
            //var viewModel = Container.Current.Resolve<TViewModel>();
            //func(viewModel);

            return this;
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents the View Uri
        /// </summary>
        /// <returns>
        /// </returns>
        public override string ToString()
        {
            string url = BuildFullUri();

            if (!string.IsNullOrEmpty(url))
                return url;

            return base.ToString();
        }

        private string BuildFullUri()
        {
            var viewModelType = typeof(TViewModel);
            var att = (ViewModelAttribute)viewModelType.GetCustomAttributes(typeof(ViewModelAttribute), true).FirstOrDefault();
            if (att != null)
            {
                var pageAssemblyName = GetAssemblyName(att.ViewType.Assembly);
                var pageName = att.ViewType.FullName.Replace(pageAssemblyName, "").Replace(".", "/") + ".xaml";

                var qs = BuildQueryString();

                return pageName + qs;
            }
            return string.Empty;
        }

        /// <summary>
        /// Navigates to associate View.
        /// </summary>
        public void Navigate()
        {
            string url = BuildFullUri();

            if (!string.IsNullOrEmpty(url))
                _navigationService.Navigate(url);
        }

        private static string GetAssemblyName(Assembly assembly)
        {
            return assembly.FullName.Remove(assembly.FullName.IndexOf(","));
        }

        private string BuildQueryString()
        {
            if (_queryString.Count < 1)
            {
                return string.Empty;
            }

            var result = _queryString
                .Aggregate("?", (current, pair) => current + (pair.Key + "=" + pair.Value + "&"));

            return result.Remove(result.Length - 1);
        }
    }
}