using MvvmCross.Platforms.Ios.Views;
using Playground.Core.ViewModels.Navigation;

namespace Playground.iOS.Views;

public sealed class ChildWithResultViewController : MvxViewController<ChildWithResultViewModel>
{
    private UITextField _message;
    private UITextField _value;
    private UIButton _close;

    public override void LoadView()
    {
        base.LoadView();

        _message = new UITextField
        {
            TranslatesAutoresizingMaskIntoConstraints = false,
            KeyboardType = UIKeyboardType.Default,
            ReturnKeyType = UIReturnKeyType.Next,
            ShouldReturn = ShouldReturn,
            TextColor = UIColor.White
        };

        _value = new UITextField
        {
            TranslatesAutoresizingMaskIntoConstraints = false,
            KeyboardType = UIKeyboardType.NumberPad,
            ReturnKeyType = UIReturnKeyType.Done,
            ShouldReturn = ShouldReturn,
            TextColor = UIColor.White
        };

        _close = new UIButton { TranslatesAutoresizingMaskIntoConstraints = false };
        _close.SetTitle("Close", UIControlState.Normal);

        Add(_message);
        Add(_value);
        Add(_close);

        NSLayoutConstraint.ActivateConstraints([
            _message.TopAnchor.ConstraintEqualTo(View.SafeAreaLayoutGuide.TopAnchor, 16),
            _message.LeadingAnchor.ConstraintEqualTo(View.LeadingAnchor, 16),
            _message.TrailingAnchor.ConstraintEqualTo(View.TrailingAnchor, -16),
            _message.HeightAnchor.ConstraintEqualTo(40),

            _value.TopAnchor.ConstraintEqualTo(_message.BottomAnchor, 16),
            _value.LeadingAnchor.ConstraintEqualTo(View.LeadingAnchor, 16),
            _value.TrailingAnchor.ConstraintEqualTo(View.TrailingAnchor, 16),
            _value.HeightAnchor.ConstraintEqualTo(40),

            _close.TopAnchor.ConstraintEqualTo(_value.BottomAnchor, 16),
            _close.LeadingAnchor.ConstraintEqualTo(View.LeadingAnchor, 16),
            _close.TrailingAnchor.ConstraintEqualTo(View.TrailingAnchor, 16)
        ]);
    }

    private bool ShouldReturn(UITextField textfield)
    {
        if (textfield.Equals(_message))
        {
            _message.ResignFirstResponder();
            _value.BecomeFirstResponder();
            return true;
        }

        if (textfield.Equals(_value))
        {
            _message.ResignFirstResponder();
            _value.ResignFirstResponder();
            return true;
        }

        return false;
    }

    public override void ViewDidLoad()
    {
        base.ViewDidLoad();

        var set = CreateBindingSet();
        set.Bind(_message).To(vm => vm.Message).TwoWay();
        set.Bind(_value).To(vm => vm.Value).TwoWay();
        set.Bind(_close).To(vm => vm.CloseCommand);
        set.Apply();
    }

    public override void ViewDidAppear(bool animated)
    {
        base.ViewDidAppear(animated);

        _message.BecomeFirstResponder();
    }
}
