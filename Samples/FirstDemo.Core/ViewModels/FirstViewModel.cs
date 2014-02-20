using System;
using Cirrious.MvvmCross.ViewModels;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

namespace FirstDemo.Core.ViewModels
{
	public class FirstViewModel : MvxViewModel
	{
		public FirstViewModel()
		{
			initContacts ();

		}

		#region Contacts

		private IEnumerable<IContact> _contacts;
		public IEnumerable<IContact> Contacts
		{
			get { return _contacts; }
			set {
				_contacts = value;
				RaisePropertyChanged (() => Contacts);
			}
		}

		private void initContacts()
		{
			var contacts = new List<IContact> ();
			contacts.Add (new Contact () {
				Name = "Neve Campbell",
				Address = null,			// what happens if one of the values is null?
			});


			contacts.Add (new Contact () {
				Name = "Sarah Jessica Parker",
				Address = "P.O. Box 10459 \nBurbank, CA 91510",
			});
			contacts.Add (new Contact () {
				Name = "Melissa Joan Hart",
				Address = "2000 Avenue Of The Stars \nLos Angeles, CA 90067",
			});

			contacts.First ().Items.Add (new Item("combo"));
			contacts.First ().Items.Add (new Item("egg"));
			contacts.First ().Items.Add (new Item("shoe"));


			contacts.First ().Contacts = new List<IContact> ();

			contacts.First ().Contacts.Add (new Contact () {
				Name = "Alice Eve",
				Address = "2000 Avenue Of The Stars \nLos Angeles, CA 90067",
			});

			contacts.First ().Contacts.Add (new Contact () {
				Name = "George Lucas",
				Address = "2000 Avenue Of The Stars \nLos Angeles, CA 90067",
			});

			contacts.First ().Contacts.First ().Contacts = new List<IContact> ();

			contacts.First ().Contacts.First().Contacts.Add (new Contact () {
				Name = "George Kostanza",
				Address = "444 Bobo Ln \nLos Angeles, CA 90067",
			});

			Contacts = contacts;
		}

		#endregion Contacts

		#region Name

		private string _firstName = string.Empty;
		public string FirstName
		{
			get { return _firstName; } 
			set { _firstName = value;
				RaisePropertyChanged (() => FirstName);
				RaisePropertyChanged (() => FullName);
			}
		}

		private string _lastName = string.Empty;
		public string LastName
		{
			get { return _lastName; } 
			set { _lastName = value;
				RaisePropertyChanged (() => LastName); 
				RaisePropertyChanged (() => FullName);
			}
		}

		public string FullName
		{
			get { return string.Format ("{0} {1}", FirstName, LastName); }
		}

		#endregion Name
	}
}