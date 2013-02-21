// MvxBaseCollectionViewCell.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Collections.Generic;
using System.Linq;
using Cirrious.MvvmCross.Binding.Interfaces;
using Cirrious.MvvmCross.Binding.Touch.Interfaces;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.Windows.Input;
using System.Drawing;

namespace Cirrious.MvvmCross.Binding.Touch.Views
{	
	public class MvxBaseCollectionViewCell
        : UICollectionViewCell
		, IMvxBindableView
    {
        public IList<IMvxUpdateableBinding> Bindings {get;set;}
		public Action CallOnNextDataContextChange { get; set; }

        public MvxBaseCollectionViewCell (string bindingText)
        {
            this.CreateFirstBindAction(bindingText);
        }

        public MvxBaseCollectionViewCell(string bindingText, IntPtr handle)
            : base(handle)
        {
            this.CreateFirstBindAction(bindingText);
        }

        public MvxBaseCollectionViewCell(string bindingText, RectangleF frame)
            : base(frame)
        {
            this.CreateFirstBindAction(bindingText);
        }

        public MvxBaseCollectionViewCell(IEnumerable<MvxBindingDescription> bindingDescriptions, RectangleF frame)
            : base(frame)
        {
            this.CreateFirstBindAction(bindingDescriptions);
        }

        [Obsolete("Please reverse the parameter order")]
        public MvxBaseCollectionViewCell(IntPtr handle, string bindingText)
            : this(bindingText, handle)
        {
        }

        [Obsolete("Please reverse the parameter order")]
        public MvxBaseCollectionViewCell(RectangleF frame, string bindingText)
            : this(bindingText, frame)
        {
        }

        [Obsolete("Please reverse the parameter order")]
        public MvxBaseCollectionViewCell(RectangleF frame, IEnumerable<MvxBindingDescription> bindingDescriptions)
            : this(bindingDescriptions, frame)
        {
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.DisposeBindings();
            }
            base.Dispose(disposing);
        }
        
		private object _dataContext;
		public object DataContext { 
			get {
				return _dataContext;
			}
			set {
				if (_dataContext == value
				    && CallOnNextDataContextChange == null)
					return;

				_dataContext = value;
				this.OnDataContextChanged();
			}
		}
    }
}