using System;

namespace DevDemo.Core.Services
{
	public interface IColorService
	{
		Color CreateNewColor(string color = "");
	}
}

