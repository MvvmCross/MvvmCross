﻿// MvxPhoneViewsContainer.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Linq;
using Cirrious.CrossCore.Exceptions;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.MvvmCross.Views;
using Cirrious.MvvmCross.WindowsPhone.Platform;

namespace Cirrious.MvvmCross.WindowsPhone.Views
{
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

            var text = Uri.UnescapeDataString(queryString);
            var converter = Mvx.Resolve<IMvxNavigationSerializer>();
            return converter.Serializer.DeserializeObject<MvxViewModelRequest>(text);
        }

        public virtual Uri GetXamlUriFor(MvxViewModelRequest request)
        {
            var viewType = GetViewType(request.ViewModelType);
            if (viewType == null)
            {
                throw new MvxException("View Type not found for " + request.ViewModelType);
            }

            var converter = Mvx.Resolve<IMvxNavigationSerializer>();
            var requestText = converter.Serializer.SerializeObject(request);
            var viewUrl = string.Format("{0}?{1}={2}", GetBaseXamlUrlForView(viewType), QueryParameterKeyName,
                                        Uri.EscapeDataString(requestText));
            return new Uri(viewUrl, UriKind.Relative);
        }

        protected virtual string GetBaseXamlUrlForView(Type viewType)
        {
            string viewUrl;
            var customAttribute =
                (MvxPhoneViewAttribute)
                viewType.GetCustomAttributes(typeof (MvxPhoneViewAttribute), false).FirstOrDefault();

            if (customAttribute == null)
            {
                viewUrl = GetConventionalXamlUrlForView(viewType);
            }
            else
                viewUrl = customAttribute.Url;

            return viewUrl;
        }

        protected virtual string GetConventionalXamlUrlForView(Type viewType)
        {
            var splitName = viewType.FullName.Split('.');
            var viewsAndBeyond = splitName.SkipWhile((segment) => segment != ViewsFolderName);
            var viewUrl = string.Format("/{0}.xaml", string.Join("/", viewsAndBeyond));
            return viewUrl;
        }

        protected virtual string ViewsFolderName
        {
            get { return "Views"; }
        }
    }
}