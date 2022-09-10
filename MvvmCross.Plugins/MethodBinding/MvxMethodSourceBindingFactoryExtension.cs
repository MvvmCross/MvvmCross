// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.Logging;
using MvvmCross.Binding.Bindings.Source;
using MvvmCross.Binding.Bindings.Source.Construction;
using MvvmCross.Binding.Parse.PropertyPath.PropertyTokens;

namespace MvvmCross.Plugin.MethodBinding
{
    [Preserve(AllMembers = true)]
    public class MvxMethodSourceBindingFactoryExtension
        : IMvxSourceBindingFactoryExtension
    {
        public bool TryCreateBinding(object source, MvxPropertyToken currentToken, List<MvxPropertyToken> remainingTokens, out IMvxSourceBinding result)
        {
            if (source == null)
            {
                result = null;
                return false;
            }

            if (remainingTokens.Count > 0)
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

            var methodInfo = FindMethodInfo(source, propertyNameToken.PropertyName);

            if (methodInfo == null)
            {
                result = null;
                return false;
            }

            var parameters = methodInfo.GetParameters();
            if (parameters.Count(p => !p.IsOptional) > 1)
            {
                MvxPluginLog.Instance?.Log(LogLevel.Warning,
                    "Problem binding to Method {0} - too many non-optional parameters");
            }

            result = new MvxMethodSourceBinding(source, methodInfo);
            return true;
        }

        protected MethodInfo FindMethodInfo(object source, string name)
        {
            var methodInfo = source.GetType().GetMethod(name);
            return methodInfo;
        }
    }
}
