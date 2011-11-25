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

    public class Product
    {
        public string ID { get; set; }
        public string Description { get; set; }
        public Decimal Price { get; set; }
        public string Manufacturer { get; set; }
        
        public static Product GetProduct(string productId) 
        {
            // TODO: Implement some product RESTful endpoint GET;
            return new Product(); 
        }
    }
}
