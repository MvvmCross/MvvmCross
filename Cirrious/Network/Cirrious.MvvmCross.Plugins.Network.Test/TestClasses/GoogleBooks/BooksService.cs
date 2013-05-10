using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cirrious.MvvmCross.Plugins.Network.Test.TestClasses.GoogleBooks
{
    public static class BooksService
    {
        public static string GetSearchUrl(string whatFor)
        {
            string address = string.Format("https://www.googleapis.com/books/v1/volumes?q={0}",
                                            Uri.EscapeDataString(whatFor));
            return address;
        }
    }
}
