using System;
using System.IO;
using System.Net;
using Cirrious.CrossCore.Interfaces.IoC;
using Cirrious.MvvmCross.ViewModels;

namespace BestSellers.ViewModels
{
    public class BaseViewModel 
        : MvxViewModel
          , IMvxConsumer
    {
        private bool _isLoading;
        public bool IsLoading
        {
            get { return _isLoading; }
            set { _isLoading = value; RaisePropertyChanged("IsLoading"); }
        }

        public void ReportError(string error)
        {
            this.GetService<IErrorReporter>().ReportError(error);
        }

        protected void GeneralAsyncLoad(string url, Action<Stream> responseStreamHandler)
        {
            try
            {
                IsLoading = true;
                var request = WebRequest.Create(url);
                request.BeginGetResponse((result) => GeneralProcessResponse(request, result, responseStreamHandler), null);
            }
            catch (Exception exception)
            {
                IsLoading = false;
                ReportError("Sorry - problem seen " + exception.Message);
            }
        }

        private void GeneralProcessResponse(WebRequest request, IAsyncResult result, Action<Stream> responseStreamHandler)
        {
            IsLoading = false;
            try
            {
                var response = request.EndGetResponse(result);
                using (var stream = response.GetResponseStream())
                {
                    responseStreamHandler(stream);
                }
            }
            catch (Exception exception)
            {
                ReportError("Sorry - problem seen " + exception.Message);
            }
        }
    }
}