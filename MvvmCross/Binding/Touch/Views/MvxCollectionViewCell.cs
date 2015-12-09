// MvxCollectionViewCell.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.MvvmCross.Binding.BindingContext;
using Cirrious.MvvmCross.Binding.Bindings;
using CoreGraphics;
using System;
using System.Collections.Generic;
using UIKit;

namespace Cirrious.MvvmCross.Binding.Touch.Views
{
    public class MvxCollectionViewCell
        : UICollectionViewCell
          , IMvxBindable
    {
        public IMvxBindingContext BindingContext { get; set; }

        public MvxCollectionViewCell(string bindingText)
        {
            this.CreateBindingContext(bindingText);
        }

        public MvxCollectionViewCell(IntPtr handle)
            : base(handle)
        {
            this.CreateBindingContext();
        }

        public MvxCollectionViewCell(string bindingText, IntPtr handle)
            : base(handle)
        {
            this.CreateBindingContext(bindingText);
        }

        public MvxCollectionViewCell(CGRect frame)
            : base(frame)
        {
            this.CreateBindingContext();
        }

        public MvxCollectionViewCell(string bindingText, CGRect frame)
            : base(frame)
        {
            this.CreateBindingContext(bindingText);
        }

        public MvxCollectionViewCell(IEnumerable<MvxBindingDescription> bindingDescriptions, CGRect frame)
            : base(frame)
        {
            this.CreateBindingContext(bindingDescriptions);
        }

        [Obsolete("Please reverse the parameter order")]
        public MvxCollectionViewCell(IntPtr handle, string bindingText)
            : this(bindingText, handle)
        {
        }

        [Obsolete("Please reverse the parameter order")]
        public MvxCollectionViewCell(CGRect frame, string bindingText)
            : this(bindingText, frame)
        {
        }

        [Obsolete("Please reverse the parameter order")]
        public MvxCollectionViewCell(CGRect frame, IEnumerable<MvxBindingDescription> bindingDescriptions)
            : this(bindingDescriptions, frame)
        {
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
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