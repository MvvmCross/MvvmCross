using System;
using System.Collections.Generic;

namespace DevDemo.Core.Services
{
	public class ColoraService : IColoraService
	{
		public Colora CreateColora (string extra = "")
		{
			return new Colora () {
				Name = _coloras[Random(_coloras.Count)] + extra
			};
		}

		private readonly List<string> _coloras = new List<string> () {
			"Red", "Orange", "Yellow", "Green", "Teal", "Blue", "Violet", "Black", "White"
		};

		private readonly System.Random _random = new System.Random();
		private int Random(int count)
		{
			return _random.Next(count);
		}
	}
}

