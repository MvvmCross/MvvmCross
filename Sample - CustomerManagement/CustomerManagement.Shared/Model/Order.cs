using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace CustomerManagement.Shared.Model
{
#if (DROID)
    [Android.Runtime.Preserve( AllMembers = true )]
#elif (TOUCH)
    [MonoTouch.Foundation.Preserve (AllMembers = true)]
#endif    
    public class Order
    {
        public string ID { get; set; }
        public string PurchaseOrder { get; set; }
        public Customer Customer { get; set; }
        public Address BillTo { get; set; }
        public Address ShipTo { get; set; }
        public List<Item> Items { get; set; }

        public class Item
        {
            public int Quantity { get; set; }
            public string Note { get; set; }
            public Product Product { get; set; }
        }		
    }
}
