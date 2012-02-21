using System.Collections.Generic;
using Cirrious.MvvmCross.Binding.Interfaces;
using Cirrious.MvvmCross.Binding.Touch.Views;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;
using Cirrious.MvvmCross.Views;
using Tutorial.Core.ViewModels;
using Cirrious.MvvmCross.Touch.Views;

namespace Tutorial.UI.Touch.Views
{
	public class MainMenuView
		: MvxTouchTableViewController<MainMenuViewModel>
		, IMvxServiceConsumer<IMvxBinder>
	{
		private readonly List<IMvxUpdateableBinding> _bindings;

        public MainMenuView(MvxShowViewModelRequest request)
            : base(request)
		{
			_bindings = new List<IMvxUpdateableBinding>();
		}
		
		protected override void Dispose (bool disposing)
		{
			if (disposing)
			{
                _bindings.ForEach(x => x.Dispose());
			}
			
			base.Dispose(disposing);
		}

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            Title = "Views";

            var tableDelegate = new MvxBindableTableViewDelegate();
            tableDelegate.SelectionChanged += (sender, args) => ViewModel.ShowItemCommand.Execute(args.AddedItems[0]);
            var tableSource = new MvxDefaultBindableTableViewDataSource(TableView);
            var binder = this.GetService<IMvxBinder>();
            _bindings.AddRange(binder.Bind(ViewModel, tableDelegate, "{'ItemsSource':{'Path':'Items'}}"));
            _bindings.AddRange(binder.Bind(ViewModel, tableSource, "{'ItemsSource':{'Path':'Items'}}"));

            TableView.Delegate = tableDelegate;
            TableView.DataSource = tableSource;
            TableView.ReloadData();
        }
	}
}

