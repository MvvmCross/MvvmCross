// MvxTargetBindingFactoryRegistry.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Collections.Generic;
using Cirrious.CrossCore.Platform;

namespace Cirrious.MvvmCross.Binding.Bindings.Target.Construction
{
    public class MvxTargetBindingFactoryRegistry : IMvxTargetBindingFactoryRegistry
    {
        private readonly Dictionary<string, IMvxPluginTargetBindingFactory> _lookups =
            new Dictionary<string, IMvxPluginTargetBindingFactory>();

        public IMvxTargetBinding CreateBinding(object target, string targetName)
        {
            IMvxTargetBinding binding;
            if (TryCreateSpecificFactoryBinding(target, targetName, out binding))
                return binding;

            if (TryCreateReflectionBasedBinding(target, targetName, out binding))
                return binding;

            return null;
        }

        protected virtual bool TryCreateReflectionBasedBinding(object target, string targetName,
                                                               out IMvxTargetBinding binding)
        {
            if (string.IsNullOrEmpty(targetName))
            {
                MvxBindingTrace.Trace(MvxTraceLevel.Error,
                                      "Empty binding target passed to MvxTargetBindingFactoryRegistry");
                binding = null;
                return false;
            }

            var targetPropertyInfo = target.GetType().GetProperty(targetName);
            if (targetPropertyInfo != null
                && targetPropertyInfo.CanWrite)
            {
                binding = new MvxWithEventPropertyInfoTargetBinding(target, targetPropertyInfo);
                return true;
            }

            var targetEventInfo = target.GetType().GetEvent(targetName);
            if (targetEventInfo != null)
            {
                // we only handle EventHandler's here
                // other event types will need to be handled by custom bindings
                if (targetEventInfo.EventHandlerType == typeof (EventHandler))
                {
                    binding = new MvxEventHandlerEventInfoTargetBinding(target, targetEventInfo);
                    return true;
                }
            }

            binding = null;
            return false;
        }

        protected virtual bool TryCreateSpecificFactoryBinding(object target, string targetName,
                                                               out IMvxTargetBinding binding)
        {
            var factory = FindSpecificFactory(target.GetType(), targetName);
            if (factory != null)
            {
                binding = factory.CreateBinding(target, targetName);
                return true;
            }

            binding = null;
            return false;
        }

        public void RegisterFactory(IMvxPluginTargetBindingFactory factory)
        {
            foreach (var supported in factory.SupportedTypes)
            {
                var key = GenerateKey(supported.Type, supported.Name);
                _lookups[key] = factory;
            }
        }

        private string GenerateKey(Type type, string name)
        {
            return string.Format("{0}:{1}", type.FullName, name);
        }

        private IMvxPluginTargetBindingFactory FindSpecificFactory(Type type, string name)
        {
            IMvxPluginTargetBindingFactory factory;
            var key = GenerateKey(type, name);
            if (_lookups.TryGetValue(key, out factory))
            {
                return factory;
            }
            var baseType = type.BaseType;
            if (baseType != null)
                return FindSpecificFactory(baseType, name);
            return null;
        }
    }
}