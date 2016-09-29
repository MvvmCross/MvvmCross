using System;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.iOS.Views;
using MvvmCross.iOS.Views;
using SampleCore.ViewModels;
using UIKit;

namespace Sample.iOS
{
    public class MainView : MvxViewController<MainViewModel>
    {
        public MainView()
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            var table = new UITableView();
            table.RowHeight = UITableView.AutomaticDimension;
            table.EstimatedRowHeight = 540;

            var source = new MvxSimpleTableViewSource(table, TestCell.Key);
            table.Source = source;

            View = table;

            var set = this.CreateBindingSet<MainView, MainViewModel>();
            set.Bind(source).To(vm => vm.TestItems);
            set.Apply();

            table.ReloadData();

        }
    }
}
