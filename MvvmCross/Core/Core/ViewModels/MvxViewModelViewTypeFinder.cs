// MvxViewModelViewTypeFinder.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Core.ViewModels
{
    using System;
    using System.Linq;
    using System.Reflection;

    using MvvmCross.Core.Views;
    using MvvmCross.Platform;
    using MvvmCross.Platform.IoC;
    using MvvmCross.Platform.Platform;

    public class MvxViewModelViewTypeFinder
        : IMvxViewModelTypeFinder
    {
        private readonly IMvxViewModelByNameLookup _viewModelByNameLookup;
        private readonly IMvxNameMapping _viewToViewModelNameMapping;

        public MvxViewModelViewTypeFinder(IMvxViewModelByNameLookup viewModelByNameLookup, IMvxNameMapping viewToViewModelNameMapping)
        {
            this._viewModelByNameLookup = viewModelByNameLookup;
            this._viewToViewModelNameMapping = viewToViewModelNameMapping;
        }

        public virtual Type FindTypeOrNull(Type candidateType)
        {
            if (!this.CheckCandidateTypeIsAView(candidateType))
                return null;

            if (!candidateType.IsConventional())
                return null;

            var typeByAttribute = this.LookupAttributedViewModelType(candidateType);
            if (typeByAttribute != null)
                return typeByAttribute;

            var concrete = this.LookupAssociatedConcreteViewModelType(candidateType);
            if (concrete != null)
                return concrete;

            var typeByName = this.LookupNamedViewModelType(candidateType);
            if (typeByName != null)
                return typeByName;

            MvxTrace.Trace("No view model association found for candidate view {0}", candidateType.Name);
            return null;
        }

        protected virtual Type LookupAttributedViewModelType(Type candidateType)
        {
            var attribute = candidateType
                                .GetCustomAttributes(typeof(MvxViewForAttribute), false)
                                .FirstOrDefault() as MvxViewForAttribute;

            return attribute?.ViewModel;
        }

        protected virtual Type LookupNamedViewModelType(Type candidateType)
        {
            var viewName = candidateType.Name;
            var viewModelName = this._viewToViewModelNameMapping.Map(viewName);

            Type toReturn;
            this._viewModelByNameLookup.TryLookupByName(viewModelName, out toReturn);
            return toReturn;
        }

        protected virtual Type LookupAssociatedConcreteViewModelType(Type candidateType)
        {
            var viewModelPropertyInfo = candidateType
                .GetProperties()
                .FirstOrDefault(x => x.Name == "ViewModel"
                                     && !x.PropertyType.GetTypeInfo().IsInterface
                                     && !x.PropertyType.GetTypeInfo().IsAbstract);

            return viewModelPropertyInfo?.PropertyType;
        }

        protected virtual bool CheckCandidateTypeIsAView(Type candidateType)
        {
            if (candidateType == null)
                return false;

            if (candidateType.GetTypeInfo().IsAbstract)
                return false;

            if (!typeof(IMvxView).IsAssignableFrom(candidateType))
                return false;

            return true;
        }
    }
}