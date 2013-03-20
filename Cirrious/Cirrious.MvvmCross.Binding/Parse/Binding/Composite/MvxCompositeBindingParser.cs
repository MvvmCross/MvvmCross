// MvxCompositeBindingParser.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com


#warning Consider deleting this file - everyone moved to Swiss binding?
#if false

using System;
using Cirrious.MvvmCross.Binding.Interfaces.Parse;
using Cirrious.MvvmCross.Binding.Parse.Binding.Json;
using Cirrious.MvvmCross.Binding.Parse.Binding.Swiss;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.Platform.Diagnostics;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;
using Cirrious.MvvmCross.Plugins.Json;

namespace Cirrious.MvvmCross.Binding.Parse.Binding.Composite
{
    public class MvxCompositeBindingParser
        : IMvxBindingParser
    {
        private readonly IMvxBindingParser _jsonBindingParser;
        private readonly IMvxBindingParser _swissBindingParser;

        public MvxCompositeBindingParser()
        {
            _jsonBindingParser = new MvxJsonBindingParser();
            _swissBindingParser = new MvxSwissBindingParser();
        }

        private IMvxBindingParser ChooseParser(string text)
        {
			text = text.TrimStart();

            if (string.IsNullOrEmpty(text))
                return _swissBindingParser;

            if (text.StartsWith("{"))
            {
                return _jsonBindingParser;
            }
            else
            {
                return _swissBindingParser;
            }
        }

        public bool TryParseBindingDescription(string text, out MvxSerializableBindingDescription requestedDescription)
        {
            return ChooseParser(text).TryParseBindingDescription(text, out requestedDescription);
        }

        public bool TryParseBindingSpecification(string text, out MvxSerializableBindingSpecification requestedBindings)
        {
            return ChooseParser(text).TryParseBindingSpecification(text, out requestedBindings);
        }
    }
}
#endif