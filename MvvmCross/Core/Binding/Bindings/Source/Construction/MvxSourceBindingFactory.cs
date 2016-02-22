// MvxSourceBindingFactory.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;

namespace MvvmCross.Binding.Bindings.Source.Construction
{
    using System.Collections.Generic;
    using System.Linq;

    using MvvmCross.Binding.Parse.PropertyPath;
    using MvvmCross.Binding.Parse.PropertyPath.PropertyTokens;
    using MvvmCross.Platform;
    using MvvmCross.Platform.Exceptions;
    using MvvmCross.Platform.Platform;

    public class MvxSourceBindingFactory
        : IMvxSourceBindingFactory
        , IMvxSourceBindingFactoryExtensionHost
    {
        private IMvxSourcePropertyPathParser _propertyPathParser;

        protected IMvxSourcePropertyPathParser SourcePropertyPathParser => this._propertyPathParser ?? (this._propertyPathParser = Mvx.Resolve<IMvxSourcePropertyPathParser>());

        private readonly List<IMvxSourceBindingFactoryExtension> _extensions = new List<IMvxSourceBindingFactoryExtension>();

        protected bool TryCreateBindingFromExtensions(object source, MvxPropertyToken propertyToken, List<MvxPropertyToken> remainingTokens, out IMvxSourceBinding result)
        {
            var enhancedSource = source as IMvxEnhancedDataContext;
            if (enhancedSource != null)
            {
                // it's a bit nasty doing the parent detection and handling here... but it works
                if (propertyToken is MvxParentPropertyToken)
                {
                    var parentSource = enhancedSource.Parent;
                    var nextPropertyToken = remainingTokens.FirstOrDefault() ?? new MvxEmptyPropertyToken();
                    var fewerTokens = remainingTokens.Skip(1).ToList();

                    // recurse... this allows us to handle $parent.$parent.$parent.path expressions
                    return TryCreateBindingFromExtensions(parentSource, nextPropertyToken, fewerTokens, out result);
                }
            }

            // beyond this point parent source isn't needed.... hackeroo
            var realSource = enhancedSource == null ? source : enhancedSource.Core;

            foreach (var extension in this._extensions)
            {
                if (extension.TryCreateBinding(realSource, propertyToken, remainingTokens, out result))
                {
                    return true;
                }
            }

            result = null;
            return false;
        }

        public IMvxSourceBinding CreateBinding(object source, string combinedPropertyName)
        {
            var tokens = this.SourcePropertyPathParser.Parse(combinedPropertyName);
            return this.CreateBinding(source, tokens);
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
            if (this.TryCreateBindingFromExtensions(source, currentToken, remainingTokens, out extensionResult))
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

        public IList<IMvxSourceBindingFactoryExtension> Extensions => this._extensions;
    }
}