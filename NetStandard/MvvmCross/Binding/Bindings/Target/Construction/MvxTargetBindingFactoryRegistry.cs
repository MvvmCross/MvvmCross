﻿// MvxTargetBindingFactoryRegistry.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Collections.Generic;
using System.Reflection;
using MvvmCross.Platform.Logging;

namespace MvvmCross.Binding.Bindings.Target.Construction
{
    public class MvxTargetBindingFactoryRegistry : IMvxTargetBindingFactoryRegistry
    {
        private readonly Dictionary<int, IMvxPluginTargetBindingFactory> _lookups =
            new Dictionary<int, IMvxPluginTargetBindingFactory>();

        public virtual IMvxTargetBinding CreateBinding(object target, string targetName)
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
                MvxLog.Instance.Trace("Empty binding target passed to MvxTargetBindingFactoryRegistry");
                binding = null;
                return false;
            }

            if (target == null)
            {
                // null passed in so return false - fixes #584
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
                if (targetEventInfo.EventHandlerType == typeof(EventHandler))
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
            if (target == null)
            {
                // null passed in so return false - fixes #584
                binding = null;
                return false;
            }

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

        private static int GenerateKey(Type type, string name)
        {
			return (type.GetHashCode () * 9) ^ name.GetHashCode ();
        }

        private IMvxPluginTargetBindingFactory FindSpecificFactory(Type type, string name)
        {
            IMvxPluginTargetBindingFactory factory;
            var key = GenerateKey(type, name);
            if (_lookups.TryGetValue(key, out factory))
            {
                return factory;
            }
            var baseType = type.GetTypeInfo().BaseType;
            if (baseType != null)
                factory = FindSpecificFactory(baseType, name);
            if (factory != null) return factory;
            var implementedInterfaces = type.GetTypeInfo().ImplementedInterfaces;
            foreach (var implementedInterface in implementedInterfaces)
            {
                factory = FindSpecificFactory(implementedInterface, name);
                if (factory != null) return factory;
            }
            return null;
        }
    }
}
