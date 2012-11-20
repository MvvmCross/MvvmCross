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

    public class Product : MvxNotifyPropertyChanged
    {
        private string _id;
        public string ID
        {
            get { return _id; }
            set { _id = value; FirePropertyChanged("ID"); }
        }

        private string _description;
        public string Description
        {
            get { return _description; }
            set { _description = value; FirePropertyChanged("Description"); }
        }

        private decimal _price;
        public Decimal Price
        {
            get { return _price; }
            set { _price = value; FirePropertyChanged("Price"); }
        }

        private string _manufacturer;
        public string Manufacturer
        {
            get { return _manufacturer; }
            set { _manufacturer = value; FirePropertyChanged("Manufacturer"); }
        }

        public static Product GetProduct(string productId) 
        {
            // TODO: Implement some product RESTful endpoint GET;
            return new Product(); 
        }
    }
}
