using System;
using System.Collections.Generic;
using System.Reflection;
using Cirrious.MvvmCross.Binding.Interfaces;
using Cirrious.MvvmCross.Binding.Interfaces.Bindings.Target;
using Cirrious.MvvmCross.Binding.Interfaces.Bindings.Target.Construction;

namespace Cirrious.MvvmCross.Binding.Bindings.Target.Construction
{
    public class MvxSimplePropertyInfoTargetBindingFactory 
        : IMvxPluginTargetBindingFactory
    {
        private readonly MvxPropertyInfoTargetBindingFactory _innerFactory;
        private readonly Type _bindingType;

        public MvxSimplePropertyInfoTargetBindingFactory(Type bindingType, Type targetType, string targetName)
        {
            _bindingType = bindingType;
            _innerFactory = new MvxPropertyInfoTargetBindingFactory(targetType, targetName, CreateTargetBinding);
        }

        private IMvxTargetBinding CreateTargetBinding(object target, PropertyInfo targetPropertyInfo) 
        {
            var targetBindingCandidate = Activator.CreateInstance(_bindingType, new object[] {target, targetPropertyInfo });
            var targetBinding = targetBindingCandidate as IMvxTargetBinding;
            if (targetBinding == null)
            {
                var disposable = targetBindingCandidate as IDisposable;
                if (disposable != null)
                    disposable.Dispose();
            }
            return targetBinding;
        }

        public IEnumerable<MvxTypeAndNamePair> SupportedTypes
        {
            get { return _innerFactory.SupportedTypes; }
        }

        public IMvxTargetBinding CreateBinding(object target, MvxBindingDescription description)
        {
            return _innerFactory.CreateBinding(target, description);
        }
    }
}