using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Cirrious.MvvmCross.ViewModels;

namespace CustomerManagement.Shared.Model
{

#if (DROID)
    [Android.Runtime.Preserve( AllMembers = true )]
#elif (TOUCH)
    [MonoTouch.Foundation.Preserve (AllMembers = true)]
#endif
    public class Contact : MvxNotifyPropertyChanged
    {
        private string _id;
        public string ID
        {
            get { return _id; }
            set { _id = value; FirePropertyChanged("ID"); }
        }

        private string _firstName;
        public string FirstName
        {
            get { return _firstName; }
            set { _firstName = value; FirePropertyChanged("FirstName"); }
        }

        private string _middleName;
        public string MiddleName
        {
            get { return _middleName; }
            set { _middleName = value; FirePropertyChanged("MiddleName"); }
        }

        private string _lastName;
        public string LastName
        {
            get { return _lastName; }
            set { _lastName = value; FirePropertyChanged("LastName"); }
        }

        private string _title;
        public string Title
        {
            get { return _title; }
            set { _title = value; FirePropertyChanged("Title"); }
        }

        private string _email;
        public string Email
        {
            get { return _email; }
            set { _email = value; FirePropertyChanged("Email"); }
        }

        private string _officePhone;
        public string OfficePhone
        {
            get { return _officePhone; }
            set { _officePhone = value; FirePropertyChanged("OfficePhone"); }
        }

        private string _mobilePhone;
        public string MobilePhone
        {
            get { return _mobilePhone; }
            set { _mobilePhone = value; FirePropertyChanged("MobilePhone"); }
        }
    }
}
