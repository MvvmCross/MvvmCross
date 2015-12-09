// MvxSourceBindingFactory.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.CrossCore;
using Cirrious.CrossCore.Exceptions;
using Cirrious.CrossCore.Platform;
using Cirrious.MvvmCross.Binding.Parse.PropertyPath;
using Cirrious.MvvmCross.Binding.Parse.PropertyPath.PropertyTokens;
using System.Collections.Generic;
using System.Linq;

namespace Cirrious.MvvmCross.Binding.Bindings.Source.Construction
{
    public class MvxSourceBindingFactory
        : IMvxSourceBindingFactory
        , IMvxSourceBindingFactoryExtensionHost
    {
        private IMvxSourcePropertyPathParser _propertyPathParser;

        protected IMvxSourcePropertyPathParser SourcePropertyPathParser => _propertyPathParser ?? (_propertyPathParser = Mvx.Resolve<IMvxSourcePropertyPathParser>());

        private readonly List<IMvxSourceBindingFactoryExtension> _extensions = new List<IMvxSourceBindingFactoryExtension>();

        protected bool TryCreateBindingFromExtensions(object source, MvxPropertyToken propertyToken,
                                            List<MvxPropertyToken> remainingTokens, out IMvxSourceBinding result)
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

        public IMvxSourceBinding CreateBinding(object source, string combinedPropertyName)
        {
            var tokens = SourcePropertyPathParser.Parse(combinedPropertyName);
            return CreateBinding(source, tokens);
        }

        public IMvxSourceBinding CreateBinding(object source, IList<MvxPropertyToken> tokens)
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
                MvxBindingTrace.Trace(
                    MvxTraceLevel.Warning,
                    "Unable to bind: source property source not found {0} on {1}"
                    , currentToken
                    , source.GetType().Name);
            }

            return new MvxMissingSourceBinding(source);
        }

        public IList<IMvxSourceBindingFactoryExtension> Extensions => _extensions;
    }
}