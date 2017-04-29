using System;
using MvvmCross.Core.ViewModels;
using MvvmCross.Core.Views;
using MvvmCross.Platform.Exceptions;

namespace MvvmCross.Mac.Views
{
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
                CurrentRequest = request;
                var viewType = GetViewType(request.ViewModelType);
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
                CurrentRequest = null;
            }
        }

        public virtual IMvxMacView CreateView(IMvxViewModel viewModel)
        {
            var request = new MvxViewModelInstanceRequest(viewModel);
            var view = CreateView(request);
            return view;
        }
    }
}