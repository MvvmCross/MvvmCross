using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Foobar.Dialog.Core.Elements;
using Foobar.Dialog.Core.Lists;

namespace FooBar.Dialog.Droid.Lists
{
    public class GeneralListLayout : IListLayout
    {
        public GeneralListLayout()
        {
            ItemLayouts = new Dictionary<string, IListItemLayout>();
        }

        public Dictionary<string, IListItemLayout> ItemLayouts { get; private set; }
    }
}