// MvxCollectionViewCell.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Binding.iOS.Views
{
    using System;
    using System.Collections.Generic;

    using CoreGraphics;

    using MvvmCross.Binding.BindingContext;
    using MvvmCross.Binding.Bindings;

    using UIKit;

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

        /// <summary>
        /// Should fix choppy scrolling on ios8+ by preventing a layout pass when autolayout is already computed
        /// 
        /// iOS 8 provides a new self-sizing API for CollectionView and CollectionViewCells. It lets cells determine their own height, based on the content that they're about to load.
        /// preferredLayoutAttributesFittingAttributes: (on the cell)
        /// shouldLayoutAttributesFittingAttributes: (on the layout)
        /// invalidationContextForPreferredLayoutAttributes:withOriginalAttributes: (on the layout)
        /// </summary>
        public override UICollectionViewLayoutAttributes PreferredLayoutAttributesFittingAttributes(UICollectionViewLayoutAttributes layoutAttributes)
        {
            return layoutAttributes;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.BindingContext.ClearAllBindings();
            }
            base.Dispose(disposing);
        }

        public object DataContext
        {
            get { return this.BindingContext.DataContext; }
            set { this.BindingContext.DataContext = value; }
        }
    }
}