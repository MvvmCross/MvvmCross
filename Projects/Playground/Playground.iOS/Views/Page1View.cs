using System;
using CoreGraphics;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding;
using MvvmCross.Platforms.Ios.Binding.Views;
using MvvmCross.Platforms.Ios.Presenters.Attributes;
using MvvmCross.Platforms.Ios.Views;
using MvvmCross.Platforms.Ios.Views.Expandable;
using Playground.Core.ViewModels;
using UIKit;

namespace Playground.iOS.Views
{
    [MvxFromStoryboard("Main")]
    [MvxPagePresentation(WrapInNavigationController = false)]
    public partial class Page1View : MvxViewController<Page1ViewModel>
    {
        private UITableView _tableView;
        private TableSource _source;

        public Page1View(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            Title = "Page 1";

            _tableView = new UITableView
            {
                TranslatesAutoresizingMaskIntoConstraints = false
            };

            _tableView.TableFooterView = new UIView();

            _tableView.RegisterClassForCellReuse(typeof(HeaderCell), HeaderCell.Identifier);
            _tableView.RegisterClassForCellReuse(typeof(ItemCell), ItemCell.Identifier);

            _source = new TableSource(_tableView);
            _tableView.Source = _source;

            Add(_tableView);

            _tableView.LeadingAnchor.ConstraintEqualTo(View.LeadingAnchor).Active = true;
            _tableView.TrailingAnchor.ConstraintEqualTo(View.TrailingAnchor).Active = true;
            _tableView.TopAnchor.ConstraintEqualTo(View.SafeAreaLayoutGuide.TopAnchor).Active = true;
            _tableView.BottomAnchor.ConstraintEqualTo(View.SafeAreaLayoutGuide.BottomAnchor).Active = true;

            var set = CreateBindingSet();
            set.Bind(_source).To(vm => vm.Sections);
            set.Bind(_source).For(v => v.HeaderTappedCommand).To(vm => vm.HeaderTappedCommand);
            set.Apply();
        }

        private class HeaderCell : MvxTableViewCell, IExpandableHeaderCell
        {
            public static NSString Identifier = new NSString(nameof(HeaderCell));
            private static UIImage _arrowImage;
            private UILabel _title;
            private UIImageView _arrow;
            private UISwitch _switch;

            public HeaderCell(IntPtr handle) : base(handle)
            {
                Initialize();
            }

            public HeaderCell()
            {
                Initialize();
            }

            private void Initialize()
            {
                if (_arrowImage == null)
                    _arrowImage = UIImage.FromBundle("ArrowDown");

                _title = new UILabel
                {
                    TranslatesAutoresizingMaskIntoConstraints = false
                };

                _arrow = new UIImageView(_arrowImage)
                {
                    TranslatesAutoresizingMaskIntoConstraints = false,
                    Hidden = true
                };

                _switch = new UISwitch
                {
                    TranslatesAutoresizingMaskIntoConstraints = false,
                    TintColor = UIColor.Blue,
                    Hidden = true
                };

                ContentView.Add(_title);
                ContentView.Add(_arrow);
                ContentView.Add(_switch);

                _title.LeadingAnchor.ConstraintEqualTo(ContentView.LeadingAnchor, 10).Active = true;
                _title.CenterYAnchor.ConstraintEqualTo(ContentView.CenterYAnchor).Active = true;

                _arrow.TrailingAnchor.ConstraintEqualTo(ContentView.TrailingAnchor, -10).Active = true;
                _arrow.CenterYAnchor.ConstraintEqualTo(ContentView.CenterYAnchor).Active = true;
                _arrow.WidthAnchor.ConstraintEqualTo(15).Active = true;
                _arrow.HeightAnchor.ConstraintEqualTo(_arrow.WidthAnchor).Active = true;

                _switch.TrailingAnchor.ConstraintEqualTo(ContentView.TrailingAnchor, -10).Active = true;
                _switch.CenterYAnchor.ConstraintEqualTo(ContentView.CenterYAnchor).Active = true;

                var set = this.CreateBindingSet<HeaderCell, Page1ViewModel.SectionViewModel>();
                set.Bind(_title).To(vm => vm.Title);
                set.Bind(_arrow).For(v => v.BindHidden()).To(vm => vm.ShowsControl);
                set.Bind(_switch).For(v => v.BindVisible()).To(vm => vm.ShowsControl);
                set.Bind(_switch).To(vm => vm.On);
                set.Apply();
            }

            public void OnCollapsed()
            {
                _arrow.Transform = CGAffineTransform.MakeIdentity();
            }

            public void OnExpanded()
            {
                _arrow.Transform = CGAffineTransform.MakeRotation(180 * ((float)Math.PI / 180.0f));
            }
        }

        private class ItemCell : MvxTableViewCell
        {
            public static NSString Identifier = new NSString(nameof(ItemCell));
            private UILabel _title;

            public ItemCell(IntPtr handle) : base(handle)
            {
                Initialize();
            }

            public ItemCell()
            {
                Initialize();
            }

            private void Initialize()
            {
                _title = new UILabel
                {
                    TranslatesAutoresizingMaskIntoConstraints = false
                };

                ContentView.Add(_title);

                _title.LeadingAnchor.ConstraintEqualTo(ContentView.LeadingAnchor, 10).Active = true;
                _title.TrailingAnchor.ConstraintEqualTo(ContentView.TrailingAnchor, -10).Active = true;
                _title.CenterYAnchor.ConstraintEqualTo(ContentView.CenterYAnchor).Active = true;

                var set = this.CreateBindingSet<ItemCell, Page1ViewModel.SectionItemViewModel>();
                set.Bind(_title).To(vm => vm.Title);
                set.Apply();
            }
        }

        private class TableSource : MvxExpandableTableViewSource<Page1ViewModel.SectionViewModel, Page1ViewModel.SectionItemViewModel>
        {
            public TableSource(UITableView tableView) : base(tableView)
            {
            }

            protected override UITableViewCell GetOrCreateCellFor(UITableView tableView, NSIndexPath indexPath, object item)
            {
                return TableView.DequeueReusableCell(ItemCell.Identifier);
            }

            protected override UITableViewCell GetOrCreateHeaderCellFor(UITableView tableView, nint section)
            {
                return tableView.DequeueReusableCell(HeaderCell.Identifier);
            }
        }
    }
}
