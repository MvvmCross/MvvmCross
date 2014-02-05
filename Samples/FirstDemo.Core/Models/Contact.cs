using System;
using Cirrious.MvvmCross.ViewModels;
using System.ComponentModel;

namespace FirstDemo.Core
{
	public interface IContact : INotifyPropertyChanged
	{
		string Name { get; set; }
		string Address { get; set; }
	}

	public class Contact : MvxNotifyPropertyChanged, IContact
	{
		public Contact()
		{
			ShouldAlwaysRaiseInpcOnUserInterfaceThread (true);
		}

		private string _name;
		public string Name
		{
			get { return _name; }
			set { _name = value; RaisePropertyChanged (() => Name); }
		}

		private string _address;
		public string Address
		{
			get { return _address; }
			set { _address = value; RaisePropertyChanged (() => Address); }
		}

		public override string ToString ()
		{
			return string.Format ("[Contact: Name={0}, Address={1}]", Name, Address);
		}
	}
}

