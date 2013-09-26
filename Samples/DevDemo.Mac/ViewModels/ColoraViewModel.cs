using System;
using DevDemo.Core.Services;
using MonoMac.Foundation;

namespace DevDemo.Mac
{
	public class ColoraViewModel : NSObject
	{
		private readonly Colora _colora;
		public ColoraViewModel (Colora colora)
		{
			_colora = colora;
		}

		[Export("name")]
		public string Name {
			get { return _colora.Name; }
			set {
				if (_colora.Name != value) {
					_colora.Name = value;
				}
			}
		}
	}
}

