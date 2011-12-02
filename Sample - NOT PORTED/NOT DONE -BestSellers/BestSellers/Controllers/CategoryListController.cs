using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Net;
using System.IO;

using MonoCross.Navigation;

using BestSellers;

namespace BestSellers.Controllers
{
    public class CategoryListController : MXController<CategoryList>, IMXController
    {
        const string URL_CATEGORIES = "http://api.nytimes.com/svc/books/v2/lists/names.xml?api-key=d8ad3be01d98001865e96ee55c1044db:8:57889697";

        public override string Load(Dictionary<string, string> parameters)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL_CATEGORIES);

            Model = new CategoryList();
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream stream = response.GetResponseStream();
            if (stream == null)
                throw new Exception("Failed to connect the New Yorw Times Best Sellers Serivce");

            XDocument loaded = XDocument.Load(stream);
            var categories = from item in loaded.Descendants("result")
                select new Category()
                {
                    ListName = item.Element("list_name").Value,
                    DisplayName = item.Element("display_name").Value,
                    //ListNameEncoded = item.Element("list_name_encoded").Value,
                    //OldestPublishedDate = item.Element("oldest_published_date").Value,
                    //NewestPublishedDate = item.Element("newest_published_date").Value
                };

            Model.AddRange(categories);

            return ViewPerspective.Read;
        }
    }
}
