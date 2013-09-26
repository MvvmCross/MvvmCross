using System;
using System.Collections.Generic;

namespace DevDemo.Core.Services
{
	public class ColorService : IColorService
	{
		public Color CreateColor (string extra = "")
		{
			return new Color () {
				Name = _colors[Random(_colors.Count)] + extra
			};
		}

		private readonly List<string> _colors = new List<string> () {
			"Red", "Orange", "Yellow", "Green", "Blue", "Violet"
		};

		private readonly System.Random _random = new System.Random();
		private int Random(int count)
		{
			return _random.Next(count);
		}
	}
}

