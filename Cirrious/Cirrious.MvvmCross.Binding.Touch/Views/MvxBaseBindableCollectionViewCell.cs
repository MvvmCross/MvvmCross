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
		, IMvxServiceConsumer
	{
		static MvxBaseBindableCollectionViewCell()
		{
#warning Not sure this is the best place for this initialisation
			Plugins.DownloadCache.PluginLoader.Instance.EnsureLoaded();
		}
		
		private IList<IMvxUpdateableBinding> _bindings;
	    private Action<object> _callOnFirstBindAction; 
				
		public MvxBaseBindableCollectionViewCell (string bindingText)
		{
		    CreateFirstBindAction(bindingText);
		}

	    public MvxBaseBindableCollectionViewCell(IntPtr handle, string bindingText)
			: base(handle)
		{
            CreateFirstBindAction(bindingText);
        }

		public MvxBaseBindableCollectionViewCell(RectangleF frame, string bindingText)
			: base(frame)
		{
            CreateFirstBindAction(bindingText);
        }

		public MvxBaseBindableCollectionViewCell(RectangleF frame, IEnumerable<MvxBindingDescription> bindingDescriptions)
			: base(frame)
		{
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