// MvxBaseCollectionViewCell.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Collections.Generic;
using System.Linq;
using Cirrious.CrossCore.Interfaces.ServiceProvider;
using Cirrious.MvvmCross.Binding.Interfaces;
using Cirrious.MvvmCross.Binding.Touch.Interfaces;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.Windows.Input;
using System.Drawing;

namespace Cirrious.MvvmCross.Binding.Touch.Views
{
	public static class MvxBindableViewExtensionMethods
	{
		private static IMvxBinder GetBinder (this IMvxBindableView view)
		{
			return view.GetService<IMvxBinder>();
		}

        public static void DisposeBindings(this IMvxBindableView view)
        {
            if (view.Bindings == null)
                return;

            foreach (var binding in view.Bindings)
            {
                binding.Dispose();
            }
            view.Bindings.Clear();
        }
		public static void CreateFirstBindAction(this IMvxBindableView view, string bindingText)
		{
			view.CallOnNextDataContextChange = new Action(() => 
			{
				view.Bindings = view.GetBinder().Bind(view.DataContext, view, bindingText).ToList(); 
			});
		}
		
		public static void CreateFirstBindAction(this IMvxBindableView view, IEnumerable<MvxBindingDescription> bindingDescriptions)
		{
			view.CallOnNextDataContextChange = new Action(() => 
			{
				view.Bindings = view.GetBinder().Bind(view.DataContext, view, bindingDescriptions).ToList(); 
			});
		}

		public static void OnDataContextChanged(this IMvxBindableView view)
		{
			if (view.CallOnNextDataContextChange != null)
			{
				view.CallOnNextDataContextChange();
				view.CallOnNextDataContextChange = null;
				return;
			}
			
			if (view.Bindings == null)
				return;
			
			foreach (var binding in view.Bindings)
			{
				binding.DataContext = view.DataContext;
			}
		}
	}
    
}