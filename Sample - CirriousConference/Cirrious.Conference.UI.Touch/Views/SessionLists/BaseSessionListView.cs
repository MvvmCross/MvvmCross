using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Cirrious.Conference.Core.Converters;
using Cirrious.Conference.Core.Models;
using Cirrious.Conference.Core.ViewModels.Helpers;
using Cirrious.Conference.Core.ViewModels.SessionLists;
using Cirrious.CrossCore.Converters;
using Cirrious.MvvmCross.Binding.Touch;
using Cirrious.MvvmCross.Binding.Touch.Views;
using Cirrious.MvvmCross.Views;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Cirrious.MvvmCross.Touch.Views;
using Cirrious.MvvmCross.Binding.BindingContext;

namespace Cirrious.Conference.UI.Touch.Views.SessionLists
{
    public class BaseSessionListView<TViewModel, TKey>
        : MvxTableViewController
        where TViewModel : BaseSessionListViewModel<TKey>
    {
        private UIActivityIndicatorView _activityView;

		public new TViewModel ViewModel {
			get { return (TViewModel)base.ViewModel; }
			set { base.ViewModel = value; }
		}

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            NavigationItem.SetRightBarButtonItem(new UIBarButtonItem("Tweet", UIBarButtonItemStyle.Bordered, (sender, e) => ViewModel.DoShareGeneral()), false);

            var converter = typeof (TKey) == typeof (DateTime)
                                ? new SimpleDateValueConverter()
                                : null;
            var source = new TableSource(converter, TableView);
            this.AddBindings(new Dictionary<object, string>()
		                         {
		                             {source, "ItemsSource GroupedList"},
		                         });

            TableView.BackgroundColor = UIColor.Black;
            TableView.RowHeight = 126;
            TableView.Source = source;
            TableView.ReloadData();
        }

        private class TableSource : MvxBaseTableViewSource
        {
            private readonly IMvxValueConverter _keyConverter;
            public TableSource(IMvxValueConverter keyConverter, UITableView tableView)
                : base(tableView)
            {
                _keyConverter = keyConverter;
            }

            private IList<BaseSessionListViewModel<TKey>.SessionGroup> _sessionGroups;
            public IList<BaseSessionListViewModel<TKey>.SessionGroup> ItemsSource
            {
                get
                {
                    return _sessionGroups;
                }
                set 
                { 
                    _sessionGroups = value;
                    ReloadTableData();
                }
            }
			
			public override string TitleForHeader(UITableView tableView, int section)
			{
		       if (_sessionGroups == null)
                    return string.Empty;

               return KeyToString(_sessionGroups[section].Key);
         	}

            public override float GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
            {
                return 126;
            }

            public override int NumberOfSections(UITableView tableView)
            {
                if (_sessionGroups == null)
                    return 0;

                return _sessionGroups.Count;
            }

            public override int RowsInSection(UITableView tableview, int section)
            {
                if (_sessionGroups == null)
                    return 0;

                return _sessionGroups[section].Count;
            }

            protected override UITableViewCell GetOrCreateCellFor(UITableView tableView, NSIndexPath indexPath, object item)
            {
                var reuse = tableView.DequeueReusableCell(SessionCell2.Identifier);
                if (reuse != null)
                    return reuse;

                var cell = SessionCell2.LoadFromNib(tableView);
                return cell;
            }

            public override string[] SectionIndexTitles(UITableView tableView)
            {
                if (_sessionGroups == null)
                    return base.SectionIndexTitles(tableView);

                return _sessionGroups.Select(x => KeyToString(x.Key, 10)).ToArray();
            }

            private string KeyToString(TKey key, int maxLength)
            {
				var candidate = KeyToString(key);
				if (candidate.Length > maxLength)
				{
					candidate = candidate.Substring(0,maxLength);
				}
				return candidate;
            }
			
			private string KeyToString(TKey key)
            {
                if (_keyConverter == null)
                    return key.ToString();

                return (string) _keyConverter.Convert(key, typeof (string), null, CultureInfo.CurrentUICulture);
            }

            protected override object GetItemAt(NSIndexPath indexPath)
            {
                if (_sessionGroups == null)
                    return null;

                return _sessionGroups[indexPath.Section][indexPath.Row];
            }
        }
    }
}