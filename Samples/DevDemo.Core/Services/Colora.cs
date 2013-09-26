using System;

namespace DevDemo.Core.Services
{
	public class Colora
	{
		public Colora()
		{
			Created = DateTime.Now;
		}

		public string Name { get; set; }
		public DateTime Created { get; set; }
	}
}

