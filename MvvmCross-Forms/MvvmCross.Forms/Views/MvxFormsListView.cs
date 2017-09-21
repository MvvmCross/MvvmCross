using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace MvvmCross.Forms.Views
{
    public class MvxFormsListView : ListView
    {
        public ICommand ItemClick
        {
            get;
            set;
        } = new Command(() => { });

        public MvxFormsListView(ListViewCachingStrategy strategy) : base(strategy)
        {

        }

        public MvxFormsListView() : base(ListViewCachingStrategy.RetainElement)
        {

        }
    }
}
