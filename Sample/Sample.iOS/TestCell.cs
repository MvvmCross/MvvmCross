using System;
using MvvmCross.Binding.BindingContext;
using Foundation;
using UIKit;
using MvvmCross.Binding.iOS.Views;
using SampleCore.ViewModels;

namespace Sample.iOS
{
    public partial class TestCell : MvxTableViewCell
    {
        public static readonly NSString Key = new NSString("TestCell");
        public static readonly UINib Nib;

        static TestCell()
        {
            Nib = UINib.FromName("TestCell", NSBundle.MainBundle);
        }

        protected TestCell(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.

            this.DelayBind(() =>
            {
                var set = this.CreateBindingSet<TestCell, TestItem>();
                set.Bind(TitleLabel).To(vm => vm.Title);
                set.Bind(TextView).To(vm => vm.Description);
                set.Apply();
            });

        }
    }
}
