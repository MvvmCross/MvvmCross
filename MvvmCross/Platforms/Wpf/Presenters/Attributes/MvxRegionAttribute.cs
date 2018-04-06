using System;

namespace MvvmCross.Platforms.Wpf.Presenters.Attributes
{
	[AttributeUsage(AttributeTargets.Class, Inherited = false)]
	public class MvxRegionAttribute
		: Attribute
	{
		public string Name { get; private set; }

		public MvxRegionAttribute(string regionName)
		{
			Name = regionName;
		}
	}
}
