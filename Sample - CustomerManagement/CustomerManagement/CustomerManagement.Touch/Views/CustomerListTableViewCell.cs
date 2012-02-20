using Cirrious.MvvmCross.Binding.Interfaces;
using Cirrious.MvvmCross.Binding.Touch.Views;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace CustomerManagement.Touch
{
    public class CustomerListTableViewCell
        : MvxBindableTableViewCell
    {
        public const string BindingText = @"{'TitleText':{'Path':'Name'},'DetailText':{'Path':'Website'}}";

        public static readonly MvxBindingDescription[] BindingDescriptions 
            = new []
                  {
                      new MvxBindingDescription()
                          {
                              TargetName = "TitleText",
                              SourcePropertyPath = "Name"
                          },
                      new MvxBindingDescription()
                          {
                              TargetName = "DetailText",
                              SourcePropertyPath = "Website"
                          },
                  };

        public CustomerListTableViewCell(UITableViewCellStyle cellStyle, NSString cellIdentifier)
            : base(BindingDescriptions, cellStyle, cellIdentifier)
        {
        }
    }
}