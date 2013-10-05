

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
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace Cirrious.MvvmCross.ViewModels
{
	public interface IMvxNameMapping
	{
		string MapName(string inputName);
	}

	public class MvxViewToViewModelNameMapping
		: IMvxNameMapping
	{
		public virtual string MapName(string inputName)
		{
			return inputName + "Model";
		}
	}

	public class MvxPostfixAwareViewToViewModelNameMapping
		: MvxViewToViewModelNameMapping
	{
		private readonly IList<string> _postfixes;

		public MvxPostfixAwareViewToViewModelNameMapping (IList<string> postfixes)
		{
			_postfixes = postfixes;
		}

		public override string MapName(string inputName)
		{
			foreach (var postfix in _postfixes) {
				if (inputName.EndsWith (postfix) && inputName.Length > postfix.Length) 
				{
					inputName = inputName.Substring (0, inputName.Length - postfix.Length);
					inputName += "View";
					break;
				}
			}
			return base.MapName(inputName);
		}
	}
	

    public class MvxViewModelViewTypeFinder
        : IMvxViewModelTypeFinder
    {
        private readonly IMvxViewModelByNameLookup _viewModelByNameLookup;
		private readonly IMvxNameMapping _viewToViewModelNameMapping;

		public MvxViewModelViewTypeFinder(IMvxViewModelByNameLookup viewModelByNameLookup, IMvxNameMapping viewToViewModelNameMapping)
        {
            _viewModelByNameLookup = viewModelByNameLookup;
			_viewToViewModelNameMapping = viewToViewModelNameMapping;
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
			var viewModelName = _viewToViewModelNameMapping.MapName(viewName);

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