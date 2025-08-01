// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Microsoft.Extensions.Logging;

namespace MvvmCross.Binding.Bindings.Target.Construction
{
    public class MvxPropertyInfoTargetBindingFactory
        : IMvxPluginTargetBindingFactory
    {
        private readonly Func<object, PropertyInfo, IMvxTargetBinding> _bindingCreator;
        private readonly string _targetName;

        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicProperties)]
        private readonly Type _targetType;

        public MvxPropertyInfoTargetBindingFactory(
            [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicProperties)] Type targetType,
            string targetName,
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
                    MvxBindingLog.Instance?.LogError(
                        exception,
                        "Problem creating target binding for {TargetName} - exception {ExceptionMessage}", _targetType.Name,
                        exception.ToString());
                }
            }

            return null;
        }

        #endregion IMvxPluginTargetBindingFactory Members
    }
}
