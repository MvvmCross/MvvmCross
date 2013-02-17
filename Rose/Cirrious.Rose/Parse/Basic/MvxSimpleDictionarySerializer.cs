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

	public class MvxSimpleDictionarySerializer
	{
		public string Serialize (Dictionary<string, object> source)
		{
			var sb = new StringBuilder();
			foreach (var kvp in source) {
				if (sb.Length > 0)
					sb.Append(";");
				sb.AppendFormat("{0} {1}", kvp.Key, WrapValue(kvp.Value));
			}
			return sb.ToString ();
		}

		private string WrapValue (object value)
		{
			if (value == null) {
				return "null";
			}
			if (value is string) {
				var toReturn = ((string)value)
								.Replace ("\\", "\\\\")
								.Replace ("\"", "\\\"");
				return "\"" + toReturn + "\"";
			}
			if (value is int) {
				return ((int)value).ToString (System.Globalization.CultureInfo.InvariantCulture);
			} else if (value is long) {
				return ((long)value).ToString (System.Globalization.CultureInfo.InvariantCulture);
			}else if (value is float) {
				return ((float)value).ToString (System.Globalization.CultureInfo.InvariantCulture);
			} else if (value is double) {
				return ((double)value).ToString (System.Globalization.CultureInfo.InvariantCulture);
			} else if (value is bool) {
				return ((bool)value).ToString (System.Globalization.CultureInfo.InvariantCulture);
			} else if (value is bool) {
				return ((bool)value).ToString (System.Globalization.CultureInfo.InvariantCulture);
			} else if (value.GetType ().IsEnum ) {
				return value.ToString();
			}

			MvxTrace.Trace(MvxTraceLevel.Warning, "Unexpected type - serialization may fail for type {0} with value {1}", value.GetType(), value);
			return value.ToString();
		}
	}
    
}