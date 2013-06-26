// MvxPathSourceBindingFactory.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System.Collections.Generic;
using System.Linq;
using Cirrious.CrossCore;
using Cirrious.CrossCore.Exceptions;
using Cirrious.MvvmCross.Binding.Bindings.PathSource.Chained;
using Cirrious.MvvmCross.Binding.Bindings.PathSource.Leaf;
using Cirrious.MvvmCross.Binding.Parse.PropertyPath;
using Cirrious.MvvmCross.Binding.Parse.PropertyPath.PropertyTokens;

namespace Cirrious.MvvmCross.Binding.Bindings.PathSource.Construction
{
    public class MvxPathSourceBindingFactory
        : IMvxPathSourceBindingFactory
    {
        private IMvxSourcePropertyPathParser _propertyPathParser;

        private IMvxSourcePropertyPathParser SourcePropertyPathParser
        {
            get
            {
                if (_propertyPathParser == null)
                {
                    _propertyPathParser = Mvx.Resolve<IMvxSourcePropertyPathParser>();
                }
                return _propertyPathParser;
            }
        }

        #region IMvxPathSourceBindingFactory Members

        public IMvxPathSourceBinding CreateBinding(object source, string combinedPropertyName)
        {
            var tokens = SourcePropertyPathParser.Parse(combinedPropertyName);
            return CreateBinding(source, tokens);
        }

        public IMvxPathSourceBinding CreateBinding(object source, IList<MvxPropertyToken> tokens)
        {
            if (tokens == null || tokens.Count == 0)
            {
                throw new MvxException("empty token list passed to CreateBinding");
            }

            var currentToken = tokens[0];
            if (tokens.Count == 1)
            {
                return CreateLeafBinding(source, currentToken);
            }
            else
            {
                var remainingTokens = tokens.Skip(1).ToList();
                return CreateChainedBinding(source, currentToken, remainingTokens);
            }
        }

        private static MvxPathChainedSourceBinding CreateChainedBinding(object source, MvxPropertyToken propertyToken,
                                                                        List<MvxPropertyToken> remainingTokens)
        {
            if (propertyToken is MvxIndexerPropertyToken)
            {
                return new MvxPathIndexerChainedSourceBinding(source, (MvxIndexerPropertyToken) propertyToken,
                                                              remainingTokens);
            }
            else if (propertyToken is MvxPropertyNamePropertyToken)
            {
                return new MvxPathSimpleChainedSourceBinding(source, (MvxPropertyNamePropertyToken) propertyToken,
                                                             remainingTokens);
            }

            throw new MvxException("Unexpected property chaining - seen token type {0}",
                                   propertyToken.GetType().FullName);
        }

        private static IMvxPathSourceBinding CreateLeafBinding(object source, MvxPropertyToken propertyToken)
        {
            if (propertyToken is MvxIndexerPropertyToken)
            {
                return new MvxPathIndexerLeafPropertyInfoSourceBinding(source, (MvxIndexerPropertyToken) propertyToken);
            }
            else if (propertyToken is MvxPropertyNamePropertyToken)
            {
                return new MvxPathSimpleLeafPropertyInfoSourceBinding(source,
                                                                      (MvxPropertyNamePropertyToken) propertyToken);
            }
            else if (propertyToken is MvxEmptyPropertyToken)
            {
                return new MvxPathDirectToSourceBinding(source);
            }

            throw new MvxException("Unexpected property source - seen token type {0}", propertyToken.GetType().FullName);
        }

        #endregion
    }
}