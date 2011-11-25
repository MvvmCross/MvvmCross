using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BestSellers
{

    public class Category
    {
        public string ListName { get; set; }
        public string DisplayName { get; set; }
        public string ListNameEncoded
        {
            get { return ListName.ToLower().Replace(' ', '-'); }
        }
        //public string OldestPublishedDate { get; set; }
        //public string NewestPublishedDate { get; set; }
        //public string Updated { get; set; }

        public override string ToString()
        {
            return DisplayName;
        }
    }

    public class CategoryList : List<Category>
    {
    }
}
