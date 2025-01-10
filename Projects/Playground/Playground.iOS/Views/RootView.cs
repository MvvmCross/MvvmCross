using MvvmCross.Platforms.Ios.Presenters.Attributes;
using MvvmCross.Platforms.Ios.Views;
using Playground.Core.ViewModels;

namespace Playground.iOS.Views;

[MvxRootPresentation(WrapInNavigationController = true)]
public sealed class RootView : MvxViewController<RootViewModel>
{
    private UIButton _btnTabs;
    private UIButton _btnPages;
    private UIButton _btnSplit;
    private UIButton _btnChild;
    private UIButton _btnChildWithResult;
    private UIButton _btnModal;
    private UIButton _btnNavModal;
    private UIButton _btnOverrideAttribute;
    private UIButton _btnCustomBinding;

    public override void LoadView()
    {
        base.LoadView();

        View.BackgroundColor = UIColor.LightGray;

        _btnTabs = new UIButton { TranslatesAutoresizingMaskIntoConstraints = false };
        _btnPages = new UIButton { TranslatesAutoresizingMaskIntoConstraints = false };
        _btnSplit = new UIButton { TranslatesAutoresizingMaskIntoConstraints = false };
        _btnChild = new UIButton { TranslatesAutoresizingMaskIntoConstraints = false };
        _btnChildWithResult = new UIButton { TranslatesAutoresizingMaskIntoConstraints = false };
        _btnModal = new UIButton { TranslatesAutoresizingMaskIntoConstraints = false };
        _btnNavModal = new UIButton { TranslatesAutoresizingMaskIntoConstraints = false };
        _btnOverrideAttribute = new UIButton { TranslatesAutoresizingMaskIntoConstraints = false };
        _btnCustomBinding = new UIButton { TranslatesAutoresizingMaskIntoConstraints = false };

        _btnTabs.SetTitle("Tabs Navigation", UIControlState.Normal);
        _btnPages.SetTitle("Page Navigation", UIControlState.Normal);
        _btnSplit.SetTitle("Split Navigation", UIControlState.Normal);
        _btnModal.SetTitle("Show Modal", UIControlState.Normal);
        _btnNavModal.SetTitle("Modal with Navigation Stack", UIControlState.Normal);
        _btnOverrideAttribute.SetTitle("Show Modal (Custom Pres. Attribute)", UIControlState.Normal);
        _btnCustomBinding.SetTitle("Show Custom Binding", UIControlState.Normal);
        _btnChildWithResult.SetTitle("Show Child With Result", UIControlState.Normal);
        _btnChild.SetTitle("Show Child", UIControlState.Normal);

        var stack =
            new UIStackView([
                _btnTabs, _btnPages, _btnSplit, _btnChild, _btnChildWithResult, _btnModal, _btnNavModal,
                _btnOverrideAttribute, _btnCustomBinding
            ])
            {
                TranslatesAutoresizingMaskIntoConstraints = false,
                Spacing = 8,
                Axis = UILayoutConstraintAxis.Vertical,
                Alignment = UIStackViewAlignment.Top,
                Distribution = UIStackViewDistribution.EqualSpacing,
            };

        var scrollView = new UIScrollView
        {
            TranslatesAutoresizingMaskIntoConstraints = false,
            InsetsLayoutMarginsFromSafeArea = true,
            ContentInset = new UIEdgeInsets(0, 16, 0, 16)
        };
        scrollView.AddSubview(stack);

        Add(scrollView);

        scrollView.TopAnchor.ConstraintEqualTo(View.TopAnchor).Active = true;
        scrollView.BottomAnchor.ConstraintEqualTo(View.BottomAnchor).Active = true;
        scrollView.LeadingAnchor.ConstraintEqualTo(View.LeadingAnchor).Active = true;
        scrollView.TrailingAnchor.ConstraintEqualTo(View.TrailingAnchor).Active = true;

        stack.TopAnchor.ConstraintEqualTo(scrollView.TopAnchor).Active = true;
        stack.BottomAnchor.ConstraintEqualTo(scrollView.BottomAnchor).Active = true;
        stack.LeadingAnchor.ConstraintEqualTo(scrollView.LeadingAnchor).Active = true;
        stack.TrailingAnchor.ConstraintEqualTo(scrollView.TrailingAnchor).Active = true;
        stack.WidthAnchor.ConstraintEqualTo(View.WidthAnchor).Active = true;
    }

    public override void ViewDidLoad()
    {
        base.ViewDidLoad();

        using var set = CreateBindingSet();
        set.Bind(_btnTabs).To(vm => vm.ShowTabsCommand);
        set.Bind(_btnPages).To(vm => vm.ShowPagesCommand);
        set.Bind(_btnSplit).To(vm => vm.ShowSplitCommand);
        set.Bind(_btnChild).To(vm => vm.ShowChildCommand);
        set.Bind(_btnModal).To(vm => vm.ShowModalCommand);
        set.Bind(_btnNavModal).To(vm => vm.ShowModalNavCommand);
        set.Bind(_btnOverrideAttribute).To(vm => vm.ShowOverrideAttributeCommand);
        set.Bind(_btnCustomBinding).To(vm => vm.ShowCustomBindingCommand);
        set.Bind(_btnChildWithResult).To(vm => vm.ShowViewModelWithResult);
    }
}
