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
    public class Order : MvxNotifyPropertyChanged
    {
        private string _id;
        public string ID
        {
            get { return _id; }
            set { _id = value; FirePropertyChanged("ID");}
        }

        private string _purchaseOrder;
        public string PurchaseOrder
        {
            get { return _purchaseOrder; }
            set { _purchaseOrder = value; FirePropertyChanged("PurchaseOrder"); }
        }

        private Customer _customer;
        public Customer Customer
        {
            get { return _customer; }
            set { _customer = value; FirePropertyChanged("Customer"); }
        }

        private Address _billTo;
        public Address BillTo
        {
            get { return _billTo; }
            set { _billTo = value; FirePropertyChanged("BillTo"); }
        }

        private Address _shipTo;
        public Address ShipTo
        {
            get { return _shipTo; }
            set { _shipTo = value; FirePropertyChanged("ShipTo"); }
        }

        private List<Item> _items;
        public List<Item> Items
        {
            get { return _items; }
            set { _items = value; FirePropertyChanged("Items"); }
        }

        public class Item : MvxNotifyPropertyChanged
        {
            private int _quantity;
            public int Quantity
            {
                get { return _quantity; }
                set { _quantity = value; FirePropertyChanged("Quantity"); }
            }

            private string _note;
            public string Note
            {
                get { return _note; }
                set { _note = value; FirePropertyChanged("Note"); }
            }

            private Product _product;
            public Product Product
            {
                get { return _product; }
                set { _product = value; FirePropertyChanged("Product"); }
            }
        }		
    }
}
