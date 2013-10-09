using System.Drawing;
using ApiExamples.Core.ViewModels;
using Cirrious.MvvmCross.Binding.BindingContext;
using Cirrious.MvvmCross.Binding.Touch.Views;
using Cirrious.MvvmCross.Touch.Views;
using MonoTouch.UIKit;
using MonoTouch.Foundation;

namespace ApiExamples.Touch.Views
{
    [Register("FirstView")]
    public class FirstView : MvxTableViewController
    {
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            var source = new MvxStandardTableViewSource(TableView, "TitleText Strip(Name, 'ViewModel')");
            TableView.Source = source;

            var set = this.CreateBindingSet<FirstView, Core.ViewModels.FirstViewModel>();
            set.Bind(source).To(vm => vm.Tests);
            set.Bind(source).For(s => s.SelectionChangedCommand).To(vm => vm.GotoTestCommand);
            set.Apply();
        }
    }

    public abstract class TestViewController
         : MvxViewController
    {
        public override void ViewDidLoad()
        {
            View.BackgroundColor = UIColor.White;
            base.ViewDidLoad();

            var title = ViewModel.GetType().Name.Replace("ViewModel", string.Empty);
            Title = title;

            var explain = new UILabel(new RectangleF(10, 40, 300,  60))
                {
                    Text = ExplainText,
                    Lines = 0,
                };
            Add(explain);

            var nextButton = new UIButton(UIButtonType.RoundedRect);
            nextButton.Frame = new RectangleF(10,10,300,30);
            nextButton.SetTitle("Next test", UIControlState.Normal);
            Add(nextButton);

            var set = this.CreateBindingSet<TestViewController, TestViewModel>();
            set.Bind(nextButton).To(vm => vm.NextCommand);
            set.Apply();
        }

        protected virtual string ExplainText
        {
            get { return "I wonder what the test is?"; }
        }
    }

    public abstract class NotTestedTestViewController
         : TestViewController
    {
        protected override string ExplainText
        {
            get { return "Test not needed on iOS"; }
        }
    }

    [Register("DateTimeView")]
    public class DateTimeView : TestViewController
    {
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            var textView = new UITextField(new RectangleF(10, 90, 300, 30));
            Add(textView);
            var datePicker = new UIDatePicker();
            datePicker.Mode = UIDatePickerMode.Date;
            textView.InputView = datePicker;
            var label = new UILabel(new RectangleF(10, 120, 300, 30));
            Add(label);

            var set = this.CreateBindingSet<DateTimeView, DateTimeViewModel>();
            set.Bind(datePicker).To(vm => vm.Property);
            set.Bind(textView).To("Format('{0:dd MMM yyyy}', Property)");
            set.Bind(label).To("Format('{0:dd MMM yyyy}', Property)");            
            set.Apply();
        }

        protected override string ExplainText
        {
            get
            {
                return "Do both dates update?";
            }
        }
    }

    [Register("TimeView")]
    public class TimeView : TestViewController
    {
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            var textView = new UITextField(new RectangleF(10, 90, 300, 30));
            Add(textView);
            var datePicker = new UIDatePicker();
            datePicker.Mode = UIDatePickerMode.Time;
            textView.InputView = datePicker;
            var label = new UILabel(new RectangleF(10, 120, 300, 30));
            Add(label);

            var set = this.CreateBindingSet<TimeView, TimeViewModel>();
            set.Bind(datePicker).For("Time").To(vm => vm.Property);
            set.Bind(textView).To("Format('{0:t}', Property)");
            set.Bind(label).To("Format('{0:t}', Property)");
            set.Apply();
        }

        protected override string ExplainText
        {
            get
            {
                return "Do both times update?";
            }
        }
    }

    [Register("SpinnerView")]
    public class SpinnerView : TestViewController
    {
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            var picker = new UIPickerView();
            var pickerViewModel = new MvxPickerViewModel(picker);
            picker.Model = pickerViewModel;
            picker.ShowSelectionIndicator = true;
            var textView = new UITextField(new RectangleF(10, 100, 300, 30));
            Add(textView);
            textView.InputView = picker;
            var label = new UILabel(new RectangleF(10, 130, 300, 30));
            Add(label);

            var set = this.CreateBindingSet<SpinnerView, SpinnerViewModel>();
            set.Bind(pickerViewModel).For(p => p.SelectedItem).To(vm => vm.SelectedItem);
            set.Bind(pickerViewModel).For(p => p.ItemsSource).To(vm => vm.Items);
            set.Bind(textView).To(vm => vm.SelectedItem);
            set.Bind(label).To(vm => vm.SelectedItem);
            set.Apply();
        }

        protected override string ExplainText
        {
            get
            {
                return "Do both selections update?";
            }
        }
    }

    [Register("ListView")]
    public class ListView : TestViewController
    {
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            var table = new UITableView(new RectangleF(0, 130, 320, 300));
            Add(table);
            var source = new MvxStandardTableViewSource(table, "TitleText .");
            table.Source = source;

            var add = new UIButton(UIButtonType.RoundedRect);
            add.SetTitle("+", UIControlState.Normal);
            add.Frame = new RectangleF(10, 100, 140, 30);
            Add(add);

            var remove = new UIButton(UIButtonType.RoundedRect);
            remove.SetTitle("-", UIControlState.Normal);
            remove.Frame = new RectangleF(170, 100, 140, 30);
            Add(remove);

            var set = this.CreateBindingSet<ListView, ListViewModel>();
            set.Bind(source).To(vm => vm.Items);
            set.Bind(add).To(vm => vm.AddCommand);
            set.Bind(remove).To(vm => vm.RemoveCommand);
            set.Apply();
        }

        protected override string ExplainText
        {
            get
            {
                return "Does the list update?";
            }
        }
    }

    [Register("LinearLayoutView")]
    public class LinearLayoutView : NotTestedTestViewController
    {
        // not tested
    }

    [Register("FrameView")]
    public class FrameView : NotTestedTestViewController
    {
        // not tested
    }
    [Register("RelativeView")]
    public class RelativeView : NotTestedTestViewController
    {
        // not tested
    }

    [Register("ObservableCollectionView")]
    public class ObservableCollectionView : TestViewController
    {
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            var all = new UIButton(UIButtonType.RoundedRect);
            all.SetTitle("Replace All", UIControlState.Normal);
            all.Frame = new RectangleF(10, 100, 140, 30);
            Add(all);

            var each = new UIButton(UIButtonType.RoundedRect);
            each.SetTitle("Replace Each", UIControlState.Normal);
            each.Frame = new RectangleF(170, 100, 140, 30);
            Add(each);

            var makeNull = new UIButton(UIButtonType.RoundedRect);
            makeNull.SetTitle("Make Null", UIControlState.Normal);
            makeNull.Frame = new RectangleF(90, 130, 140, 30);
            Add(makeNull);

            var label1 = new UILabel(new RectangleF(10, 200, 300, 30));
            Add(label1);
            var label2 = new UILabel(new RectangleF(10, 230, 300, 30));
            Add(label2);
            var label3 = new UILabel(new RectangleF(10, 260, 300, 30));
            Add(label3);

            var set = this.CreateBindingSet<ObservableCollectionView, ObservableCollectionViewModel>();
            set.Bind(label1).To(vm => vm.Items[0]);
            set.Bind(label2).To(vm => vm.Items[1]);
            set.Bind(label3).To(vm => vm.Items[2]);
            set.Bind(all).To(vm => vm.ReplaceAllCommand);
            set.Bind(each).To(vm => vm.ReplaceEachCommand);
            set.Bind(makeNull).To(vm => vm.MakeNullCommand);
            set.Apply();
        }

        protected override string ExplainText
        {
            get
            {
                return "Do the indexed items update?";
            }
        }
    }

    [Register("ObservableDictionaryView")]
    public class ObservableDictionaryView : TestViewController
    {
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            var all = new UIButton(UIButtonType.RoundedRect);
            all.SetTitle("Replace All", UIControlState.Normal);
            all.Frame = new RectangleF(10, 100, 140, 30);
            Add(all);

            var each = new UIButton(UIButtonType.RoundedRect);
            each.SetTitle("Replace Each", UIControlState.Normal);
            each.Frame = new RectangleF(170, 100, 140, 30);
            Add(each);

            var makeNull = new UIButton(UIButtonType.RoundedRect);
            makeNull.SetTitle("Make Null", UIControlState.Normal);
            makeNull.Frame = new RectangleF(90, 130, 140, 30);
            Add(makeNull);

            var label1 = new UILabel(new RectangleF(10, 200, 300, 30));
            Add(label1);
            var label2 = new UILabel(new RectangleF(10, 230, 300, 30));
            Add(label2);
            var label3 = new UILabel(new RectangleF(10, 260, 300, 30));
            Add(label3);

            var set = this.CreateBindingSet<ObservableDictionaryView, ObservableDictionaryViewModel>();
            set.Bind(label1).To(vm => vm.Items["One"]);
            set.Bind(label2).To(vm => vm.Items["Two"]);
            set.Bind(label3).To(vm => vm.Items["Three"]);
            set.Bind(all).To(vm => vm.ReplaceAllCommand);
            set.Bind(each).To(vm => vm.ReplaceEachCommand);
            set.Bind(makeNull).To(vm => vm.MakeNullCommand);
            set.Apply();
        }

        protected override string ExplainText
        {
            get
            {
                return "Do the indexed items update?";
            }
        }
    }

    [Register("WithErrorsView")]
    public class WithErrorsView : TestViewController
    {
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();


            var label = new UILabel(new RectangleF(10, 100, 100, 30));
            label.Text = "Email";
            Add(label);
            var field = new UITextField(new RectangleF(110, 100, 200, 30));
            Add(field);
            var errorLabel = new UILabel(new RectangleF(10, 130, 300, 30));
            errorLabel.TextColor = UIColor.Red;
            errorLabel.TextAlignment = UITextAlignment.Right;
            Add(errorLabel);

            var label1 = new UILabel(new RectangleF(10, 160, 100, 30));
            label1.Text = "Accept";
            Add(label1);
            var ok = new UISwitch(new RectangleF(110, 160, 200, 30));
            Add(ok);
            var errorLabel1 = new UILabel(new RectangleF(10, 190, 300, 30));
            errorLabel1.TextColor = UIColor.Red;
            errorLabel1.TextAlignment = UITextAlignment.Right;
            Add(errorLabel1);

            var label2 = new UILabel(new RectangleF(10, 220, 100, 30));
            label2.Text = "Error count:";
            Add(label2);
            var errorLabel2 = new UILabel(new RectangleF(110, 220, 200, 30));
            errorLabel2.TextColor = UIColor.Red;
            errorLabel2.TextAlignment = UITextAlignment.Right;
            Add(errorLabel2);

            var set = this.CreateBindingSet<WithErrorsView, WithErrorsViewModel>();
            set.Bind(errorLabel).To(vm => vm.Errors["Email"]);
            set.Bind(errorLabel1).To(vm => vm.Errors["AcceptTerms"]);
            set.Bind(errorLabel2).To(vm => vm.Errors.Count);
            set.Bind(field).To(vm => vm.Email);
            set.Bind(ok).To(vm => vm.AcceptTerms);
            set.Apply();

            var gesture = new UITapGestureRecognizer(() => field.ResignFirstResponder());
            View.AddGestureRecognizer(gesture);
        }

        protected override string ExplainText
        {
            get
            {
                return "Do errors clear as expected?";
            }
        }
    }

    [Register("TextView")]
    public class TextView : TestViewController
    {
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            var label = new UILabel(new RectangleF(10, 100, 100, 30));
            label.Text = "Some text:";
            Add(label);
            var field = new UITextField(new RectangleF(110, 100, 200, 30));
            Add(field);
            var mirrorLabel = new UILabel(new RectangleF(110, 130, 200, 30));
            mirrorLabel.TextColor = UIColor.Blue;
            Add(mirrorLabel);


            var label1 = new UILabel(new RectangleF(10, 160, 100, 30));
            label1.Text = "A number:";
            Add(label1);
            var field1 = new UITextField(new RectangleF(110, 160, 200, 30));
            field1.KeyboardType = UIKeyboardType.DecimalPad;
            Add(field1);
            var mirrorLabel1 = new UILabel(new RectangleF(110, 190, 200, 30));
            mirrorLabel1.TextColor = UIColor.Blue;
            Add(mirrorLabel1);

            var set = this.CreateBindingSet<TextView, TextViewModel>();
            set.Bind(field).To(vm => vm.StringProperty);
            set.Bind(mirrorLabel).To(vm => vm.StringProperty);
            set.Bind(field1).To(vm => vm.DoubleProperty);
            set.Bind(mirrorLabel1).To(vm => vm.DoubleProperty);
            set.Apply();

            var gesture = new UITapGestureRecognizer(() =>
            {
                if (field.IsFirstResponder)
                    field.ResignFirstResponder();
                if (field1.IsFirstResponder)
                    field1.ResignFirstResponder();
            });
            View.AddGestureRecognizer(gesture);
        }

        protected override string ExplainText
        {
            get
            {
                return "Do the fields mirror as expected? Do decimals work?";
            }
        }
    }

    [Register("SeekView")]
    public class SeekView : TestViewController
    {
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            var label = new UILabel(new RectangleF(10, 100, 100, 30));
            label.Text = "Slide me:";
            Add(label);
            var seek = new UISlider(new RectangleF(110, 100, 200, 30));
            seek.MinValue = 0;
            seek.MaxValue = 100;
            Add(seek);
            var mirrorLabel = new UILabel(new RectangleF(110, 130, 200, 30));
            mirrorLabel.TextColor = UIColor.Blue;
            Add(mirrorLabel);

            var set = this.CreateBindingSet<SeekView, SeekViewModel>();
            set.Bind(seek).To(vm => vm.SeekProperty);
            set.Bind(mirrorLabel).To(vm => vm.SeekProperty);
            set.Apply();
        }

        protected override string ExplainText
        {
            get
            {
                return "Does the seek field mirror as expected?";
            }
        }
    }

    public class PersonUIView : MvxView
    {
        public PersonUIView()
        {
            var fname = new UITextField(new RectangleF(0, 0, 300, 20));
            Add(fname);
            var lname = new UITextField(new RectangleF(0, 20, 300, 20));
            Add(lname);
            this.DelayBind(() =>
                {
                    var set = this.CreateBindingSet<PersonUIView, ContainsSubViewModel.PersonViewModel>();
                    set.Bind(fname).To(vm => vm.FirstName);
                    set.Bind(lname).To(vm => vm.LastName);
                    set.Apply();
                });
        }

        public void ResignFirstResponders()
        {
            foreach (var subview in Subviews)
            {
                if (subview.IsFirstResponder)
                    subview.ResignFirstResponder();
            }
        }
    }

    [Register("ContainsSubView")]
    public class ContainsSubView : TestViewController
    {
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            var label = new UILabel(new RectangleF(10, 100, 300, 30));
            label.Text = "Do these update?";
            Add(label);
            var label2 = new UILabel(new RectangleF(10, 130, 300, 30));
            label2.BackgroundColor = UIColor.Yellow;
            Add(label2);
            var label3 = new UILabel(new RectangleF(10, 160, 300, 30));
            label3.BackgroundColor = UIColor.Cyan;
            Add(label3);

            var p1 = new PersonUIView();
            p1.Frame = new RectangleF(10, 200, 300, 40);
            p1.BackgroundColor = UIColor.Yellow;
            Add(p1);

            var p2 = new PersonUIView();
            p2.Frame = new RectangleF(10, 250, 300, 40);
            p2.BackgroundColor = UIColor.Cyan;
            Add(p2);

            var set = this.CreateBindingSet<ContainsSubView, ContainsSubViewModel>();
            set.Bind(label2).To("FirstPerson.FirstName + ' ' + FirstPerson.LastName");
            set.Bind(label3).To("SecondPerson.FirstName + ' ' + SecondPerson.LastName");
            set.Bind(p1).For(p => p.DataContext).To(vm => vm.FirstPerson);
            set.Bind(p2).For(p => p.DataContext).To(vm => vm.SecondPerson);
            set.Apply();

            var gesture = new UITapGestureRecognizer(() =>
            {
                p1.ResignFirstResponders();
                p2.ResignFirstResponders();
            });
            View.AddGestureRecognizer(gesture);
        }

        protected override string ExplainText
        {
            get
            {
                return "Do the people update the names as expected?";
            }
        }
    }

    [Register("ConvertThisView")]
    public class ConvertThisView : TestViewController
    {
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            var label = new UILabel(new RectangleF(10, 100, 100, 30));
            label.Text = "Enter a number:";
            Add(label);
            var field = new UITextField(new RectangleF(110, 100, 200, 30));
            field.KeyboardType = UIKeyboardType.NumberPad;
            Add(field);

            var label1 = new UILabel(new RectangleF(10, 130, 100, 30));
            label1.Text = "The number (minus 10) is:";
            Add(label1);
            var field1 = new UITextField(new RectangleF(110, 130, 200, 30));
            field1.KeyboardType = UIKeyboardType.NumberPad;
            Add(field1);

            var set = this.CreateBindingSet<ConvertThisView, ConvertThisViewModel>();
            set.Bind(field).To(vm => vm.Value).WithConversion("PlusTen");
            set.Bind(field1).To(vm => vm.Value);
            set.Apply();

            var gesture = new UITapGestureRecognizer(() =>
            {
                if (field.IsFirstResponder)
                    field.ResignFirstResponder();
                if (field1.IsFirstResponder)
                    field1.ResignFirstResponder();
            });
            View.AddGestureRecognizer(gesture);
        }

        protected override string ExplainText
        {
            get
            {
                return "Are the numbers both updating 10 apart?";
            }
        }
    }

    [Register("IfView")]
    public class IfView : TestViewController
    {
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            var label1 = new UILabel(new RectangleF(10, 90, 100, 30));
            Add(label1);
            var seek1 = new UISlider(new RectangleF(110, 90, 200, 30));
            seek1.MinValue = 0;
            seek1.MaxValue = 10;
            Add(seek1);

            var label2 = new UILabel(new RectangleF(10, 120, 100, 30));
            Add(label2);
            var seek2 = new UISlider(new RectangleF(110, 120, 200, 30));
            seek2.MinValue = 0;
            seek2.MaxValue = 10;
            Add(seek2);

            var labelA = new UILabel(new RectangleF(10, 150, 300, 20));
            Add(labelA);
            var labelB = new UILabel(new RectangleF(10, 170, 300, 20));
            Add(labelB);
            var labelC = new UILabel(new RectangleF(10, 190, 300, 20));
            Add(labelC);
            var labelD = new UILabel(new RectangleF(10, 210, 300, 20));
            Add(labelD);
            var labelE = new UILabel(new RectangleF(10, 230, 300, 20));
            Add(labelE);
            var labelF = new UILabel(new RectangleF(10, 250, 300, 20));
            Add(labelF);
            var labelG = new UILabel(new RectangleF(10, 270, 300, 20));
            Add(labelG);
            var labelH = new UILabel(new RectangleF(10, 290, 300, 20));
            Add(labelH);
            var labelI = new UILabel(new RectangleF(10, 310, 300, 20));
            Add(labelI);
            var labelJ = new UILabel(new RectangleF(10, 330, 300, 20));
            Add(labelJ);

            var set = this.CreateBindingSet<IfView, IfViewModel>();
            set.Bind(label1).To(vm => vm.TestVal1);
            set.Bind(seek1).To(vm => vm.TestVal1);
            set.Bind(label2).To(vm => vm.TestVal2);
            set.Bind(seek2).To(vm => vm.TestVal2);
            labelA.Text = "Smallest? (Second if equal)";
            set.Bind(labelB).SourceDescribed("If(TestVal1 < TestVal2, 'First', 'Second')");
            labelC.Text = "Largest? (Second if equal)";
            set.Bind(labelD).SourceDescribed("If(TestVal1 > TestVal2, 'First', 'Second')");
            labelE.Text = "Smallest? (First if equal)";
            set.Bind(labelF).SourceDescribed("If(TestVal1 <= TestVal2, 'First', 'Second')");
            labelG.Text = "Largest? (First if equal)";
            set.Bind(labelH).SourceDescribed("If(TestVal1 >= TestVal2, 'First', 'Second')");
            labelI.Text = "Equal?";
            set.Bind(labelJ).SourceDescribed("If(TestVal1 == TestVal2, 'Yes', 'No')");
            set.Apply();
        }

        protected override string ExplainText
        {
            get
            {
                return "Does the logic work as expected?";
            }
        }
    }

    [Register("MathsView")]
    public class MathsView : TestViewController
    {
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            var label1 = new UILabel(new RectangleF(10, 90, 100, 30));
            Add(label1);
            var seek1 = new UISlider(new RectangleF(110, 90, 200, 30));
            seek1.MinValue = 0;
            seek1.MaxValue = 10;
            Add(seek1);

            var label2 = new UILabel(new RectangleF(10, 120, 100, 30));
            Add(label2);
            var seek2 = new UISlider(new RectangleF(110, 120, 200, 30));
            seek2.MinValue = 0;
            seek2.MaxValue = 10;
            Add(seek2);

            var labelA = new UILabel(new RectangleF(10, 150, 300, 20));
            Add(labelA);
            var labelB = new UILabel(new RectangleF(10, 170, 300, 20));
            Add(labelB);
            var labelC = new UILabel(new RectangleF(10, 190, 300, 20));
            Add(labelC);
            var labelD = new UILabel(new RectangleF(10, 210, 300, 20));
            Add(labelD);
            var labelE = new UILabel(new RectangleF(10, 230, 300, 20));
            Add(labelE);
            var labelF = new UILabel(new RectangleF(10, 250, 300, 20));
            Add(labelF);
            var labelG = new UILabel(new RectangleF(10, 270, 300, 20));
            Add(labelG);
            var labelH = new UILabel(new RectangleF(10, 290, 300, 20));
            Add(labelH);
            var labelI = new UILabel(new RectangleF(10, 310, 300, 20));
            Add(labelI);
            var labelJ = new UILabel(new RectangleF(10, 330, 300, 20));
            Add(labelJ);

            var set = this.CreateBindingSet<MathsView, MathsViewModel>();
            set.Bind(label1).To(vm => vm.TestVal1);
            set.Bind(seek1).To(vm => vm.TestVal1);
            set.Bind(label2).To(vm => vm.TestVal2);
            set.Bind(seek2).To(vm => vm.TestVal2);
            labelA.Text = "Add";
            set.Bind(labelB).SourceDescribed("TestVal1 + '+' + TestVal2 + '=' + (TestVal1 + TestVal2)");
            labelC.Text = "Subtract";
            set.Bind(labelD).SourceDescribed("TestVal1 + '-' + TestVal2 + '=' + (TestVal1 - TestVal2)");
            labelE.Text = "Multiply";
            set.Bind(labelF).SourceDescribed("TestVal1 + '*' + TestVal2 + '=' + (TestVal1 * TestVal2)");
            labelG.Text = "Divide";
            set.Bind(labelH).SourceDescribed("TestVal1 + '/' + TestVal2 + '=' + (TestVal1 / TestVal2)");
            labelI.Text = "Modulo";
            set.Bind(labelJ).SourceDescribed("TestVal1 + '%' + TestVal2 + '=' + (TestVal1 % TestVal2)");
            set.Apply();
        }

        protected override string ExplainText
        {
            get
            {
                return "Does the maths work as expected?";
            }
        }
    }

    [Register("RadioGroupView")]
    public class RadioGroupView : NotTestedTestViewController
    {
        // not tested
    }
}