using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace MvvmCross.Forms.Views
{
    public class MvxListView : ListView
    {
        public MvxListView(ListViewCachingStrategy strategy) : base(strategy)
        {
        }

        public MvxListView() : base()
        {
        }

        public ICommand ItemClick
        {
            get;
            set;
        }
    }
}
