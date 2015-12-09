// GeneralListItemViewFactory.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.CrossCore;
using Cirrious.MvvmCross.AutoView.Touch.Interfaces.Lists;
using Cirrious.MvvmCross.Binding.Binders;
using Cirrious.MvvmCross.Binding.Bindings;
using Foundation;
using System.Collections.Generic;
using UIKit;

namespace Cirrious.MvvmCross.AutoView.Touch.Views.Lists
{
    public class GeneralListItemViewFactory
        : IMvxLayoutListItemViewFactory

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
            return _cachedBindingDescriptions ?? (_cachedBindingDescriptions = CreateBindingDescriptions());
        }

        private IEnumerable<MvxBindingDescription> CreateBindingDescriptions()
        {
            var parser = Mvx.Resolve<IMvxBindingDescriptionParser>();
            var toReturn = new List<MvxBindingDescription>();
            foreach (var binding in Bindings)
            {
                var bindingDescription = parser.ParseSingle(binding.Value);
                bindingDescription.TargetName = binding.Key;
                toReturn.Add(bindingDescription);
            }

            return toReturn;
        }
    }
}