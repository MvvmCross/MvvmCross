// GeneralListItemViewFactory.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System.Collections.Generic;
using Cirrious.CrossCore.Interfaces.ServiceProvider;
using Cirrious.MvvmCross.AutoView.Touch.Interfaces.Lists;
using Cirrious.MvvmCross.Binding.Interfaces;
using Cirrious.MvvmCross.Binding.Interfaces.Binders;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace Cirrious.MvvmCross.AutoView.Touch.Views.Lists
{
    public class GeneralListItemViewFactory
        : IMvxLayoutListItemViewFactory
          , IMvxServiceConsumer
    {
        public UITableViewCell BuildView(NSIndexPath indexPath, object item, string cellId)
        {
            var bindings = GetBindingDescriptions();
            var style = GetCellStyle();
            var cell = new GeneralTableViewCell(bindings, style, new NSString(cellId));
            return cell;
        }

        public string LayoutName { get; set; }

        private Dictionary<string, string> _bindings;

        public Dictionary<string, string> Bindings
        {
            get { return _bindings; }
            set
            {
                // clear the cached _cachedBindingDescriptions - these can be regenerated when required
                _cachedBindingDescriptions = null;
                _bindings = value;
            }
        }

        protected virtual UITableViewCellStyle GetCellStyle()
        {
#warning LayoutName is ignored currently
            return UITableViewCellStyle.Subtitle;
        }

        private IEnumerable<MvxBindingDescription> _cachedBindingDescriptions;

        protected virtual IEnumerable<MvxBindingDescription> GetBindingDescriptions()
        {
            if (_cachedBindingDescriptions == null)
            {
                _cachedBindingDescriptions = CreateBindingDescriptions();
            }
            return _cachedBindingDescriptions;
        }

        private IEnumerable<MvxBindingDescription> CreateBindingDescriptions()
        {
            var parser = this.GetService<IMvxBindingDescriptionParser>();
            var toReturn = new List<MvxBindingDescription>();
            foreach (var binding in Bindings)
            {
                var text = string.Format("{0} {1}", binding.Key, binding.Value);
                var bindingDescription = parser.ParseSingle(text);
                toReturn.Add(bindingDescription);
            }

            return toReturn;
        }
    }
}