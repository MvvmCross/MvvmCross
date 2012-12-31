#region Copyright

// <copyright file="MvxAdapterViewSelectedItemPositionTargetBinding.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
//  
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com

#endregion

using System;
using Android.Widget;
using Cirrious.MvvmCross.Binding.Interfaces;

namespace Cirrious.MvvmCross.Binding.Droid.Target
{
    public class MvxAdapterViewSelectedItemPositionTargetBinding : MvxBaseAndroidTargetBinding
    {
        private readonly AdapterView _adapterView;

        public MvxAdapterViewSelectedItemPositionTargetBinding(AdapterView adapterView)
        {
            _adapterView = adapterView;
            _adapterView.ItemSelected += AdapterViewOnItemSelected;
        }

        public override void SetValue(object value)
        {
            _adapterView.SetSelection((int) value);
        }

        private void AdapterViewOnItemSelected(object sender, AdapterView.ItemSelectedEventArgs itemSelectedEventArgs)
        {
            FireValueChanged(itemSelectedEventArgs.Position);
        }

        public override MvxBindingMode DefaultMode
        {
            get { return MvxBindingMode.TwoWay; }
        }

        public override Type TargetType
        {
            get { return typeof (Int32); }
        }

        protected override void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                if (_adapterView != null)
                {
                    _adapterView.ItemSelected -= AdapterViewOnItemSelected;
                }
            }
            base.Dispose(isDisposing);
        }
    }
}