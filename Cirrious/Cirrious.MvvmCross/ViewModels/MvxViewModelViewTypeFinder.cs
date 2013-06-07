// MvxViewModelViewTypeFinder.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Linq;
using Cirrious.CrossCore.IoC;
using Cirrious.CrossCore.Platform;
using Cirrious.MvvmCross.Views;

namespace Cirrious.MvvmCross.ViewModels
{
    public class MvxViewModelViewTypeFinder
        : IMvxViewModelTypeFinder
    {
        private readonly IMvxViewModelByNameLookup _viewModelByNameLookup;

        public MvxViewModelViewTypeFinder(IMvxViewModelByNameLookup viewModelByNameLookup)
        {
            _viewModelByNameLookup = viewModelByNameLookup;
        }

        public virtual Type FindTypeOrNull(Type candidateType)
        {
            if (!CheckCandidateTypeIsAView(candidateType))
                return null;

            if (!candidateType.IsConventional())
                return null;

            var typeByAttribute = LookupAttributedViewModelType(candidateType);
            if (typeByAttribute != null)
                return typeByAttribute;

            var concrete = LookupAssociatedConcreteViewModelType(candidateType);
            if (concrete != null)
                return concrete;

            var typeByName = LookupNamedViewModelType(candidateType);
            if (typeByName != null)
                return typeByName;

            MvxTrace.Trace("No view model association found for candidate view {0}", candidateType.Name);
            return null;
        }

        protected virtual Type LookupAttributedViewModelType(Type candidateType)
        {
            var attribute = candidateType
                                .GetCustomAttributes(typeof (MvxViewForAttribute), false)
                                .FirstOrDefault() as MvxViewForAttribute;

            if (attribute == null)
                return null;

            return attribute.ViewModel;
        }

        protected virtual Type LookupNamedViewModelType(Type candidateType)
        {
            var viewName = candidateType.Name;
            var viewModelName = viewName + "Model";

            Type toReturn;
            _viewModelByNameLookup.TryLookupByName(viewModelName, out toReturn);
            return toReturn;
        }

        protected virtual Type LookupAssociatedConcreteViewModelType(Type candidateType)
        {
            var viewModelPropertyInfo = candidateType
                .GetProperties()
                .FirstOrDefault(x => x.Name == "ViewModel"
                                     && !x.PropertyType.IsInterface
                                     && !x.PropertyType.IsAbstract);

            if (viewModelPropertyInfo == null)
                return null;

            return viewModelPropertyInfo.PropertyType;
        }

        protected virtual bool CheckCandidateTypeIsAView(Type candidateType)
        {
            if (candidateType == null)
                return false;

            if (candidateType.IsAbstract)
                return false;

            if (!typeof (IMvxView).IsAssignableFrom(candidateType))
                return false;

            return true;
        }
    }
}