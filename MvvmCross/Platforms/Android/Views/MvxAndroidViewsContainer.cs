// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.
#nullable enable

using Android.Content;
using Microsoft.Extensions.Logging;
using MvvmCross.Exceptions;
using MvvmCross.Logging;
using MvvmCross.ViewModels;
using MvvmCross.Views;

namespace MvvmCross.Platforms.Android.Views;

public class MvxAndroidViewsContainer
    : MvxViewsContainer, IMvxAndroidViewsContainer
{
    private const string ExtrasKey = "MvxLaunchData";
    private const string SubViewModelKey = "MvxSubViewModelKey";

    private readonly Context _applicationContext;
    private readonly ILogger<MvxAndroidViewsContainer>? _logger;

    public MvxAndroidViewsContainer(Context applicationContext)
    {
        _applicationContext = applicationContext;
        _logger = MvxLogHost.GetLog<MvxAndroidViewsContainer>();
    }

    #region Implementation of IMvxAndroidViewModelRequestTranslator

    public virtual IMvxViewModel? Load(Intent? intent, IMvxBundle? savedState)
    {
        return Load(intent, null, null);
    }

    public virtual IMvxViewModel? Load(Intent? intent, IMvxBundle? savedState, Type? viewModelTypeHint)
    {
        if (intent == null)
        {
            _logger?.Log(LogLevel.Error, "Null Intent seen when creating ViewModel");
            return null;
        }

        if (intent.Action == Intent.ActionMain)
        {
            _logger?.Log(LogLevel.Trace, "Creating ViewModel for ActionMain");
            return DirectLoad(savedState, viewModelTypeHint);
        }

        if (intent.Extras == null)
        {
            _logger?.Log(LogLevel.Trace, "Null Extras seen on Intent when creating ViewModel - have you tried to navigate to an MvvmCross View directly? Will try direct load");
            return DirectLoad(savedState, viewModelTypeHint);
        }

        if (TryGetEmbeddedViewModel(intent, out var mvxViewModel))
        {
            _logger?.Log(LogLevel.Trace, "Embedded ViewModel used");
            return mvxViewModel;
        }

        _logger?.Log(LogLevel.Trace, "Attempting to load new ViewModel from Intent with Extras");
        var toReturn = CreateViewModelFromIntent(intent, savedState);
        if (toReturn != null)
            return toReturn;

        _logger?.Log(LogLevel.Trace, "ViewModel not loaded from Extras - will try DirectLoad");
        return DirectLoad(savedState, viewModelTypeHint);
    }

    protected virtual IMvxViewModel? DirectLoad(IMvxBundle? savedState, Type? viewModelTypeHint)
    {
        if (viewModelTypeHint == null)
        {
            _logger?.Log(LogLevel.Error, "Unable to load viewmodel - no type hint provided");
            return null;
        }

        if (Mvx.IoCProvider?.TryResolve(out IMvxViewModelLoader? viewModelLoader) != true ||
            viewModelLoader == null)
        {
            return null;
        }

        var viewModelRequest = MvxViewModelRequest.GetDefaultRequest(viewModelTypeHint);
        var viewModel = viewModelLoader.LoadViewModel(viewModelRequest, savedState);
        return viewModel;
    }

    protected virtual IMvxViewModel? CreateViewModelFromIntent(Intent intent, IMvxBundle? savedState)
    {
        var extraData = intent.Extras?.GetString(ExtrasKey);
        if (extraData == null)
            return null;

        if (Mvx.IoCProvider?.TryResolve(out IMvxNavigationSerializer? navigationSerializer) != true ||
            navigationSerializer == null)
        {
            return null;
        }

        var viewModelRequest = navigationSerializer.Serializer.DeserializeObject<MvxViewModelRequest>(extraData);
        return ViewModelFromRequest(viewModelRequest, savedState);
    }

    protected virtual IMvxViewModel? ViewModelFromRequest(MvxViewModelRequest? viewModelRequest, IMvxBundle? savedState)
    {
        if (viewModelRequest == null)
            return null;

        if (Mvx.IoCProvider?.TryResolve(out IMvxViewModelLoader? viewModelLoader) == true && viewModelLoader != null)
        {
            return viewModelLoader.LoadViewModel(viewModelRequest, savedState);
        }

        return null;
    }

    protected virtual bool TryGetEmbeddedViewModel(Intent intent, out IMvxViewModel? mvxViewModel)
    {
        var embeddedViewModelKey = intent.Extras?.GetInt(SubViewModelKey);
        if (embeddedViewModelKey != null && embeddedViewModelKey.Value != 0)
        {
            if (Mvx.IoCProvider?.TryResolve(out IMvxChildViewModelCache? childViewModelCache) != true ||
                childViewModelCache == null)
            {
                mvxViewModel = null;
                return false;
            }

            mvxViewModel = childViewModelCache.Get(embeddedViewModelKey.Value);
            if (mvxViewModel != null)
            {
                RemoveSubViewModelWithKey(embeddedViewModelKey.Value);
                return true;
            }
        }

        mvxViewModel = null;
        return false;
    }

    public virtual Intent GetIntentFor(MvxViewModelRequest request)
    {
        var viewType = GetViewType(request.ViewModelType);
        if (viewType == null)
        {
            throw new MvxException("View Type not found for " + request.ViewModelType);
        }

        var intent = new Intent(_applicationContext, viewType);

        if (Mvx.IoCProvider?.TryResolve(out IMvxNavigationSerializer? navigationSerializer) != true ||
            navigationSerializer == null)
        {
            return intent;
        }

        var requestText = navigationSerializer.Serializer.SerializeObject(request);
        intent.PutExtra(ExtrasKey, requestText);
        AdjustIntentForPresentation(intent, request);

        return intent;
    }

    protected virtual void AdjustIntentForPresentation(Intent intent, MvxViewModelRequest request)
    {
        //todo we want to do things here... clear top, remove history item, etc
        //#warning ClearTop is not enough :/ Need to work on an Intent based scheme like http://stackoverflow.com/questions/3007998/on-logout-clear-activity-history-stack-preventing-back-button-from-opening-l
        //            if (request.ClearTop)
        //                intent.AddFlags(ActivityFlags.ClearTop);
    }

    public virtual (Intent intent, int key) GetIntentWithKeyFor(IMvxViewModel existingViewModelToUse, MvxViewModelRequest? request)
    {
        request ??= MvxViewModelRequest.GetDefaultRequest(existingViewModelToUse.GetType());
        var intent = GetIntentFor(request);

        if (Mvx.IoCProvider?.TryResolve(out IMvxChildViewModelCache? viewModelCache) != true || viewModelCache == null)
        {
            return (intent, -1);
        }

        var key = viewModelCache.Cache(existingViewModelToUse);
        intent.PutExtra(SubViewModelKey, key);
        return (intent, key);
    }

    public void RemoveSubViewModelWithKey(int key)
    {
        if (Mvx.IoCProvider?.TryResolve(out IMvxChildViewModelCache? viewModelCache) == true && viewModelCache != null)
        {
            viewModelCache.Remove(key);
        }
    }

    #endregion Implementation of IMvxAndroidViewModelRequestTranslator
}
