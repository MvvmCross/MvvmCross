#region Copyright
// <copyright file="MvxTargetBindingFactoryRegistry.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com
#endregion

using System;
using System.Collections.Generic;
using Cirrious.MvvmCross.Binding.Interfaces;
using Cirrious.MvvmCross.Binding.Interfaces.Bindings.Target;
using Cirrious.MvvmCross.Binding.Interfaces.Bindings.Target.Construction;

namespace Cirrious.MvvmCross.Binding.Bindings.Target.Construction
{
    public class MvxTargetBindingFactoryRegistry : IMvxTargetBindingFactoryRegistry
    {
        private readonly Dictionary<string, IMvxPluginTargetBindingFactory> _lookups =
            new Dictionary<string, IMvxPluginTargetBindingFactory>();

        #region IMvxTargetBindingFactoryRegistry Members

        public IMvxTargetBinding CreateBinding(object target, MvxBindingDescription description)
        {
            var factory = FindSpecificFactory(target.GetType(), description.TargetName);
            if (factory != null)
                return factory.CreateBinding(target, description);

            var targetPropertyInfo = target.GetType().GetProperty(description.TargetName);
            if (targetPropertyInfo != null)
            {
                return new MvxPropertyInfoTargetBinding(target, targetPropertyInfo);
            }

            var targetEventInfo = target.GetType().GetEvent(description.TargetName);
            if (targetEventInfo != null)
            {
#warning Handle other event types here - possibly another lookup table so people can register their own?
                if (targetEventInfo.EventHandlerType == typeof(EventHandler))
                    return new MvxEventHandlerEventInfoTargetBinding(target, targetEventInfo);
            }

            return null;
        }

        public void RegisterFactory(IMvxPluginTargetBindingFactory factory)
        {
            foreach (var supported in factory.SupportedTypes)
            {
                var key = GenerateKey(supported.Type, supported.Name);
                _lookups[key] = factory;
            }
        }

        #endregion

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