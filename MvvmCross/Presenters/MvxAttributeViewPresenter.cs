// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MvvmCross.Logging;
using MvvmCross.Presenters.Attributes;
using MvvmCross.Presenters.Hints;
using MvvmCross.ViewModels;
using MvvmCross.Views;

namespace MvvmCross.Presenters
{
    public abstract class MvxAttributeViewPresenter : MvxViewPresenter, IMvxAttributeViewPresenter
    {
        protected IMvxViewModelTypeFinder _viewModelTypeFinder;
        public virtual IMvxViewModelTypeFinder ViewModelTypeFinder
        {
            get
            {
                if (_viewModelTypeFinder == null)
                    _viewModelTypeFinder = Mvx.IoCProvider.Resolve<IMvxViewModelTypeFinder>();
                return _viewModelTypeFinder;
            }
            set
            {
                _viewModelTypeFinder = value;
            }
        }

        protected IMvxViewsContainer _viewsContainer;
        public virtual IMvxViewsContainer ViewsContainer
        {
            get
            {
                if (_viewsContainer == null)
                    _viewsContainer = Mvx.IoCProvider.Resolve<IMvxViewsContainer>();
                return _viewsContainer;
            }
            set
            {
                _viewsContainer = value;
            }
        }

        protected Dictionary<Type, MvxPresentationAttributeAction> _attributeTypesActionsDictionary;
        public virtual Dictionary<Type, MvxPresentationAttributeAction> AttributeTypesToActionsDictionary
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
            set
            {
                _attributeTypesActionsDictionary = value;
            }
        }

        public abstract void RegisterAttributeTypes();

        public abstract MvxBasePresentationAttribute CreatePresentationAttribute(Type viewModelType, Type viewType);

        public virtual MvxBasePresentationAttribute GetOverridePresentationAttribute(MvxViewModelRequest request, Type viewType)
        {
            if (viewType?.GetInterfaces().Contains(typeof(IMvxOverridePresentationAttribute)) ?? false)
            {
                var viewInstance = Activator.CreateInstance(viewType) as IMvxOverridePresentationAttribute;
                try
                {
                    var presentationAttribute = (viewInstance as IMvxOverridePresentationAttribute)?.PresentationAttribute(request);
                    if (presentationAttribute == null)
                    {
                        MvxLog.Instance.Warn("Override PresentationAttribute null. Falling back to existing attribute.");
                    }
                    else
                    {
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
                }
                finally
                {
                    (viewInstance as IDisposable)?.Dispose();
                }
            }

            return null;
        }

        public virtual MvxBasePresentationAttribute GetPresentationAttribute(MvxViewModelRequest request)
        {
            var viewType = ViewsContainer.GetViewType(request.ViewModelType);

            var overrideAttribute = GetOverridePresentationAttribute(request, viewType);
            if (overrideAttribute != null)
                return overrideAttribute;

            var attribute = viewType
                .GetCustomAttributes(typeof(MvxBasePresentationAttribute), true)
                .FirstOrDefault() as MvxBasePresentationAttribute;
            if (attribute != null)
            {
                if (attribute.ViewType == null)
                    attribute.ViewType = viewType;

                if (attribute.ViewModelType == null)
                    attribute.ViewModelType = request.ViewModelType;

                return attribute;
            }

            return CreatePresentationAttribute(request.ViewModelType, viewType);
        }

        public virtual MvxPresentationAttributeAction GetPresentationAttributeAction(MvxViewModelRequest request, out MvxBasePresentationAttribute attribute)
        {
            attribute = GetPresentationAttribute(request);
            attribute.ViewModelType = request.ViewModelType;
            var viewType = attribute.ViewType;
            var attributeType = attribute.GetType();

            if (AttributeTypesToActionsDictionary.TryGetValue(
                attributeType,
                out MvxPresentationAttributeAction attributeAction))
            {
                if (attributeAction.ShowAction == null)
                    throw new NullReferenceException($"attributeAction.ShowAction is null for attribute: {attributeType.Name}");

                if (attributeAction.CloseAction == null)
                    throw new NullReferenceException($"attributeAction.CloseAction is null for attribute: {attributeType.Name}");

                return attributeAction;
            }

            throw new KeyNotFoundException($"The type {attributeType.Name} is not configured in the presenter dictionary");
        }

        public override async Task<bool> ChangePresentation(MvxPresentationHint hint)
        {
            if (await HandlePresentationChange(hint)) return true;

            if (hint is MvxClosePresentationHint presentationHint)
            {
                return await Close(presentationHint.ViewModelToClose);
            }

            MvxLog.Instance.Warn("Hint ignored {0}", hint.GetType().Name);
            return false;
        }

        public override Task<bool> Close(IMvxViewModel viewModel)
        {
            var attribute = GetPresentationAttribute(new MvxViewModelInstanceRequest(viewModel));
            var attributeType = attribute.GetType();

            if (AttributeTypesToActionsDictionary.TryGetValue(
                attributeType,
                out MvxPresentationAttributeAction attributeAction))
            {
                if (attributeAction.CloseAction == null)
                {
                    throw new NullReferenceException($"attributeAction.CloseAction is null for attribute: {attributeType.Name}");
                }

                return attributeAction.CloseAction.Invoke(viewModel, attribute);
            }

            throw new KeyNotFoundException($"The type {attributeType.Name} is not configured in the presenter dictionary");
        }

        public override Task<bool> Show(MvxViewModelRequest request)
        {
            var attribute = GetPresentationAttribute(request);
            attribute.ViewModelType = request.ViewModelType;
            var attributeType = attribute.GetType();

            if (AttributeTypesToActionsDictionary.TryGetValue(
                attributeType,
                out MvxPresentationAttributeAction attributeAction))
            {
                if (attributeAction.ShowAction == null)
                {
                    throw new NullReferenceException($"attributeAction.ShowAction is null for attribute: {attributeType.Name}");
                }

                return attributeAction.ShowAction.Invoke(attribute.ViewType, attribute, request);
            }

            throw new KeyNotFoundException($"The type {attributeType.Name} is not configured in the presenter dictionary");
        }
    }
}
