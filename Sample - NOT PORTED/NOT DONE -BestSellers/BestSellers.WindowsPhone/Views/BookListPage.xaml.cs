using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;

using MonoCross.Navigation;
using MonoCross.WindowsPhone;

using BestSellers;

namespace BestSellers.WindowsPhone
{

    public class BookListView : MXPhonePage<BookList> { }

    [MXPhoneView("/Views/BookListPage.xaml")]
    public partial class BookListPage : BookListView
    {
        public BookListPage()
        {
            InitializeComponent();
        }

        public override void Render()
        {
            ApplicationTitle.Text = MXContainer.Instance.App.Title;
            PageTitle.Text = Model.CategoryDisplayName;

            //listBox.DataContext = Model;
            foreach (var book in Model)
                listBox.Items.Add(book);

            listBox.SelectionChanged += new SelectionChangedEventHandler(listBox_SelectionChanged);
        }

        void listBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Book b = e.AddedItems[0] as Book;
            if (b != null)
                MXPhoneContainer.Navigate(this, string.Format("{0}/{1}", b.CategoryEncoded, b.ISBN));
        }
    }    
}