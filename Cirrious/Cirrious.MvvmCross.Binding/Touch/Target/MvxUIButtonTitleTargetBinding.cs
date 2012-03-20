#region Copyright
// <copyright file="MvxUIButtonTitleTargetBinding.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com
#endregion

using System.Reflection;
using Cirrious.MvvmCross.Binding.Bindings.Target;
using Cirrious.MvvmCross.Binding.Interfaces;
using Cirrious.MvvmCross.Interfaces.Platform.Diagnostics;
using MonoTouch.UIKit;

namespace Cirrious.MvvmCross.Binding.Touch.Target
{
    public class MvxUIButtonTitleTargetBinding : MvxBaseTargetBinding
    {     
		UIButton _button;
		
        public MvxUIButtonTitleTargetBinding(UIButton button)
        {
            _button = button;
            if (_button == null)
            {
                MvxBindingTrace.Trace(MvxTraceLevel.Error,"Error - UIButton is null in MvxUIButtonTitleTargetBinding");
            }
        }

        public override MvxBindingMode DefaultMode
        {
            get
            {
                return MvxBindingMode.OneWay;
            }
        }
		
		public override System.Type TargetType 
		{
			get 
			{
				return typeof(string);
			}
		}
		
		public override void SetValue (object value)
		{
			if (_button == null)
				return;
			
			_button.SetTitle(value as string, UIControlState.Normal);
		}
    }
}