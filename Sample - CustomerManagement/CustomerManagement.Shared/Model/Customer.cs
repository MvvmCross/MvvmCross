using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Xml.Serialization;
using System.IO;
using System.Runtime.Serialization;

namespace CustomerManagement.Shared.Model
{
#if (DROID)
    [Android.Runtime.Preserve( AllMembers = true )]
#elif (TOUCH)
    [MonoTouch.Foundation.Preserve (AllMembers = true)]
#endif
  public class Customer
  {
    public Customer()
    {
      ID = "0";
      Name = string.Empty;
      Website = string.Empty;
      PrimaryAddress = new Address();
      Addresses = new List<Address>();
      Contacts = new List<Contact>();
      Orders = new List<Order>();
    }

    public string ID { get; set; }
    public string Name { get; set; }
    public string Website { get; set; }
    public string PrimaryPhone { get; set; }
    public Address PrimaryAddress { get; set; }
    public List<Address> Addresses { get; set; }
    public List<Contact> Contacts { get; set; }
    public List<Order> Orders { get; set; }
  }

#if (DROID)
    [Android.Runtime.Preserve( AllMembers = true )]
#elif (TOUCH)
    [MonoTouch.Foundation.Preserve (AllMembers = true)]
#endif
  //[DataContract]
  public class Address
  {
    public string ID { get; set; }
    public string Description { get; set; }
    public string Street1 { get; set; }
    public string Street2 { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string Zip { get; set; }
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
  }
}
