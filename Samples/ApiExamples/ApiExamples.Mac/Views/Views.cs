using System;
using Cirrious.MvvmCross.Mac.Views;
using MonoMac.Foundation;
using MonoMac.AppKit;
using System.Drawing;
using Cirrious.MvvmCross.Binding.BindingContext;
using Cirrious.MvvmCross.Binding.Mac.Views;
using ApiExamples.Core.ViewModels;

namespace ApiExamples.Mac
{
	public partial class FirstViewController : MvxViewController
	{
		public FirstViewController () : base ()
		{
			Initialize ();
		}

		// Shared initialization code
		void Initialize ()
		{
		}

		NSTableView _tableView;

		public override void LoadView ()
		{
			_tableView = new NSTableView(new RectangleF(0,100,320,500));
			View = _tableView;
			ViewDidLoad ();
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			var column = new MvxTableColumn ();
			column.Identifier = "First";
			column.BindingText = "Text Name";
			column.HeaderCell = new NSCell ("Example");
			_tableView.AddColumn (column);

			var source = new MvxTableViewSource (_tableView);
			_tableView.Source = source;

			var set = this.CreateBindingSet<FirstViewController, FirstViewModel> ();
			set.Bind (source).For(v => v.ItemsSource).To (vm => vm.Tests);
			set.Bind (source).For (v => v.SelectionChangedCommand).To (vm => vm.GotoTestCommand);
			set.Apply ();
		}
	}

	public static class RectangleExtensionMethods
	{
		public const int MaxY = 450;

		public static RectangleF Upside(this RectangleF original)
		{
			return new RectangleF (original.X, MaxY - original.Y, original.Width, original.Height);
		}
	}

	public abstract class TestViewController
		: MvxViewController
	{
		protected void Add(NSView view)
		{
			View.AddSubview (view);
		}

		public override void LoadView ()
		{
			var view = new NSView(new RectangleF(0,100,320,500));
			//view.BackgroundColor = NSColor.Gray;
			View = view;
			ViewDidLoad ();
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			var title = ViewModel.GetType().Name.Replace("ViewModel", string.Empty);
			Title = title;

			var explain = new SimpleLabel(new RectangleF(10, 50, 300,  20).Upside())
			{
				Text = ExplainText,
			};
			Add(explain);

			var nextButton = new NSButton();
			nextButton.Frame = new RectangleF(10,20,300,30).Upside();
			nextButton.Title = "Next test";
			Add(nextButton);

			var titleLabel = new SimpleLabel(new RectangleF(10,-10,300,30).Upside());
			titleLabel.Text = Title;
			Add(titleLabel);

			var set = this.CreateBindingSet<TestViewController, TestViewModel>();
			set.Bind(nextButton).For("Activated").To(vm => vm.NextCommand);
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
			get { return "Test not needed (or done yet) on Mac"; }
		}
	}

	[Register("DateTimeView")]
	public class DateTimeView : NotTestedTestViewController
	{
		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			var label = new NSTextField(new RectangleF(10, 90, 300, 30).Upside());
			Add(label);
			var datePicker = new NSDatePicker(new RectangleF(10, 120, 300, 75).Upside());
			datePicker.DatePickerElements = NSDatePickerElementFlags.YearMonthDateDay;
			datePicker.DatePickerMode =  NSDatePickerMode.Single;
			Add (datePicker);

			var set = this.CreateBindingSet<DateTimeView, DateTimeViewModel>();
			set.Bind(datePicker).For("Date").To(vm => vm.Property);
			set.Bind(label).To("Format('{0:dd MMM yyyy}', Property)");
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

	[Register("TimeView")]
	public class TimeView : NotTestedTestViewController
	{
		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			var label = new NSTextField(new RectangleF(10, 90, 300, 30).Upside());
			Add(label);
			var datePicker = new NSDatePicker(new RectangleF(10, 120, 300, 75).Upside());
			datePicker.DatePickerElements = NSDatePickerElementFlags.HourMinute;
			datePicker.DatePickerMode =  NSDatePickerMode.Single;
			Add (datePicker);

			var set = this.CreateBindingSet<TimeView, TimeViewModel>();
			set.Bind(datePicker).For("Time").To(vm => vm.Property);
			set.Bind(label).To("Format('{0:h\\\\:mm}', Property)");
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
	public class SpinnerView : NotTestedTestViewController
	{
	}

	[Register("ListView")]
	public class ListView : NotTestedTestViewController
	{
		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			var tableView = new NSTableView(new RectangleF(10, 400, 300, 300).Upside());
			Add(tableView);

			var column = new MvxTableColumn ();
			column.Identifier = "First";
			column.BindingText = "Text .";
			column.HeaderCell = new NSCell ("Example");
			tableView.AddColumn (column);

			var source = new MvxTableViewSource (tableView);
			tableView.Source = source;

			var add = new NSButton();
			add.Title = "+";
			add.Frame = new RectangleF(10, 100, 140, 30).Upside();
			Add(add);

			var remove = new NSButton();
			remove.Title = "-";
			remove.Frame = new RectangleF(170, 100, 140, 30).Upside();
			Add(remove);

			var set = this.CreateBindingSet<ListView, ListViewModel>();
			set.Bind(source).For(v => v.ItemsSource).To(vm => vm.Items);
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
	public class ObservableCollectionView : NotTestedTestViewController
	{
		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			var all = new NSButton();
			all.Title = "Replace All";
			all.Frame = new RectangleF(10, 100, 140, 30).Upside();
			Add(all);

			var each = new NSButton();
			each.Title = "Replace Each";
			each.Frame = new RectangleF(170, 100, 140, 30).Upside();
			Add(each);

			var makeNull = new NSButton();
			makeNull.Title = "Make Null";
			makeNull.Frame = new RectangleF(90, 130, 140, 30).Upside();
			Add(makeNull);

			var label1 = new SimpleLabel(new RectangleF(10, 200, 300, 30).Upside());
			Add(label1);
			var label2 = new SimpleLabel(new RectangleF(10, 230, 300, 30).Upside());
			Add(label2);
			var label3 = new SimpleLabel(new RectangleF(10, 260, 300, 30).Upside());
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
	public class ObservableDictionaryView : NotTestedTestViewController
	{
		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			var all = new NSButton();
			all.Title = "Replace All";
			all.Frame = new RectangleF(10, 100, 140, 30).Upside();
			Add(all);

			var each = new NSButton();
			each.Title = "Replace Each";
			each.Frame = new RectangleF(170, 100, 140, 30).Upside();
			Add(each);

			var makeNull = new NSButton();
			makeNull.Title = "Make Null";
			makeNull.Frame = new RectangleF(90, 130, 140, 30).Upside();
			Add(makeNull);

			var label1 = new SimpleLabel(new RectangleF(10, 200, 300, 30).Upside());
			Add(label1);
			var label2 = new SimpleLabel(new RectangleF(10, 230, 300, 30).Upside());
			Add(label2);
			var label3 = new SimpleLabel(new RectangleF(10, 260, 300, 30).Upside());
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
	public class WithErrorsView : NotTestedTestViewController
	{
		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			var label = new SimpleLabel(new RectangleF(10, 100, 100, 30).Upside());
			label.Text = "Email";
			Add(label);
			var field = new NSTextField(new RectangleF(110, 100, 200, 30).Upside());
			Add(field);
			var errorLabel = new SimpleLabel(new RectangleF(10, 130, 300, 30).Upside());
			errorLabel.TextColor = NSColor.Red;
			errorLabel.Alignment = NSTextAlignment.Right;
			Add(errorLabel);

			var label1 = new SimpleLabel(new RectangleF(10, 160, 100, 30).Upside());
			label1.Text = "Accept";
			Add(label1);
			var ok = new NSButton(new RectangleF(110, 160, 200, 30).Upside());
			ok.SetButtonType (NSButtonType.Switch);
			Add(ok);

			var errorLabel1 = new SimpleLabel(new RectangleF(10, 190, 300, 30).Upside());
			errorLabel1.TextColor = NSColor.Red;
			errorLabel1.Alignment = NSTextAlignment.Right;
			Add(errorLabel1);

			var label2 = new SimpleLabel(new RectangleF(10, 220, 100, 30).Upside());
			label2.Text = "Error count:";
			Add(label2);
			var errorLabel2 = new SimpleLabel(new RectangleF(110, 220, 200, 30).Upside());
			errorLabel2.TextColor = NSColor.Red;
			errorLabel2.Alignment = NSTextAlignment.Right;
			Add(errorLabel2);

			var set = this.CreateBindingSet<WithErrorsView, WithErrorsViewModel>();
			set.Bind(errorLabel).To(vm => vm.Errors["Email"]);
			set.Bind(errorLabel1).To(vm => vm.Errors["AcceptTerms"]);
			set.Bind(errorLabel2).To(vm => vm.Errors.Count);
			set.Bind(field).To(vm => vm.Email);
			set.Bind(ok).For(v => v.State).To(vm => vm.AcceptTerms);
			set.Apply();
		}

		protected override string ExplainText
		{
			get
			{
				return "Do errors clear as expected?";
			}
		}
	}

	public class SimpleLabel : NSTextField
	{
		public SimpleLabel (RectangleF frame)
			: base(frame)
		{
			Editable = false;
			Bordered = false;
			BackgroundColor = NSColor.Clear;
		}

		public string Text {
			get { return StringValue; }
			set { StringValue = value; }
		}
	}

	[Register("TextView")]
	public class TextView : TestViewController
	{
		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			var label = new SimpleLabel(new RectangleF(10, 100, 100, 30).Upside());
			label.StringValue = "Some text:";
			Add(label);
			var field = new NSTextField(new RectangleF(110, 100, 200, 30).Upside());
			Add(field);
			var mirrorLabel = new SimpleLabel(new RectangleF(110, 130, 200, 30).Upside());
			mirrorLabel.TextColor = NSColor.Blue;
			Add(mirrorLabel);


			var label1 = new SimpleLabel(new RectangleF(10, 160, 100, 30).Upside());
			label1.Text = "A number:";
			Add(label1);
			var field1 = new NSTextField(new RectangleF(110, 160, 200, 30).Upside());
			Add(field1);
			var mirrorLabel1 = new SimpleLabel(new RectangleF(110, 190, 200, 30).Upside());
			mirrorLabel1.TextColor = NSColor.Blue;
			Add(mirrorLabel1);

			var set = this.CreateBindingSet<TextView, TextViewModel>();
			set.Bind(field).To(vm => vm.StringProperty);
			set.Bind(mirrorLabel).To(vm => vm.StringProperty);
			set.Bind(field1).To(vm => vm.DoubleProperty);
			set.Bind(mirrorLabel1).To(vm => vm.DoubleProperty);
			set.Apply();
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
	public class SeekView : NotTestedTestViewController
	{
		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			var label = new SimpleLabel(new RectangleF(10, 100, 100, 30).Upside());
			label.Text = "Slide me:";
			Add(label);
			var seek = new NSSlider(new RectangleF(110, 100, 200, 30).Upside());
			seek.MinValue = 0;
			seek.MaxValue = 100;
			Add(seek);
			var mirrorLabel = new SimpleLabel(new RectangleF(110, 130, 200, 30).Upside());
			mirrorLabel.TextColor = NSColor.Blue;
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

	[Register("ContainsSubView")]
	public class ContainsSubView : NotTestedTestViewController
	{
	}

	[Register("ConvertThisView")]
	public class ConvertThisView : NotTestedTestViewController
	{
		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			var label = new SimpleLabel(new RectangleF(10, 100, 100, 30).Upside());
			label.Text = "Enter a number:";
			Add(label);
			var field = new NSTextField(new RectangleF(110, 100, 200, 30).Upside());
			Add(field);

			var label1 = new SimpleLabel(new RectangleF(10, 130, 100, 30).Upside());
			label1.Text = "The number (minus 10) is:";
			Add(label1);
			var field1 = new NSTextField(new RectangleF(110, 130, 200, 30).Upside());
			Add(field1);

			var set = this.CreateBindingSet<ConvertThisView, ConvertThisViewModel>();
			set.Bind(field).To(vm => vm.Value).WithConversion("PlusTen");
			set.Bind(field1).To(vm => vm.Value);
			set.Apply();
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
	public class IfView : NotTestedTestViewController
	{
		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			var label1 = new SimpleLabel(new RectangleF(10, 90, 100, 30).Upside());
			Add(label1);
			var seek1 = new NSSlider(new RectangleF(110, 90, 200, 30).Upside());
			seek1.MinValue = 0;
			seek1.MaxValue = 10;
			Add(seek1);

			var label2 = new SimpleLabel(new RectangleF(10, 120, 100, 30).Upside());
			Add(label2);
			var seek2 = new NSSlider(new RectangleF(110, 120, 200, 30).Upside());
			seek2.MinValue = 0;
			seek2.MaxValue = 10;
			Add(seek2);

			var labelA = new SimpleLabel(new RectangleF(10, 150, 300, 20).Upside());
			Add(labelA);
			var labelB = new SimpleLabel(new RectangleF(10, 170, 300, 20).Upside());
			Add(labelB);
			var labelC = new SimpleLabel(new RectangleF(10, 190, 300, 20).Upside());
			Add(labelC);
			var labelD = new SimpleLabel(new RectangleF(10, 210, 300, 20).Upside());
			Add(labelD);
			var labelE = new SimpleLabel(new RectangleF(10, 230, 300, 20).Upside());
			Add(labelE);
			var labelF = new SimpleLabel(new RectangleF(10, 250, 300, 20).Upside());
			Add(labelF);
			var labelG = new SimpleLabel(new RectangleF(10, 270, 300, 20).Upside());
			Add(labelG);
			var labelH = new SimpleLabel(new RectangleF(10, 290, 300, 20).Upside());
			Add(labelH);
			var labelI = new SimpleLabel(new RectangleF(10, 310, 300, 20).Upside());
			Add(labelI);
			var labelJ = new SimpleLabel(new RectangleF(10, 330, 300, 20).Upside());
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
	public class MathsView : NotTestedTestViewController
	{
		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			var label1 = new SimpleLabel(new RectangleF(10, 90, 100, 30).Upside());
			Add(label1);
			var seek1 = new NSSlider(new RectangleF(110, 90, 200, 30).Upside());
			seek1.MinValue = 0;
			seek1.MaxValue = 10;
			Add(seek1);

			var label2 = new SimpleLabel(new RectangleF(10, 120, 100, 30).Upside());
			Add(label2);
			var seek2 = new NSSlider(new RectangleF(110, 120, 200, 30).Upside());
			seek2.MinValue = 0;
			seek2.MaxValue = 10;
			Add(seek2);

			var labelA = new SimpleLabel(new RectangleF(10, 150, 300, 20).Upside());
			Add(labelA);
			var labelB = new SimpleLabel(new RectangleF(10, 170, 300, 20).Upside());
			Add(labelB);
			var labelC = new SimpleLabel(new RectangleF(10, 190, 300, 20).Upside());
			Add(labelC);
			var labelD = new SimpleLabel(new RectangleF(10, 210, 300, 20).Upside());
			Add(labelD);
			var labelE = new SimpleLabel(new RectangleF(10, 230, 300, 20).Upside());
			Add(labelE);
			var labelF = new SimpleLabel(new RectangleF(10, 250, 300, 20).Upside());
			Add(labelF);
			var labelG = new SimpleLabel(new RectangleF(10, 270, 300, 20).Upside());
			Add(labelG);
			var labelH = new SimpleLabel(new RectangleF(10, 290, 300, 20).Upside());
			Add(labelH);
			var labelI = new SimpleLabel(new RectangleF(10, 310, 300, 20).Upside());
			Add(labelI);
			var labelJ = new SimpleLabel(new RectangleF(10, 330, 300, 20).Upside());
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

