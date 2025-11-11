// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Microsoft.Extensions.Logging;

namespace MvvmCross.Binding.Bindings.Target.Construction
{
    public class MvxSimplePropertyInfoTargetBindingFactory
        : IMvxPluginTargetBindingFactory
    {
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)]
        private readonly Type _bindingType;
        private readonly MvxPropertyInfoTargetBindingFactory _innerFactory;

        [RequiresUnreferencedCode("This constructor creates bindings using reflection which may not be preserved by trimming")]
        public MvxSimplePropertyInfoTargetBindingFactory(
            [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type bindingType,
            [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicProperties)] Type targetType,
            string targetName)
        {
            _bindingType = bindingType;
            _innerFactory = new MvxPropertyInfoTargetBindingFactory(targetType, targetName, CreateTargetBinding);
        }

        #region IMvxPluginTargetBindingFactory Members

        public IEnumerable<MvxTypeAndNamePair> SupportedTypes => _innerFactory.SupportedTypes;

        [RequiresUnreferencedCode("This method creates target bindings using reflection-based binding creation which may not be preserved by trimming")]
        public IMvxTargetBinding CreateBinding(object target, string targetName)
        {
            return _innerFactory.CreateBinding(target, targetName);
        }

        #endregion IMvxPluginTargetBindingFactory Members

        [RequiresUnreferencedCode("This method uses Activator.CreateInstance to create binding instances, which may not be preserved by trimming")]
        private IMvxTargetBinding CreateTargetBinding(object target, PropertyInfo targetPropertyInfo)
        {
            var targetBindingCandidate = Activator.CreateInstance(_bindingType, target, targetPropertyInfo);
            var targetBinding = targetBindingCandidate as IMvxTargetBinding;
            if (targetBinding == null)
            {
                MvxBindingLog.Instance?.LogWarning("The TargetBinding created did not support IMvxTargetBinding");
                var disposable = targetBindingCandidate as IDisposable;
                disposable?.Dispose();
            }
            return targetBinding;
        }
    }
}
