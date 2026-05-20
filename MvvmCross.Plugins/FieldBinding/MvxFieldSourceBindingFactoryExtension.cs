// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.Logging;
using MvvmCross.Binding;
using MvvmCross.Binding.Bindings.Source;
using MvvmCross.Binding.Bindings.Source.Construction;
using MvvmCross.Binding.Parse.PropertyPath.PropertyTokens;
using MvvmCross.Logging;

namespace MvvmCross.Plugin.FieldBinding
{
    [RequiresUnreferencedCode("This class uses reflection to bind to fields which may not be preserved by trimming.")]
    public class MvxFieldSourceBindingFactoryExtension
        : IMvxSourceBindingFactoryExtension
    {
        [RequiresUnreferencedCode("This method uses reflection to bind to fields which may not be preserved by trimming.")]
        public bool TryCreateBinding(object? source, IMvxPropertyToken currentToken,
                                     List<IMvxPropertyToken> remainingTokens, out IMvxSourceBinding? result)
        {
            if (source == null)
            {
                result = null;
                return false;
            }

            var propertyNameToken = currentToken as MvxPropertyNamePropertyToken;
            if (propertyNameToken == null)
            {
                result = null;
                return false;
            }

            var fieldInfo = FindFieldInfo(source, propertyNameToken.PropertyName);

            if (fieldInfo == null)
            {
                result = null;
                return false;
            }

            if (typeof(INotifyChange).IsAssignableFrom(fieldInfo.FieldType))
            {
                return TryCreateNotifyChangeBinding(source, remainingTokens, out result, fieldInfo, propertyNameToken);
            }

            return TryCreateFieldInfoBinding(source, remainingTokens, out result, fieldInfo);
        }

        protected bool TryCreateFieldInfoBinding(object source, List<IMvxPropertyToken> remainingTokens,
                                                 out IMvxSourceBinding? result, FieldInfo fieldInfo)
        {
            if (remainingTokens.Any())
            {
                result = new MvxChainedFieldSourceBinding(source, fieldInfo, remainingTokens);
            }
            else
            {
                result = new MvxLeafFieldSourceBinding(source, fieldInfo);
            }
            return true;
        }

        protected bool TryCreateNotifyChangeBinding(object source, List<IMvxPropertyToken> remainingTokens,
                                                    out IMvxSourceBinding? result,
                                                    FieldInfo fieldInfo, MvxPropertyNamePropertyToken propertyNameToken)
        {
            var fieldValue = fieldInfo.GetValue(source) as INotifyChange;
            if (fieldValue == null)
            {
                MvxLogHost.GetLog("MvxBind")?.LogWarning("INotifyChange is null for {PropertyName}",
                    propertyNameToken.PropertyName);
                result = null;
                return false;
            }

            if (remainingTokens.Any())
            {
                result = new MvxChainedNotifyChangeFieldSourceBinding(source, fieldValue, remainingTokens);
            }
            else
            {
                result = new MvxLeafNotifyChangeFieldSourceBinding(source, fieldValue);
            }
            return true;
        }

        [UnconditionalSuppressMessage("Trimming", "IL2075", Justification = "Field binding plugin intentionally uses reflection to bind to fields at runtime.")]
        protected FieldInfo? FindFieldInfo(object source, string name)
        {
            var fieldInfo = source.GetType()
                                  .GetField(name,
                                            BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy);
            return fieldInfo;
        }
    }
}
