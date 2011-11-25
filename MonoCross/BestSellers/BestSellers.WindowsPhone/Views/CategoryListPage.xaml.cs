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
    public class CategoryListView : MXPhonePage<CategoryList> { }

    [MXPhoneView("/Views/CategoryListPage.xaml")]
    public partial class CategoryListPage : CategoryListView
    {
        // Constructor
        public CategoryListPage()
        {
            InitializeComponent();

            ApplicationTitle.Text = MXContainer.Instance.App.Title;
            PageTitle.Text = "Categories";
        }

        public override void Render()
        {
            foreach (var category in Model)
                listBox.Items.Add(category);

            listBox.SelectionChanged += new SelectionChangedEventHandler(listBox_SelectionChanged);

            // remove the splash screen that was shown just before this
            NavigationService.RemoveBackEntry();
        }

        void listBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Category c = e.AddedItems[0] as Category;

            MXPhoneContainer.Navigate(this, c.ListNameEncoded);
        }
    }
}