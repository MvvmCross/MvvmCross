
namespace MvvmCross.Mac.Views
{
    using System;

    using global::MvvmCross.Core.ViewModels;
    using global::MvvmCross.Core.Views;
    using global::MvvmCross.Platform.Exceptions;

    // TODO - move this into another file
    public interface IMvxMacViewsContainer
        : IMvxViewsContainer
        , IMvxMacViewCreator
        , IMvxCurrentRequest
    {
    }

    public class MvxMacViewsContainer
        : MvxViewsContainer, IMvxMacViewsContainer
    {
        public MvxViewModelRequest CurrentRequest { get; private set; }

        public virtual IMvxMacView CreateView(MvxViewModelRequest request)
        {
            try
            {
                this.CurrentRequest = request;
                var viewType = this.GetViewType(request.ViewModelType);
                if (viewType == null)
                    throw new MvxException("View Type not found for " + request.ViewModelType);

                var view = Activator.CreateInstance(viewType) as IMvxMacView;
                if (view == null)
                    throw new MvxException("View not loaded for " + viewType);
                view.Request = request;
                return view;
            }
            finally
            {
                this.CurrentRequest = null;
            }
        }

        public virtual IMvxMacView CreateView(IMvxViewModel viewModel)
        {
            var request = new MvxViewModelInstanceRequest(viewModel);
            var view = this.CreateView(request);
            return view;
        }
    }
}