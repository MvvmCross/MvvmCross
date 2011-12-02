using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Xml.Linq;

using MonoCross.Navigation;
using BestSellers;

namespace BestSellers.Controllers
{
    class BookController : MXController<Book>
    {
        public override string Load(Dictionary<string, string> parameters)
        {
            string category = parameters.ContainsKey("Category") ? parameters["Category"] : string.Empty;
            string book = parameters.ContainsKey("Book") ? parameters["Book"] : string.Empty;

            //Name = category;
            Model = Book.Find(category, book);

            return ViewPerspective.Read;
        }
    }
}
