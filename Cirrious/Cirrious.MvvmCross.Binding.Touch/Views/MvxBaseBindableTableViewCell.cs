// MvxBaseBindableTableViewCell.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
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
    public class MvxBaseBindableTableViewCell
        : UITableViewCell
          , IMvxBindableView
          , IMvxServiceConsumer<IMvxBinder>
    {
        static MvxBaseBindableTableViewCell()
        {
            Plugins.DownloadCache.PluginLoader.Instance.EnsureLoaded();
        }

        private readonly IList<IMvxUpdateableBinding> _bindings;

        public MvxBaseBindableTableViewCell(string bindingText, IntPtr handle)
            : base(handle)
        {
            _bindings = Binder.Bind(null, this, bindingText).ToList();
        }

        public MvxBaseBindableTableViewCell(IEnumerable<MvxBindingDescription> bindingDescriptions, IntPtr handle)
            : base(handle)
        {
            _bindings = Binder.Bind(null, this, bindingDescriptions).ToList();
        }

        public MvxBaseBindableTableViewCell(string bindingText, UITableViewCellStyle cellStyle, NSString cellIdentifier,
                                            UITableViewCellAccessory tableViewCellAccessory =
                                                UITableViewCellAccessory.None)
            : base(cellStyle, cellIdentifier)
        {
            Accessory = tableViewCellAccessory;
            _bindings = Binder.Bind(null, this, bindingText).ToList();
        }

        public MvxBaseBindableTableViewCell(IEnumerable<MvxBindingDescription> bindingDescriptions,
                                            UITableViewCellStyle cellStyle, NSString cellIdentifier,
                                            UITableViewCellAccessory tableViewCellAccessory =
                                                UITableViewCellAccessory.None)
            : base(cellStyle, cellIdentifier)
        {
            Accessory = tableViewCellAccessory;
            _bindings = Binder.Bind(null, this, bindingDescriptions).ToList();
        }

        // we seal Accessory here so that we can use it in the constructor - otherwise virtual issues.
        public override sealed UITableViewCellAccessory Accessory
        {
            get { return base.Accessory; }
            set { base.Accessory = value; }
        }

        private IMvxBinder Binder
        {
            get { return this.GetService(); }
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

        #region IMvxBindableView Members

        public void BindTo(object source)
        {
            foreach (var binding in _bindings)
            {
                binding.DataContext = source;
            }
        }

        #endregion
    }
}