using System;
using Cirrious.MvvmCross.ViewModels;

namespace CustomerManagement.Core.Models
{
#if (DROID)
    [Android.Runtime.Preserve( AllMembers = true )]
#elif (TOUCH)
    [MonoTouch.Foundation.Preserve (AllMembers = true)]
#endif
    public class Customer : MvxNotifyPropertyChanged
    {
        public Customer()
        {
            ID = Guid.NewGuid().ToString();
            Name = string.Empty;
            Website = string.Empty;
            PrimaryAddress = new Address();
        }

        private string _id;
        public string ID
        {
            get { return _id; }
            set { _id = value; RaisePropertyChanged("ID"); }
        }

        private string _name;
        public string Name
        {
            get { return _name; }
            set { _name = value; RaisePropertyChanged("Name"); }
        }

        private string _website;
        public string Website
        {
            get { return _website; }
            set { _website = value; RaisePropertyChanged("Website"); }
        }

        private string _primaryPhone;
        public string PrimaryPhone
        {
            get { return _primaryPhone; }
            set { _primaryPhone = value; RaisePropertyChanged("PrimaryPhone"); }
        }

        private Address _primaryAddress;
        public Address PrimaryAddress
        {
            get { return _primaryAddress; }
            set { _primaryAddress = value; RaisePropertyChanged("PrimaryAddress"); }
        }

        public void CloneFrom(Customer customer)
        {
            ID = customer.ID;
            Name = customer.Name;
            PrimaryPhone = customer.PrimaryPhone;
            Website = customer.Website;
            PrimaryAddress.CloneFrom(customer.PrimaryAddress);
        }
    }

#if (DROID)
    [Android.Runtime.Preserve( AllMembers = true )]
#elif (TOUCH)
    [MonoTouch.Foundation.Preserve (AllMembers = true)]
#endif
    //[DataContract]
}
