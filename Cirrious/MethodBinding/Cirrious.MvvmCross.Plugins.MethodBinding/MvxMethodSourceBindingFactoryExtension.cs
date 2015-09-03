// MvxMethodSourceBindingFactoryExtension.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.Binding;
using Cirrious.MvvmCross.Binding.Bindings.Source;
using Cirrious.MvvmCross.Binding.Bindings.Source.Construction;
using Cirrious.MvvmCross.Binding.Parse.PropertyPath.PropertyTokens;

namespace Cirrious.MvvmCross.Plugins.MethodBinding
{
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
                MvxBindingTrace.Warning("Problem binding to Method {0} - too many non-optional parameters");
            }

            result = new MvxMethodSourceBinding(source, methodInfo);
            return true;
        }

        protected MethodInfo FindMethodInfo(object source, string name)
        {
            var methodInfo = source.GetType()
                                         .GetMethod(name);
            return methodInfo;
        }
    }
}