// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;

namespace MvvmCross.Binding.Bindings.Target.Construction
{
    public class MvxCustomBindingFactory<TTarget>
        : IMvxPluginTargetBindingFactory
        where TTarget : class
    {
        private readonly Func<TTarget, IMvxTargetBinding> _targetBindingCreator;
        private readonly string _targetFakePropertyName;

        public MvxCustomBindingFactory(string targetFakePropertyName,
                                       Func<TTarget, IMvxTargetBinding> targetBindingCreator)
        {
            _targetFakePropertyName = targetFakePropertyName;
            _targetBindingCreator = targetBindingCreator;
        }

        #region IMvxPluginTargetBindingFactory Members

        public IEnumerable<MvxTypeAndNamePair> SupportedTypes => new[]
        {
            new MvxTypeAndNamePair(typeof(TTarget), _targetFakePropertyName)
        };

        public IMvxTargetBinding CreateBinding(object target, string targetName)
        {
            var castTarget = target as TTarget;
            if (castTarget == null)
            {
                MvxBindingLog.Error("Passed an invalid target for MvxCustomBindingFactory");
                return null;
            }

            return _targetBindingCreator(castTarget);
        }

        #endregion IMvxPluginTargetBindingFactory Members
    }
}
