// MvxDefaultViewModelLocator.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Collections.Generic;
using System.Linq;
using Cirrious.MvvmCross.Interfaces.ViewModels;

namespace Cirrious.MvvmCross.Application
{
    public class MvxDefaultViewModelLocator
        : MvxBaseViewModelLocator
    {
        #region IMvxViewModelLocator Members

        public override bool TryLoad(Type viewModelType, IDictionary<string, string> parameterValueLookup,
                                     out IMvxViewModel model)
        {
            model = null;
            var constructor = viewModelType
                .GetConstructors()
                .FirstOrDefault(c => c.GetParameters().All(p => IsConvertibleParameter(p)));

            if (constructor == null)
                return false;

            var invokeWith = CreateArgumentList(viewModelType, parameterValueLookup, constructor.GetParameters());
            model = Activator.CreateInstance(viewModelType, invokeWith.ToArray()) as IMvxViewModel;
            return (model != null);
        }

        #endregion
    }
}