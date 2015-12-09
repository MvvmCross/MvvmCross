// MvxStringDictionaryParser.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Core.Parse.StringDictionary
{
    using System.Collections.Generic;

    using MvvmCross.Platform.Exceptions;
    using MvvmCross.Platform.Parse;

    public class MvxStringDictionaryParser
        : MvxParser, IMvxStringDictionaryParser
    {
        protected Dictionary<string, string> CurrentEntries { get; private set; }

        public IDictionary<string, string> Parse(string textToParse)
        {
            this.Reset(textToParse);

            while (!this.IsComplete)
            {
                this.ParseNextKeyValuePair();
                this.SkipWhitespaceAndCharacters(';');
            }

            return this.CurrentEntries;
        }

        protected override void Reset(string textToParse)
        {
            this.CurrentEntries = new Dictionary<string, string>();
            base.Reset(textToParse);
        }

        private void ParseNextKeyValuePair()
        {
            this.SkipWhitespace();

            if (this.IsComplete)
            {
                return;
            }

            var key = this.ReadValue();
            if (!(key is string))
            {
                throw new MvxException("Unexpected object in key for keyvalue pair {0} at position {1}",
                                       key.GetType().Name, this.CurrentIndex);
            }

            this.SkipWhitespace();

            if (this.CurrentChar != '=')
            {
                throw new MvxException("Unexpected character in keyvalue pair {0} at position {1}", this.CurrentChar,
                                       this.CurrentIndex);
            }

            this.MoveNext();
            this.SkipWhitespace();

            var value = this.ReadValue();
            if (value != null && !(value is string))
            {
                throw new MvxException("Unexpected object in value for keyvalue pair {0} for key {1} at position {2}",
                                       value.GetType().Name, key, this.CurrentIndex);
            }

            this.CurrentEntries[(string)key] = (string)value;
        }
    }
}