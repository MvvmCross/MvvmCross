// MvxPropertyInfoTargetBindingFactory.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Binding.Bindings.Target.Construction
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    using MvvmCross.Platform;
    using MvvmCross.Platform.Platform;

    public class MvxPropertyInfoTargetBindingFactory
        : IMvxPluginTargetBindingFactory
    {
        private readonly Func<object, PropertyInfo, IMvxTargetBinding> _bindingCreator;
        private readonly string _targetName;
        private readonly Type _targetType;

        public MvxPropertyInfoTargetBindingFactory(Type targetType, string targetName,
                                                   Func<object, PropertyInfo, IMvxTargetBinding> bindingCreator)
        {
            this._targetType = targetType;
            this._targetName = targetName;
            this._bindingCreator = bindingCreator;
        }

        protected Type TargetType => this._targetType;

        #region IMvxPluginTargetBindingFactory Members

        public IEnumerable<MvxTypeAndNamePair> SupportedTypes => new[] { new MvxTypeAndNamePair { Name = this._targetName, Type = this._targetType } };

        public IMvxTargetBinding CreateBinding(object target, string targetName)
        {
            var targetPropertyInfo = target.GetType().GetProperty(targetName);
            if (targetPropertyInfo != null)
            {
                try
                {
                    return this._bindingCreator(target, targetPropertyInfo);
                }
                catch (Exception exception)
                {
                    MvxBindingTrace.Trace(
                        MvxTraceLevel.Error,
                        "Problem creating target binding for {0} - exception {1}", this._targetType.Name,
                        exception.ToString());
                }
            }

            return null;
        }

        #endregion IMvxPluginTargetBindingFactory Members
    }
}