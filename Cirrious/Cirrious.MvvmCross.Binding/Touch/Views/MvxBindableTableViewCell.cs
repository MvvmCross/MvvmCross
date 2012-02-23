using System.Collections.Generic;
using System.Linq;
using Cirrious.MvvmCross.Binding.Interfaces;
using Cirrious.MvvmCross.Binding.Touch.Interfaces.Views;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace Cirrious.MvvmCross.Binding.Touch.Views
{
    public class MvxBindableTableViewCell
        : UITableViewCell
          , IMvxBindableView
          , IMvxServiceConsumer<IMvxBinder>
    {
        private readonly IList<IMvxUpdateableBinding> _bindings;

        private IMvxBinder Binder
        {
            get { return this.GetService<IMvxBinder>(); }
        }

        public MvxBindableTableViewCell(string bindingText, UITableViewCellStyle cellStyle, NSString cellIdentifier)
            : base(cellStyle, cellIdentifier)
        {
            _bindings = Binder.Bind(null, this, bindingText).ToList();
        }

        public MvxBindableTableViewCell(IEnumerable<MvxBindingDescription> bindingDescriptions, UITableViewCellStyle cellStyle, NSString cellIdentifier)
            : base(cellStyle, cellIdentifier)
        {
            _bindings = Binder.Bind(null, this, bindingDescriptions).ToList();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                foreach (var binding in _bindings)
                {
                    binding.Dispose();
                }
                _bindings.Clear();
            }
            base.Dispose(disposing);
        }

        public void BindTo(object source)
        {
            foreach (var binding in _bindings)
            {
                binding.DataContext = source;
            }
        }

        public string TitleText
        {
            get { return TextLabel.Text; }
            set { TextLabel.Text = value; }
        }

        public string DetailText
        {
            get { return DetailTextLabel.Text; }
            set { DetailTextLabel.Text = value; }
        }
    }
}