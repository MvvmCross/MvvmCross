using System.Collections.Generic;
using Cirrious.MvvmCross.AutoView.Touch.Interfaces.Lists;
using Cirrious.MvvmCross.Binding.Interfaces;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;
using Cirrious.MvvmCross.Plugins.Json;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Cirrious.MvvmCross.Binding.Binders.Json;

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
            var json = this.GetService<IMvxJsonConverter>();
			var toReturn = new List<MvxBindingDescription>();
            foreach (var binding in Bindings)
            {
				var bindingDescription = json.DeserializeObject<MvxJsonBindingDescription>(binding.Value);
				var binder = this.GetService<IMvxJsonBindingDescriptionParser>();
				var description = binder.JsonBindingToBinding(binding.Key, bindingDescription);

                toReturn.Add(description);
            }

            return toReturn;
        }
    }
}

