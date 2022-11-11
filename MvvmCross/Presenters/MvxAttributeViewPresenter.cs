// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MvvmCross.Logging;
using MvvmCross.Presenters.Attributes;
using MvvmCross.Presenters.Hints;
using MvvmCross.ViewModels;
using MvvmCross.Views;

namespace MvvmCross.Presenters
{
#nullable enable
    public abstract class MvxAttributeViewPresenter : MvxViewPresenter, IMvxAttributeViewPresenter
    {
        private readonly Lazy<IMvxViewModelTypeFinder> _viewModelTypeFinder =
            new Lazy<IMvxViewModelTypeFinder>(() => Mvx.IoCProvider.Resolve<IMvxViewModelTypeFinder>());

        private readonly Lazy<IMvxViewsContainer> _viewsContainer =
            new Lazy<IMvxViewsContainer>(() => Mvx.IoCProvider.Resolve<IMvxViewsContainer>());

        private IDictionary<Type, MvxPresentationAttributeAction>? _attributeTypesActionsDictionary;

        public virtual IMvxViewModelTypeFinder? ViewModelTypeFinder => _viewModelTypeFinder.Value;

        public virtual IMvxViewsContainer? ViewsContainer => _viewsContainer.Value;

        public virtual IDictionary<Type, MvxPresentationAttributeAction> AttributeTypesToActionsDictionary
        {
            get
            {
                if (_attributeTypesActionsDictionary == null)
                {
                    _attributeTypesActionsDictionary = new Dictionary<Type, MvxPresentationAttributeAction>();
                    RegisterAttributeTypes();
                }
                return _attributeTypesActionsDictionary;
            }
        }

        public abstract void RegisterAttributeTypes();

        public abstract MvxBasePresentationAttribute CreatePresentationAttribute(Type viewModelType, Type viewType);

        public virtual object? CreateOverridePresentationAttributeViewInstance(Type viewType)
        {
            if (viewType == null)
                throw new ArgumentNullException(nameof(viewType));

            return Activator.CreateInstance(viewType);
        }

        public virtual MvxBasePresentationAttribute? GetOverridePresentationAttribute(
            MvxViewModelRequest request, Type viewType)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            if (viewType == null)
                throw new ArgumentNullException(nameof(viewType));

            var hasInterface = viewType.GetInterfaces().Contains(typeof(IMvxOverridePresentationAttribute));
            if (!hasInterface)
                return null;

            var viewInstance =
                CreateOverridePresentationAttributeViewInstance(viewType) as IMvxOverridePresentationAttribute;
            try
            {
                var presentationAttribute = viewInstance?.PresentationAttribute(request);
                if (presentationAttribute == null)
                    return null;

                if (presentationAttribute.ViewType == null)
                {
                    presentationAttribute.ViewType = viewType;
                }

                if (presentationAttribute.ViewModelType == null)
                {
                    presentationAttribute.ViewModelType = request.ViewModelType;
                }

                return presentationAttribute;
            }
            finally
            {
                (viewInstance as IDisposable)?.Dispose();
            }
        }

        public virtual MvxBasePresentationAttribute GetPresentationAttribute(MvxViewModelRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            if (request.ViewModelType == null)
                throw new InvalidOperationException("Cannot get view types for null ViewModelType");

            if (ViewsContainer == null)
                throw new InvalidOperationException($"Cannot get view types from null {nameof(ViewsContainer)}");

            var viewType = ViewsContainer.GetViewType(request.ViewModelType);
            if (viewType == null)
                throw new InvalidOperationException($"Could not get View Type for ViewModel Type {request.ViewModelType}");

            var overrideAttribute = GetOverridePresentationAttribute(request, viewType);
            if (overrideAttribute != null)
                return overrideAttribute;

            var attribute = viewType
                .GetCustomAttributes(typeof(MvxBasePresentationAttribute), true)
                .FirstOrDefault();

            if (attribute is MvxBasePresentationAttribute basePresentationAttribute)
            {
                if (basePresentationAttribute.ViewType == null)
                    basePresentationAttribute.ViewType = viewType;

                if (basePresentationAttribute.ViewModelType == null)
                    basePresentationAttribute.ViewModelType = request.ViewModelType;

                return basePresentationAttribute;
            }

            return CreatePresentationAttribute(request.ViewModelType, viewType);
        }

        protected virtual MvxPresentationAttributeAction GetPresentationAttributeAction(
            MvxViewModelRequest request, out MvxBasePresentationAttribute attribute)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            var presentationAttribute = GetPresentationAttribute(request);
            presentationAttribute.ViewModelType = request.ViewModelType;
            var attributeType = presentationAttribute.GetType();

            attribute = presentationAttribute;

            if (AttributeTypesToActionsDictionary != null &&
                AttributeTypesToActionsDictionary.TryGetValue(attributeType,
                out MvxPresentationAttributeAction attributeAction))
            {
                if (attributeAction.ShowAction == null)
                {
                    throw new InvalidOperationException(
                        $"attributeAction.ShowAction is null for attribute: {attributeType.Name}");
                }

                if (attributeAction.CloseAction == null)
                {
                    throw new InvalidOperationException(
                        $"attributeAction.CloseAction is null for attribute: {attributeType.Name}");
                }

                return attributeAction;
            }

            throw new KeyNotFoundException($"The type {attributeType.Name} is not configured in the presenter dictionary");
        }

        public override async Task<bool> ChangePresentation(MvxPresentationHint hint)
        {
            if (await HandlePresentationChange(hint).ConfigureAwait(true))
                return true;

            if (hint is MvxClosePresentationHint presentationHint)
            {
                return await Close(presentationHint.ViewModelToClose).ConfigureAwait(true);
            }

            MvxLogHost.Default?.Log(LogLevel.Warning, "Hint ignored {name}", hint.GetType().Name);
            return false;
        }

        public override Task<bool> Close(IMvxViewModel viewModel)
        {
            return GetPresentationAttributeAction(
                new MvxViewModelInstanceRequest(viewModel), out MvxBasePresentationAttribute attribute)
                    .CloseAction?
                    .Invoke(viewModel, attribute) ?? Task.FromResult(false);
        }

        public override Task<bool> Show(MvxViewModelRequest request)
        {
            var attributeAction = GetPresentationAttributeAction(request, out MvxBasePresentationAttribute attribute);

            if (attributeAction.ShowAction != null && attribute.ViewType != null)
                return attributeAction.ShowAction.Invoke(attribute.ViewType, attribute, request);

            return Task.FromResult(false);
        }
    }
#nullable restore
}
