// MvxSimpleDictionaryParser.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cirrious.MvvmCross.Exceptions;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.Platform.Diagnostics;
using Cirrious.MvvmCross.Platform.Diagnostics;

namespace Cirrious.MvvmCross.Binding.Parse.Basic
{

    public class MvxSimpleDictionaryParser
        : MvxBaseParser
    {
        public bool TryParse(string text, out Dictionary<string, object> dictionary)
        {
            try
            {
                Reset(text);
                dictionary = this.ParseDictionary();
                return true;
            }
            catch (Exception exception)
            {
                MvxTrace.Trace(MvxTraceLevel.Error,
                                        "Problem parsing text {0}", exception.ToLongString());
                dictionary = null;
                return false;
            }
        }
      
		private Dictionary<string, object> ParseDictionary ()
		{
			var toReturn = new Dictionary<string, object> ();

			SkipWhitespace();
			while (!IsComplete) {
				var propertyName = ReadValidCSharpName();
				SkipWhitespace();
				var propertyValue = ReadValue();
				toReturn[propertyName] = propertyValue;
				SkipWhitespaceAndDescriptionSeparators();
			}
			return toReturn;
		}

        protected void SkipWhitespaceAndDescriptionSeparators()
        {
            SkipWhitespaceAndCharacters(';');
        }
    }
}