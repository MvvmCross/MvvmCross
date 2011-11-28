using System;
using System.Linq;
using Cirrious.MvvmCross.Exceptions;
using Cirrious.MvvmCross.Interfaces.Views;
using Cirrious.MvvmCross.Views;
using Cirrious.MvvmCross.WindowsPhone.Interfaces;
using Microsoft.Phone.Controls;

namespace Cirrious.MvvmCross.WindowsPhone.Views
{
	public class MvxPhoneViewsContainer : MvxViewsContainer, IMvxWindowsPhoneViewModelRequestTranslator, IMvxViewDispatcherProvider
	{
		public static MvxPhoneViewsContainer PhoneViewModelContainerInstance { get { return Instance as MvxPhoneViewsContainer; } }
		private readonly PhoneApplicationFrame _rootFrame;

		public IMvxViewDispatcher Dispatcher
		{
			get { return new MvxPhoneViewDispatcher(_rootFrame); }
		}

		public MvxPhoneViewsContainer(PhoneApplicationFrame frame)
		{
			_rootFrame = frame;
		}

		private const string QueryParameterText = @"ApplicationUrl=";

		public MvxShowViewModelRequest GetRequestFromXamlUri(Uri viewUri)
		{
			var queryString = viewUri.ToString();
			var queryParameterIndex = queryString.IndexOf(QueryParameterText);
			if (queryParameterIndex >= 0)
				queryString = queryString.Substring(queryParameterIndex + QueryParameterText.Length);

			var text = Uri.UnescapeDataString(queryString);
			return Newtonsoft.Json.JsonConvert.DeserializeObject<MvxShowViewModelRequest>(text);
		}

		public Uri GetXamlUriFor(MvxShowViewModelRequest request)
		{
			var viewType = GetViewType(request.ViewModelAction);
			if (viewType == null)
			{
				throw new MvxException("View Type not found for " + request.ViewModelAction);
			}

            var requestText = Newtonsoft.Json.JsonConvert.SerializeObject(request);
			var viewUrl = string.Format("{0}?{1}{2}", GetBaseXamlUrlForView(viewType), QueryParameterText, Uri.EscapeDataString(requestText));
			return new Uri(viewUrl, UriKind.Relative);
		}

		private static string GetBaseXamlUrlForView(Type viewType)
		{
			string viewUrl;
			var customAttribute =
				(MvxPhoneViewAttribute)
				viewType.GetCustomAttributes(typeof (MvxPhoneViewAttribute), false).FirstOrDefault();
			
			if (customAttribute == null)
				viewUrl = string.Format("/Views/{0}.xaml", viewType.Name);
			else
				viewUrl = customAttribute.Url;

			return viewUrl;
		}
	}
}