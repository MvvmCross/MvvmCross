using System;

namespace DevDemo.Core.Services
{
	public class Colora
	{
		public Colora()
		{
			Created = DateTime.Now;
			MyBola = new Bola {
				Name = "Meatball"
			};
		}

		public string Name { get; set; }
		public DateTime Created { get; set; }
		public Bola MyBola { get; set; }
	}
}

