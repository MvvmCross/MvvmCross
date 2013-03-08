using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Xml.Linq;
using Cirrious.MvvmCross.ViewModels;

namespace BestSellers.ViewModels
{
    public class CategoryListViewModel : BaseViewModel
    {
        const string URL_CATEGORIES = "http://api.nytimes.com/svc/books/v2/lists/names.xml?api-key=d8ad3be01d98001865e96ee55c1044db:8:57889697";

        private List<CategoryDataViewModel> _list;
        public List<CategoryDataViewModel> List
        {
            get { return _list; }
            set { _list = value; RaisePropertyChanged("List"); }
        }

        public void Init()
        {
            AsyncLoad();
        }

        private void AsyncLoad()
        {
            GeneralAsyncLoad(URL_CATEGORIES, ProcessResult);
        }

        private void ProcessResult(Stream stream)
        {
            XDocument loaded = XDocument.Load(stream);
            var categories = from item in loaded.Descendants("result")
                             select new CategoryDataViewModel()
                             {
                                 ListName = item.Element("list_name").Value,
                                 DisplayName = item.Element("display_name").Value,
                                 //ListNameEncoded = item.Element("list_name_encoded").Value,
                                 //OldestPublishedDate = item.Element("oldest_published_date").Value,
                                 //NewestPublishedDate = item.Element("newest_published_date").Value
                             };

            List = categories.ToList();
        }
    }
}
