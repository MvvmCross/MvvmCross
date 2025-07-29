// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Logging;
using MvvmCross.Binding.Parse.PropertyPath;
using MvvmCross.Binding.Parse.PropertyPath.PropertyTokens;
using MvvmCross.Exceptions;

namespace MvvmCross.Binding.Bindings.Source.Construction
{
    public class MvxSourceBindingFactory
        : IMvxSourceBindingFactory
        , IMvxSourceBindingFactoryExtensionHost
    {
        private IMvxSourcePropertyPathParser _propertyPathParser;

        protected IMvxSourcePropertyPathParser SourcePropertyPathParser => _propertyPathParser ??= Mvx.IoCProvider.Resolve<IMvxSourcePropertyPathParser>();

        private readonly List<IMvxSourceBindingFactoryExtension> _extensions = [];

        [RequiresUnreferencedCode("This method uses reflection to create bindings, which may not be preserved in trimming scenarios")]
        protected bool TryCreateBindingFromExtensions(
            object source, IMvxPropertyToken propertyToken,
            List<IMvxPropertyToken> remainingTokens, out IMvxSourceBinding result)
        {
            foreach (var extension in _extensions)
            {
                if (extension.TryCreateBinding(source, propertyToken, remainingTokens, out result))
                {
                    return true;
                }
            }

            result = null;
            return false;
        }

        [RequiresUnreferencedCode("This method uses reflection to create bindings, which may not be preserved in trimming scenarios")]
        public IMvxSourceBinding CreateBinding(object source, string combinedPropertyName)
        {
            var tokens = SourcePropertyPathParser.Parse(combinedPropertyName);
            return CreateBinding(source, tokens);
        }

        [RequiresUnreferencedCode("This method uses reflection to create bindings, which may not be preserved in trimming scenarios")]
        public IMvxSourceBinding CreateBinding(object source, IList<IMvxPropertyToken> tokens)
        {
            if (tokens == null || tokens.Count == 0)
            {
                throw new MvxException("empty token list passed to CreateBinding");
            }

            var currentToken = tokens[0];
            var remainingTokens = tokens.Skip(1).ToList();
            IMvxSourceBinding extensionResult;
            if (TryCreateBindingFromExtensions(source, currentToken, remainingTokens, out extensionResult))
            {
                return extensionResult;
            }

            if (source != null)
            {
                MvxBindingLog.Instance?.LogWarning(
                    "Unable to bind: source property source not found @{CurrentToken} on {SourceTypeName}",
                    currentToken,
                    source.GetType().Name);
            }

            return new MvxMissingSourceBinding(source);
        }

        public IList<IMvxSourceBindingFactoryExtension> Extensions => _extensions;
    }
}
