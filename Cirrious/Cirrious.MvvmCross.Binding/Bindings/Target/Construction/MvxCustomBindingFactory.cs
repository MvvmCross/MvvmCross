#region Copyright
// <copyright file="MvxCustomBindingFactory.cs" company="Cirrious">
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
using Cirrious.MvvmCross.Binding.Interfaces;
using Cirrious.MvvmCross.Binding.Interfaces.Bindings.Target;
using Cirrious.MvvmCross.Binding.Interfaces.Bindings.Target.Construction;
using Cirrious.MvvmCross.Interfaces.Platform.Diagnostics;

namespace Cirrious.MvvmCross.Binding.Bindings.Target.Construction
{
    public class MvxCustomBindingFactory<TTarget>
        : IMvxPluginTargetBindingFactory
        where TTarget : class
    {
        private readonly Func<TTarget, IMvxTargetBinding> _targetBindingCreator;
        private readonly string _targetFakePropertyName;

        public MvxCustomBindingFactory(string targetFakePropertyName, Func<TTarget, IMvxTargetBinding> targetBindingCreator)
        {
            _targetFakePropertyName = targetFakePropertyName;
            _targetBindingCreator = targetBindingCreator;
        }

        #region IMvxPluginTargetBindingFactory Members

        public IEnumerable<MvxTypeAndNamePair> SupportedTypes
        {
            get { return new[] { new MvxTypeAndNamePair(typeof(TTarget), _targetFakePropertyName) }; }
        }

        public IMvxTargetBinding CreateBinding(object target, MvxBindingDescription description)
        {
            var castTarget = target as TTarget;
            if (castTarget == null)
            {
                MvxBindingTrace.Trace(MvxTraceLevel.Error, "Passed an invalid target for MvxCustomBindingFactory");
                return null;
            }

            return _targetBindingCreator(castTarget);
        }

        #endregion
    }
}