// MvxBaseCollectionViewCell.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Collections.Generic;
using System.Drawing;
using Cirrious.MvvmCross.Binding.Interfaces;
using Cirrious.MvvmCross.Binding.Interfaces.BindingContext;
using Cirrious.MvvmCross.Binding.Touch.BindingContext;
using Cirrious.MvvmCross.Binding.Touch.Interfaces;
using MonoTouch.UIKit;

namespace Cirrious.MvvmCross.Binding.Touch.Views
{
    public class MvxBaseCollectionViewCell
        : UICollectionViewCell
        , IMvxBindableView
    {
        public IMvxBaseBindingContext<UIView> BindingContext { get; set; }

        public MvxBaseCollectionViewCell(string bindingText)
        {
            BindingContext = new MvxBindingContext(this, bindingText);
        }

        public MvxBaseCollectionViewCell(string bindingText, IntPtr handle)
            : base(handle)
        {
            BindingContext = new MvxBindingContext(this, bindingText);
        }

        public MvxBaseCollectionViewCell(string bindingText, RectangleF frame)
            : base(frame)
        {
            BindingContext = new MvxBindingContext(this, bindingText);
        }

        public MvxBaseCollectionViewCell(IEnumerable<MvxBindingDescription> bindingDescriptions, RectangleF frame)
            : base(frame)
        {
            BindingContext = new MvxBindingContext(this, bindingDescriptions);
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
#warning ClearAllBindings is better as Dispose?
                BindingContext.ClearAllBindings();
            }
            base.Dispose(disposing);
        }

        public object DataContext
        {
            get { return BindingContext.DataContext; }
            set { BindingContext.DataContext = value; }
        }
    }
}