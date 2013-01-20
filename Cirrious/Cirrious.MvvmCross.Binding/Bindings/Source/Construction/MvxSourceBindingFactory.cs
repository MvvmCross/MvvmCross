// MvxSourceBindingFactory.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System.Collections.Generic;
using System.Linq;
using Cirrious.MvvmCross.Binding.Bindings.Source.Chained;
using Cirrious.MvvmCross.Binding.Bindings.Source.Leaf;
using Cirrious.MvvmCross.Binding.Interfaces.Bindings.Source;
using Cirrious.MvvmCross.Binding.Interfaces.Bindings.Source.Construction;
using Cirrious.MvvmCross.Binding.Interfaces.Parse;
using Cirrious.MvvmCross.Binding.Parse.PropertyPath.PropertyTokens;
using Cirrious.MvvmCross.Exceptions;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;

namespace Cirrious.MvvmCross.Binding.Bindings.Source.Construction
{
    public class MvxSourceBindingFactory
        : IMvxSourceBindingFactory
          , IMvxServiceConsumer
    {
        private static readonly char[] FieldSeparator = new[] {'.', '['};

        private IMvxSourcePropertyPathParser _propertyPathParser;

        private IMvxSourcePropertyPathParser SourcePropertyPathParser
        {
            get
            {
                if (_propertyPathParser == null)
                {
                    _propertyPathParser = this.GetService<IMvxSourcePropertyPathParser>();
                }
                return _propertyPathParser;
            }
        }

        #region IMvxSourceBindingFactory Members

        public IMvxSourceBinding CreateBinding(object source, string combinedPropertyName)
        {
            if (combinedPropertyName == null)
                combinedPropertyName = "";

            var tokens = SourcePropertyPathParser.Parse(combinedPropertyName);
            var queue = new List<MvxBasePropertyToken>(tokens);
            return CreateBinding(source, queue);
        }

#warning CreateBinding should probably take an IList to make the code run quicker!
        public IMvxSourceBinding CreateBinding(object source, IList<MvxBasePropertyToken> tokens)
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

        private static MvxChainedSourceBinding CreateChainedBinding(object source, MvxBasePropertyToken propertyToken,
                                                                    List<MvxBasePropertyToken> remainingTokens)
        {
            if (propertyToken is MvxIndexerPropertyToken)
            {
                return new MvxIndexerChainedSourceBinding(source, (MvxIndexerPropertyToken) propertyToken,
                                                          remainingTokens);
            }
            else if (propertyToken is MvxPropertyNamePropertyToken)
            {
                return new MvxSimpleChainedSourceBinding(source, (MvxPropertyNamePropertyToken) propertyToken,
                                                         remainingTokens);
            }

            throw new MvxException("Unexpected property chaining - seen token type {0}",
                                   propertyToken.GetType().FullName);
        }

        private static IMvxSourceBinding CreateLeafBinding(object source, MvxBasePropertyToken propertyToken)
        {
            if (propertyToken is MvxIndexerPropertyToken)
            {
                return new MvxIndexerLeafPropertyInfoSourceBinding(source, (MvxIndexerPropertyToken) propertyToken);
            }
            else if (propertyToken is MvxPropertyNamePropertyToken)
            {
                return new MvxSimpleLeafPropertyInfoSourceBinding(source, (MvxPropertyNamePropertyToken) propertyToken);
            }
            else if (propertyToken is MvxEmptyPropertyToken)
            {
                return new MvxDirectToSourceBinding(source);
            }

            throw new MvxException("Unexpected property source - seen token type {0}", propertyToken.GetType().FullName);
        }

        #endregion
    }
}