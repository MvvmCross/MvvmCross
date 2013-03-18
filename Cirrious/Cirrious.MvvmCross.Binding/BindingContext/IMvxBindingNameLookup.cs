// IMvxDefaultBindingName.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Collections.Generic;

namespace Cirrious.MvvmCross.Binding.BindingContext
{
	public interface IMvxBindingNameLookup
	{
		string DefaultFor(Type type);
	}
	public interface IMvxBindingNameRegistry
	{
		void AddOrOverwrite(Type type, string name); 
	}
	public class MvxBindingNameRegistry
		: IMvxBindingNameLookup
		, IMvxBindingNameRegistry

	{
		private readonly Dictionary<Type, string> _lookup = new Dictionary<Type,string>();

		public string DefaultFor (Type type)
		{
			string toReturn;
			_lookup.TryGetValue (type, out toReturn);
			return toReturn;
		}

		public void AddOrOverwrite (Type type, string name)
		{
			_lookup [type] = name;
		}
	}
}