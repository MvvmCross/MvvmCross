using System;
using System.Collections.Generic;
using Cirrious.MvvmCross.ViewModels;

namespace DevDemo.Core.Services
{
	public class Colora : MvxViewModel
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
					  
		private string _name;
		public string Name { 
			get {
				return _name;
			}
			set {
				if (_name != value) {
					_name = value;
					RaisePropertyChanged (() => Name);
					RaisePropertyChanged (() => Consolidated);
				}
			}
		}

		private DateTime _created;
		public DateTime Created { 
			get {
				return _created;
			}
			set {
				if (_created != value) {
					_created = value;
					RaisePropertyChanged (() => Created);
				}
			}
		}

		private Bola _myBola;
		public Bola MyBola 
		{ 
			get {
				return _myBola;
			} 
			set {
				if (_myBola != value) {
					_myBola = value;
					RaisePropertyChanged (() => MyBola);
				}
			}
		}

		public string Consolidated
		{
			get { return Name + "hello"; }
		}

	}
}

