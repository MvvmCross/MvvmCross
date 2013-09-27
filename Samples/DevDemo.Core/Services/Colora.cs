using System;
using System.Collections.Generic;

namespace DevDemo.Core.Services
{
	public class Colora
	{
		public Colora()
		{
			Created = DateTime.Now;
			MyBola = new Bola {
				Name = "Meatball",
				Dict = new Dictionary<string, string>()
			};
			MyBola.Dict.Add ("avalue", "import");
		}

	   

		public string Name { get; set; }
		public DateTime Created { get; set; }
		public Bola MyBola { get; set; }
	}
}

