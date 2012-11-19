using System;
using Cirrious.MvvmCross.ViewModels;

namespace CustomerManagement.AutoViews.Core.Models
{
    public class Address : MvxNotifyPropertyChanged
    {
        public Address()
        {
            ID = Guid.NewGuid().ToString();
        }

        private string _id;
        public string ID
        {
            get { return _id; }
            set { _id = value; RaisePropertyChanged("ID"); }
        }

        private string _description;
        public string Description
        {
            get { return _description; }
            set { _description = value; RaisePropertyChanged("Description"); }
        }

        private string _street1;
        public string Street1
        {
            get { return _street1; }
            set { _street1 = value; RaisePropertyChanged("Street1"); }
        }

        private string _street2;
        public string Street2
        {
            get { return _street2; }
            set { _street2 = value; RaisePropertyChanged("Street2"); }
        }

        private string _city;
        public string City
        {
            get { return _city; }
            set { _city = value; RaisePropertyChanged("City"); }
        }

        private string _state;
        public string State
        {
            get { return _state; }
            set { _state = value; RaisePropertyChanged("State"); }
        }

        private string _zip;
        public string Zip
        {
            get { return _zip; }
            set { _zip = value; RaisePropertyChanged("Zip"); }
        }

        public override string ToString()
        {
            string address = Description;
            address += Environment.NewLine;
            address += Street1;
            address += Environment.NewLine;
            if (!string.IsNullOrWhiteSpace(Street2))
            {
                address += Street2;
                address += Environment.NewLine;
            }
            address += string.Format("{0}, {1} {2}", City, State, Zip);
            return address;
        }

        public void CloneFrom(Address address)
        {
            ID = address.ID;
            City = address.City;
            State = address.State;
            Street1 = address.Street1;
            Street2 = address.Street2;
            Description = address.Description;
            Zip = address.Zip;
        }
    }
}