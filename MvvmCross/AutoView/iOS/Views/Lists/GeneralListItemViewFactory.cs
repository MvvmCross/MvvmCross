// GeneralListItemViewFactory.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using MvvmCross.AutoView.iOS.Interfaces.Lists;

namespace MvvmCross.AutoView.iOS.Views.Lists
{
    using System.Collections.Generic;

    using Foundation;

    using iOS.Interfaces.Lists;
    using MvvmCross.Binding.Binders;
    using MvvmCross.Binding.Bindings;
    using MvvmCross.Platform;

    using UIKit;

    public class GeneralListItemViewFactory
        : IMvxLayoutListItemViewFactory

    {
        public UITableViewCell BuildView(NSIndexPath indexPath, object item, string cellId)
        {
            var bindings = this.GetBindingDescriptions();
            var style = this.GetCellStyle();
            var cell = new GeneralTableViewCell(bindings, style, new NSString(cellId));
            return cell;
        }

        public string LayoutName { get; set; }

        private Dictionary<string, string> _bindings;

        public Dictionary<string, string> Bindings
        {
            get { return this._bindings; }
            set
            {
                // clear the cached _cachedBindingDescriptions - these can be regenerated when required
                this._cachedBindingDescriptions = null;
                this._bindings = value;
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
            return this._cachedBindingDescriptions ?? (this._cachedBindingDescriptions = this.CreateBindingDescriptions());
        }

        private IEnumerable<MvxBindingDescription> CreateBindingDescriptions()
        {
            var parser = Mvx.Resolve<IMvxBindingDescriptionParser>();
            var toReturn = new List<MvxBindingDescription>();
            foreach (var binding in this.Bindings)
            {
                var bindingDescription = parser.ParseSingle(binding.Value);
                bindingDescription.TargetName = binding.Key;
                toReturn.Add(bindingDescription);
            }

            return toReturn;
        }
    }
}