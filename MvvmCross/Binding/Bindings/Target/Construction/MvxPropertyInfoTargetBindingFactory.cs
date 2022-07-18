// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Reflection;

namespace MvvmCross.Binding.Bindings.Target.Construction
{
    public class MvxPropertyInfoTargetBindingFactory
        : IMvxPluginTargetBindingFactory
    {
        private readonly Func<object, PropertyInfo, IMvxTargetBinding> _bindingCreator;
        private readonly string _targetName;
        private readonly Type _targetType;

        public MvxPropertyInfoTargetBindingFactory(Type targetType, string targetName,
                                                   Func<object, PropertyInfo, IMvxTargetBinding> bindingCreator)
        {
            _targetType = targetType;
            _targetName = targetName;
            _bindingCreator = bindingCreator;
        }

        protected Type TargetType => _targetType;

        #region IMvxPluginTargetBindingFactory Members

        public IEnumerable<MvxTypeAndNamePair> SupportedTypes => new[]
        {
            new MvxTypeAndNamePair { Name = _targetName, Type = _targetType }
        };

        public IMvxTargetBinding CreateBinding(object target, string targetName)
        {
            var targetPropertyInfo = target.GetType().GetProperty(targetName);
            if (targetPropertyInfo != null)
            {
                try
                {
                    return _bindingCreator(target, targetPropertyInfo);
                }
                catch (Exception exception)
                {
                    MvxBindingLog.Error(
                        "Problem creating target binding for {0} - exception {1}", _targetType.Name,
                        exception.ToString());
                }
            }

            return null;
        }

        #endregion IMvxPluginTargetBindingFactory Members
    }
}
