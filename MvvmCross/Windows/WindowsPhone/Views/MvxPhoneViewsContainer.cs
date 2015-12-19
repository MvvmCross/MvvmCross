// MvxPhoneViewsContainer.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.WindowsPhone.Views
{
    using System;
    using System.Linq;

    using MvvmCross.Core.ViewModels;
    using MvvmCross.Core.Views;
    using MvvmCross.Platform;
    using MvvmCross.Platform.Exceptions;
    using MvvmCross.WindowsPhone.Platform;

    public class MvxPhoneViewsContainer
        : MvxViewsContainer
        , IMvxPhoneViewsContainer
    {
        private const string QueryParameterKeyName = @"ApplicationUrl";

        public virtual MvxViewModelRequest GetRequestFromXamlUri(Uri viewUri)
        {
            var parsed = viewUri.ParseQueryString();

            string queryString;
            if (!parsed.TryGetValue(QueryParameterKeyName, out queryString))
                throw new MvxException("Unable to find incoming MvxViewModelRequest");

            var converter = Mvx.Resolve<IMvxNavigationSerializer>();
            return converter.Serializer.DeserializeObject<MvxViewModelRequest>(queryString);
        }

        public virtual Uri GetXamlUriFor(MvxViewModelRequest request)
        {
            var viewType = this.GetViewType(request.ViewModelType);
            if (viewType == null)
            {
                throw new MvxException("View Type not found for " + request.ViewModelType);
            }

            var converter = Mvx.Resolve<IMvxNavigationSerializer>();
            var requestText = converter.Serializer.SerializeObject(request);
            var viewUrl =
                $"{this.GetBaseXamlUrlForView(viewType)}?{QueryParameterKeyName}={Uri.EscapeDataString(requestText)}";
            return new Uri(viewUrl, UriKind.Relative);
        }

        protected virtual string GetBaseXamlUrlForView(Type viewType)
        {
            var customAttribute =
                (MvxPhoneViewAttribute)
                viewType.GetCustomAttributes(typeof(MvxPhoneViewAttribute), false).FirstOrDefault();

            string viewUrl = customAttribute == null ? this.GetConventionalXamlUrlForView(viewType) : customAttribute.Url;

            return viewUrl;
        }

        protected virtual string GetConventionalXamlUrlForView(Type viewType)
        {
            var splitName = viewType.FullName.Split('.');
            var viewsAndBeyond = splitName.SkipWhile((segment) => segment != this.ViewsFolderName);
            var viewUrl = $"/{string.Join("/", viewsAndBeyond)}.xaml";
            return viewUrl;
        }

        protected virtual string ViewsFolderName => "Views";
    }
}