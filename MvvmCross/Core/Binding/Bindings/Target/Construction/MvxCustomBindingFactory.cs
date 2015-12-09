// MvxCustomBindingFactory.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Binding.Bindings.Target.Construction
{
    using System;
    using System.Collections.Generic;

    using MvvmCross.Platform.Platform;

    public class MvxCustomBindingFactory<TTarget>
        : IMvxPluginTargetBindingFactory
        where TTarget : class
    {
        private readonly Func<TTarget, IMvxTargetBinding> _targetBindingCreator;
        private readonly string _targetFakePropertyName;

        public MvxCustomBindingFactory(string targetFakePropertyName,
                                       Func<TTarget, IMvxTargetBinding> targetBindingCreator)
        {
            this._targetFakePropertyName = targetFakePropertyName;
            this._targetBindingCreator = targetBindingCreator;
        }

        #region IMvxPluginTargetBindingFactory Members

        public IEnumerable<MvxTypeAndNamePair> SupportedTypes => new[] { new MvxTypeAndNamePair(typeof(TTarget), this._targetFakePropertyName) };

        public IMvxTargetBinding CreateBinding(object target, string targetName)
        {
            var castTarget = target as TTarget;
            if (castTarget == null)
            {
                MvxBindingTrace.Trace(MvxTraceLevel.Error, "Passed an invalid target for MvxCustomBindingFactory");
                return null;
            }

            return this._targetBindingCreator(castTarget);
        }

        #endregion IMvxPluginTargetBindingFactory Members
    }
}