// MvxStringDictionaryParser.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.CrossCore.Exceptions;
using Cirrious.CrossCore.Parse;
using System.Collections.Generic;

namespace Cirrious.MvvmCross.Parse.StringDictionary
{
    public class MvxStringDictionaryParser
        : MvxParser, IMvxStringDictionaryParser
    {
        protected Dictionary<string, string> CurrentEntries { get; private set; }

        public IDictionary<string, string> Parse(string textToParse)
        {
            Reset(textToParse);

            while (!IsComplete)
            {
                ParseNextKeyValuePair();
                SkipWhitespaceAndCharacters(';');
            }

            return CurrentEntries;
        }

        protected override void Reset(string textToParse)
        {
            CurrentEntries = new Dictionary<string, string>();
            base.Reset(textToParse);
        }

        private void ParseNextKeyValuePair()
        {
            SkipWhitespace();

            if (IsComplete)
            {
                return;
            }

            var key = ReadValue();
            if (!(key is string))
            {
                throw new MvxException("Unexpected object in key for keyvalue pair {0} at position {1}",
                                       key.GetType().Name, CurrentIndex);
            }

            SkipWhitespace();

            if (CurrentChar != '=')
            {
                throw new MvxException("Unexpected character in keyvalue pair {0} at position {1}", CurrentChar,
                                       CurrentIndex);
            }

            MoveNext();
            SkipWhitespace();

            var value = ReadValue();
            if (value != null && !(value is string))
            {
                throw new MvxException("Unexpected object in value for keyvalue pair {0} for key {1} at position {2}",
                                       value.GetType().Name, key, CurrentIndex);
            }

            CurrentEntries[(string)key] = (string)value;
        }
    }
}