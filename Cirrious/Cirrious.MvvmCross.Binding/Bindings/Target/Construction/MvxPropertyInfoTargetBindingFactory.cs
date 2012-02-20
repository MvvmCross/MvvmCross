using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using Cirrious.MvvmCross.Binding.Interfaces;
using Cirrious.MvvmCross.Binding.Interfaces.Bindings.Target;
using Cirrious.MvvmCross.Binding.Interfaces.Bindings.Target.Construction;

namespace Cirrious.MvvmCross.Binding.Bindings.Target.Construction
{
    public class MvxPropertyInfoTargetBindingFactory
     : IMvxPluginTargetBindingFactory
    {
        private readonly Type _targetType;
        private readonly string _targetName;
        private readonly Func<object, PropertyInfo, IMvxTargetBinding> _bindingCreator;

        protected Type TargetType { get { return _targetType; } }

        public MvxPropertyInfoTargetBindingFactory(Type targetType, string targetName, Func<object, PropertyInfo, IMvxTargetBinding> bindingCreator)
        {
            _targetType = targetType;
            _targetName = targetName;
            _bindingCreator = bindingCreator;
        }

        public IEnumerable<MvxTypeAndNamePair> SupportedTypes
        {
            get { return new[] {new MvxTypeAndNamePair() {Name = _targetName, Type = _targetType}}; }
        }

        public IMvxTargetBinding CreateBinding(object target, MvxBindingDescription description)
        {
            var targetPropertyInfo = target.GetType().GetProperty(description.TargetName);
            if (targetPropertyInfo != null)
            {
                try
                {
                    return _bindingCreator(target, targetPropertyInfo);
                }
                catch (ThreadAbortException)
                {
                    throw;
                }
                catch (Exception exception)
                {
                    MvxBindingTrace.Trace(
                                            MvxBindingTraceLevel.Error,
"Problem creating target binding for {0} - exception {1}", _targetType.Name, exception.ToString());
                }
            }

            return null;
        }
    }
}