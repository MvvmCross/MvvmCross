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
	public class MvxSimpleSerialization
		: IMvxSimpleSerialization
	{
		public Dictionary<string, object> Deserialize (string text)
		{
			var parser = new MvxSimpleDictionaryParser();
			Dictionary<string, object> result;
			parser.TryParse(text, out result); 
			return result;
		}

		public string Serialize (Dictionary<string, object> input)
		{
			var serializer = new MvxSimpleDictionarySerializer();
			return serializer.Serialize(input);
		}
	}


}