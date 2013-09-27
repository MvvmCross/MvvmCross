using System;
using System.Collections;
using Cirrious.MvvmCross.ViewModels;

namespace DevDemo.Core.Services
{
	public class Bola : MvxViewModel
	{
		private string _name;
		public string Name { 
			get {
				return _name;
			}
			set {
				if (_name != value) {
					_name = value;
					RaisePropertyChanged (() => Name);
				}
			}
		}
		public IDictionary Dict { get; set; }
	}
}

