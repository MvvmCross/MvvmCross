// MvxBaseBindableCollectionViewCell.cs
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
using System.Windows.Input;
using System.Drawing;

namespace Cirrious.MvvmCross.Binding.Touch.Views
{
	public class MvxBaseBindableCollectionViewCell
		: UICollectionViewCell
		, IMvxBindableView
		, IMvxServiceConsumer<IMvxBinder>
	{
		static MvxBaseBindableCollectionViewCell()
		{
#warning Not sure this is the best place for this initialisation
			Plugins.DownloadCache.PluginLoader.Instance.EnsureLoaded();
		}
		
		private readonly IList<IMvxUpdateableBinding> _bindings;
				
		public MvxBaseBindableCollectionViewCell(RectangleF frame, string bindingText)
			: base(frame)
		{
			_bindings = Binder.Bind(null, this, bindingText).ToList();
		}

		public MvxBaseBindableCollectionViewCell(RectangleF frame, IEnumerable<MvxBindingDescription> bindingDescriptions)
			: base(frame)
		{
			_bindings = Binder.Bind(null, this, bindingDescriptions).ToList();
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