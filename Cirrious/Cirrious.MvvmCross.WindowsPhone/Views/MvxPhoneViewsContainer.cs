// MvxPhoneViewsContainer.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Linq;
using Cirrious.MvvmCross.Exceptions;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.Platform;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;
using Cirrious.MvvmCross.Views;
using Cirrious.MvvmCross.WindowsPhone.ExtensionMethods;
using Cirrious.MvvmCross.WindowsPhone.Interfaces;
using Microsoft.Phone.Controls;

namespace Cirrious.MvvmCross.WindowsPhone.Views
{
    public class MvxPhoneViewsContainer
        : MvxViewsContainer
          , IMvxWindowsPhoneViewModelRequestTranslator
          , IMvxServiceConsumer
    {
        private const string QueryParameterKeyName = @"ApplicationUrl";

        private readonly PhoneApplicationFrame _rootFrame;

        public MvxPhoneViewsContainer(PhoneApplicationFrame frame)
        {
            _rootFrame = frame;
        }

        #region IMvxWindowsPhoneViewModelRequestTranslator Members

        public virtual MvxShowViewModelRequest GetRequestFromXamlUri(Uri viewUri)
        {
            var parsed = viewUri.ParseQueryString();

            string queryString;
            if (!parsed.TryGetValue(QueryParameterKeyName, out queryString))
                throw new MvxException("Unable to find incoming MvxShowViewModelRequest");

            var text = Uri.UnescapeDataString(queryString);
			var converter = this.GetService<IMvxTextSerializer>();
            return converter.DeserializeObject<MvxShowViewModelRequest>(text);
        }

        public virtual Uri GetXamlUriFor(MvxShowViewModelRequest request)
        {
            var viewType = GetViewType(request.ViewModelType);
            if (viewType == null)
            {
                throw new MvxException("View Type not found for " + request.ViewModelType);
            }

			var converter = this.GetService<IMvxTextSerializer>();
            var requestText = converter.SerializeObject(request);
            var viewUrl = string.Format("{0}?{1}={2}", GetBaseXamlUrlForView(viewType), QueryParameterKeyName,
                                        Uri.EscapeDataString(requestText));
            return new Uri(viewUrl, UriKind.Relative);
        }

        #endregion

        protected static string GetBaseXamlUrlForView(Type viewType)
        {
            string viewUrl;
            var customAttribute =
                (MvxPhoneViewAttribute)
                viewType.GetCustomAttributes(typeof (MvxPhoneViewAttribute), false).FirstOrDefault();

            if (customAttribute == null)
            {
                var splitName = viewType.FullName.Split('.');
                var viewsAndBeyond = splitName.SkipWhile((segment) => segment != "Views");
                viewUrl = string.Format("/{0}.xaml", string.Join("/", viewsAndBeyond));
            }
            else
                viewUrl = customAttribute.Url;

            return viewUrl;
        }
    }
}