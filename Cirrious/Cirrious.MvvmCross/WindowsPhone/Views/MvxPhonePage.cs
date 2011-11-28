using System;
using Cirrious.MvvmCross.Exceptions;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.Application;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;
using Cirrious.MvvmCross.Interfaces.ViewModel;
using Cirrious.MvvmCross.Interfaces.Views;
using Cirrious.MvvmCross.Views;
using Cirrious.MvvmCross.WindowsPhone.Interfaces;
using Microsoft.Phone.Controls;

namespace Cirrious.MvvmCross.WindowsPhone.Views
{
    public abstract class MvxPhonePage<T> 
        : PhoneApplicationPage
        , IMvxView
        , IMvxServiceConsumer<IMvxWindowsPhoneViewModelRequestTranslator>
        , IMvxServiceConsumer<IMvxViewModelLocatorFinder>
        where T : IMvxViewModel
    {
        public virtual void Render() { }

        public T Model { get; set; }
        public Type ModelType { get { return typeof(T); } }
        public void SetModel(object model)
        {
            Model = (T)model;
            DataContext = Model;
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            var translatorService = this.GetService<IMvxWindowsPhoneViewModelRequestTranslator>();
            var viewModelRequest = translatorService.GetRequestFromXamlUri(e.Uri);

            var model = LoadModel(viewModelRequest);

            SetModel(model);
        }

#warning Would ideally like LoadModel to go somewhere in non-platfrom specific code!
        protected IMvxViewModel LoadModel(MvxShowViewModelRequest request)
        {
            var viewModelLocatorFinder = this.GetService<IMvxViewModelLocatorFinder>();
            var viewModelLocator = viewModelLocatorFinder.FindLocator(request);

            if (viewModelLocator == null)
                throw new MvxException("Sorry - somehow there's no viewmodel locator wired up for {0} looking for {1}",
                                       GetType().Name, typeof (T).Name);

            if (viewModelLocator.ViewModelType != typeof(T))
                throw new MvxException(
                    "Sorry - somehow view {0} has been wired up to viewmodel type {1} but received a request to show viewmodel type {2}",
                    GetType().Name, typeof(T).Name, viewModelLocator.ViewModelType.Name);


            IMvxViewModel model = null;
            if (!viewModelLocator.TryLoad(request.ViewModelAction.ActionName, request.ParameterValues, out model))
                throw new MvxException(
                    "Failed to load ViewModel for type {0} action {1}",
                    typeof(T).Name, request.ViewModelAction.ActionName);

            return model;
        }
    }
}
