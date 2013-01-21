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
using System.Drawing;

namespace Cirrious.MvvmCross.Binding.Touch.Views
{
    public class MvxBaseBindableTableViewCell
        : UITableViewCell
        , IMvxBindableView
        , IMvxServiceConsumer
    {
        static MvxBaseBindableTableViewCell()
        {
#warning Not sure this is the best place for this initialisation
            Plugins.DownloadCache.PluginLoader.Instance.EnsureLoaded();
        }

        private IList<IMvxUpdateableBinding> _bindings;
        private Action<object> _callOnFirstBindAction; 

        public MvxBaseBindableTableViewCell(string bindingText)
            : base()
        {
            CreateFirstBindAction(bindingText);
        }

        public MvxBaseBindableTableViewCell(IEnumerable<MvxBindingDescription> bindingDescriptions)
            : base()
        {
            CreateFirstBindAction(bindingDescriptions);
        }

        public MvxBaseBindableTableViewCell(string bindingText, RectangleF frame)
            : base(frame)
        {
            CreateFirstBindAction(bindingText);
        }
        
        public MvxBaseBindableTableViewCell(IEnumerable<MvxBindingDescription> bindingDescriptions, RectangleF frame)
            : base(frame)
        {
            CreateFirstBindAction(bindingDescriptions);
        }

        public MvxBaseBindableTableViewCell(string bindingText, IntPtr handle)
            : base(handle)
        {
            CreateFirstBindAction(bindingText);
        }

        public MvxBaseBindableTableViewCell(IEnumerable<MvxBindingDescription> bindingDescriptions, IntPtr handle)
            : base(handle)
        {
            CreateFirstBindAction(bindingDescriptions);
        }

        public MvxBaseBindableTableViewCell(string bindingText, UITableViewCellStyle cellStyle, NSString cellIdentifier,
                                            UITableViewCellAccessory tableViewCellAccessory =
                                            UITableViewCellAccessory.None)
            : base(cellStyle, cellIdentifier)
        {
            Accessory = tableViewCellAccessory;
            CreateFirstBindAction(bindingText);
        }

        public MvxBaseBindableTableViewCell(IEnumerable<MvxBindingDescription> bindingDescriptions,
                                            UITableViewCellStyle cellStyle, NSString cellIdentifier,
                                            UITableViewCellAccessory tableViewCellAccessory =
                                            UITableViewCellAccessory.None)
            : base(cellStyle, cellIdentifier)
        {
            Accessory = tableViewCellAccessory;
            CreateFirstBindAction(bindingDescriptions);
        }

        private void CreateFirstBindAction(string bindingText)
        {
            _callOnFirstBindAction = new Action<Object>(source =>
            {
                _bindings = Binder.Bind(source, this, bindingText).ToList();
            });
        }

        private void CreateFirstBindAction(IEnumerable<MvxBindingDescription> bindingDescriptions)
        {
            _callOnFirstBindAction = new Action<Object>(source =>
            {
                _bindings = Binder.Bind(source, this, bindingDescriptions).ToList();
            });
        }

        // we seal Accessory here so that we can use it in the constructor - otherwise virtual issues.
        public override sealed UITableViewCellAccessory Accessory
        {
            get { return base.Accessory; }
            set { base.Accessory = value; }
        }

        private IMvxBinder Binder
        {
			get { return this.GetService<IMvxBinder>(); }
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
            if (_callOnFirstBindAction != null)
            {
                _callOnFirstBindAction(source);
                _callOnFirstBindAction = null;
                return;
            }

            if (_bindings == null)
                return;

            foreach (var binding in _bindings)
            {
                binding.DataContext = source;
            }
        }

        #endregion
    }
}