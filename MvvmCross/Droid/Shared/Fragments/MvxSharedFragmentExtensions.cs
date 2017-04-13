using System;
using MvvmCross.Core.ViewModels;
using MvvmCross.Core.Views;
using MvvmCross.Droid.Shared.Attributes;
using MvvmCross.Platform;
using MvvmCross.Platform.Exceptions;
using MvvmCross.Platform.Platform;

namespace MvvmCross.Droid.Shared.Fragments
{
    public static class MvxSharedFragmentExtensions
    {
        public static Type FindAssociatedViewModelType(this IMvxFragmentView fragmentView, Type fragmentActivityParentType)
        {
            var viewModelType = fragmentView.FindAssociatedViewModelTypeOrNull();

            var type = fragmentView.GetType();

            if (viewModelType == null)
            {
                if (!type.HasMvxFragmentAttribute())
                    throw new InvalidOperationException($"Your fragment is not generic and it does not have {nameof(MvxFragmentAttribute)} attribute set!");

                var cacheableFragmentAttribute = type.GetMvxFragmentAttribute(fragmentActivityParentType);
                if (cacheableFragmentAttribute.ViewModelType == null)
                    throw new InvalidOperationException($"Your fragment is not generic and it does not use {nameof(MvxFragmentAttribute)} with ViewModel Type constructor.");

                viewModelType = cacheableFragmentAttribute.ViewModelType;
            }

            return viewModelType;
        }

        public static IMvxViewModel LoadViewModel(this IMvxFragmentView fragmentView, IMvxBundle savedState, Type fragmentParentActivityType,
            MvxViewModelRequest request = null)
        {
            var viewModelType = fragmentView.FindAssociatedViewModelType(fragmentParentActivityType);
            if (viewModelType == typeof(MvxNullViewModel))
                return new MvxNullViewModel();

            if (viewModelType == null
                || viewModelType == typeof(IMvxViewModel))
            {
                MvxTrace.Trace("No ViewModel class specified for {0} in LoadViewModel",
                    fragmentView.GetType().Name);
            }

            if (request == null)
                request = MvxViewModelRequest.GetDefaultRequest(viewModelType);

            var loaderService = Mvx.Resolve<IMvxViewModelLoader>();
            var viewModel = loaderService.LoadViewModel(request, savedState);

            return viewModel;
        }

        public static void RunViewModelLifecycle(IMvxViewModel viewModel, IMvxBundle savedState,
            MvxViewModelRequest request)
        {
            try
            {
                if (request != null)
                {
                    var parameterValues = new MvxBundle(request.ParameterValues);
                    viewModel.CallBundleMethods("Init", parameterValues);
                }
                if (savedState != null)
                {
                    viewModel.CallBundleMethods("ReloadState", savedState);
                }
                viewModel.Start();
            }
            catch (Exception exception)
            {
                throw exception.MvxWrap("Problem running viewModel lifecycle of type {0}", viewModel.GetType().Name);
            }
        }
    }
}