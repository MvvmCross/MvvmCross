// MvxSimplePropertyInfoTargetBindingFactory.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Collections.Generic;
using System.Reflection;
using Cirrious.CrossCore.Platform;

namespace Cirrious.MvvmCross.Binding.Bindings.Target.Construction
{
    public class MvxSimplePropertyInfoTargetBindingFactory
        : IMvxPluginTargetBindingFactory
    {
        private readonly Type _bindingType;
        private readonly MvxPropertyInfoTargetBindingFactory _innerFactory;

        public MvxSimplePropertyInfoTargetBindingFactory(Type bindingType, Type targetType, string targetName)
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

        #endregion

        private IMvxTargetBinding CreateTargetBinding(object target, PropertyInfo targetPropertyInfo)
        {
            var targetBindingCandidate = Activator.CreateInstance(_bindingType, new[] {target, targetPropertyInfo});
            var targetBinding = targetBindingCandidate as IMvxTargetBinding;
            if (targetBinding == null)
            {
                MvxBindingTrace.Trace(MvxTraceLevel.Warning,
                                      "The TargetBinding created did not support IMvxTargetBinding");
                var disposable = targetBindingCandidate as IDisposable;
                disposable?.Dispose();
            }
            return targetBinding;
        }
    }
}