// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using MvvmCross.Base;
using MvvmCross.Exceptions;

namespace MvvmCross.Core.Parse.StringDictionary
{
#nullable enable
    public class MvxStringDictionaryParser
        : MvxParser, IMvxStringDictionaryParser
    {
        protected Dictionary<string, string?>? CurrentEntries { get; private set; }

        public IDictionary<string, string?> Parse(string textToParse)
        {
            Reset(textToParse);

            while (!IsComplete)
            {
                ParseNextKeyValuePair();
                SkipWhitespaceAndCharacters(';');
            }

            return CurrentEntries!;
        }

        protected override void Reset(string textToParse)
        {
            CurrentEntries = new Dictionary<string, string?>();
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
                throw new MvxException($"Unexpected object in key for keyvalue pair {key?.GetType().Name} at position {CurrentIndex}");
            }

            SkipWhitespace();

            if (CurrentChar != '=')
            {
                throw new MvxException($"Unexpected character in keyvalue pair {CurrentChar} at position {CurrentIndex}");
            }

            MoveNext();
            SkipWhitespace();

            var value = ReadValue();
            if (value == null)
            {
                CurrentEntries![(string)key] = null;
            }
            else if (value is string stringValue)
            {
                CurrentEntries![(string)key] = stringValue;
            }
            else
            {
                throw new MvxException($"Unexpected object in value for keyvalue pair {value?.GetType().Name} for key {key} at position {CurrentIndex}");
            }
        }
    }
#nullable restore
}
