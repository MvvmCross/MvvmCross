using System;
using System.Collections.Generic;
using Cirrious.MvvmCross.Binding.Interfaces;
using Cirrious.MvvmCross.Binding.Interfaces.Bindings.Target;
using Cirrious.MvvmCross.Binding.Interfaces.Bindings.Target.Construction;

namespace Cirrious.MvvmCross.Binding.Bindings.Target.Construction
{
    public class MvxCustomBindingFactory<TTarget>
        : IMvxPluginTargetBindingFactory
        where TTarget : class
    {
        private readonly string _targetFakePropertyName;
        private readonly Func<TTarget, IMvxTargetBinding> _targetBindingCreator;

        public MvxCustomBindingFactory(string targetFakePropertyName, Func<TTarget, IMvxTargetBinding> targetBindingCreator)
        {
            _targetFakePropertyName = targetFakePropertyName;
            _targetBindingCreator = targetBindingCreator;
        }

        public IEnumerable<MvxTypeAndNamePair> SupportedTypes
        {
            get { return new[] { new MvxTypeAndNamePair(typeof(TTarget), _targetFakePropertyName) }; }
        }

        public IMvxTargetBinding CreateBinding(object target, MvxBindingDescription description)
        {
            var castTarget = target as TTarget;
            if (castTarget == null)
            {
                MvxBindingTrace.Trace(MvxBindingTraceLevel.Error, "Passed an invalid target for MvxCustomBindingFactory");
                return null;
            }

            return _targetBindingCreator(castTarget);
        }
    }
}