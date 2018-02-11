// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using MvvmCross.Logging;
using MvvmCross.ViewModels;
using MvvmCross.Views;

namespace MvvmCross.Presenters
{
    public abstract class MvxAttributeViewPresenter : MvxViewPresenter, IMvxAttributeViewPresenter
    {
        private IMvxViewModelTypeFinder _viewModelTypeFinder;
        public IMvxViewModelTypeFinder ViewModelTypeFinder
        {
            get
            {
                if (_viewModelTypeFinder == null)
                    _viewModelTypeFinder = Mvx.Resolve<IMvxViewModelTypeFinder>();
                return _viewModelTypeFinder;
            }
            set
            {
                _viewModelTypeFinder = value;
            }
        }

        private IMvxViewsContainer _viewsContainer;
        public IMvxViewsContainer ViewsContainer
        {
            get
            {
                if (_viewsContainer == null)
                    _viewsContainer = Mvx.Resolve<IMvxViewsContainer>();
                return _viewsContainer;
            }
            set
            {
                _viewsContainer = value;
            }
        }

        private Dictionary<Type, MvxPresentationAttributeAction> _attributeTypesActionsDictionary;
        public Dictionary<Type, MvxPresentationAttributeAction> AttributeTypesToActionsDictionary
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

        public override void ChangePresentation(MvxPresentationHint hint)
        {
            if (HandlePresentationChange(hint)) return;

            if (hint is MvxClosePresentationHint presentationHint)
            {
                Close(presentationHint.ViewModelToClose);
                return;
            }

            MvxLog.Instance.Warn("Hint ignored {0}", hint.GetType().Name);
        }

        public override void Close(IMvxViewModel viewModel)
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

                attributeAction.CloseAction.Invoke(viewModel, attribute);
                return;
            }

            throw new KeyNotFoundException($"The type {attributeType.Name} is not configured in the presenter dictionary");
        }

        public override void Show(MvxViewModelRequest request)
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

                attributeAction.ShowAction.Invoke(attribute.ViewType, attribute, request);
                return;
            }

            throw new KeyNotFoundException($"The type {attributeType.Name} is not configured in the presenter dictionary");
        }
    }
}
