// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace MvvmCross.Binding.Bindings.Target.Construction
{
    public class MvxSimplePropertyInfoTargetBindingFactory
        : IMvxPluginTargetBindingFactory
    {
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)]
        private readonly Type _bindingType;
        private readonly MvxPropertyInfoTargetBindingFactory _innerFactory;

        public MvxSimplePropertyInfoTargetBindingFactory(
            [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type bindingType,
            Type targetType, string targetName)
        {
            _bindingType = bindingType;
            _innerFactory = new MvxPropertyInfoTargetBindingFactory(targetType, targetName, CreateTargetBinding);
        }

        #region IMvxPluginTargetBindingFactory Members

        public IEnumerable<MvxTypeAndNamePair> SupportedTypes => _innerFactory.SupportedTypes;

        public IMvxTargetBinding CreateBinding(object target, string targetName)
        {
            return _innerFactory.CreateBinding(target, targetName);
        }

        #endregion IMvxPluginTargetBindingFactory Members

        private IMvxTargetBinding CreateTargetBinding(object target, PropertyInfo targetPropertyInfo)
        {
            var targetBindingCandidate = Activator.CreateInstance(_bindingType, target, targetPropertyInfo);
            var targetBinding = targetBindingCandidate as IMvxTargetBinding;
            if (targetBinding == null)
            {
                MvxBindingLog.Warning("The TargetBinding created did not support IMvxTargetBinding");
                var disposable = targetBindingCandidate as IDisposable;
                disposable?.Dispose();
            }
            return targetBinding;
        }
    }
}
