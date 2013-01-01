#region Copyright

// <copyright file="MvxDefaultViewModelLocator.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
//  
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com

#endregion

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