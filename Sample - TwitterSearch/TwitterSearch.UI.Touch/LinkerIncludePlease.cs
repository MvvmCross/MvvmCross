using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MonoTouch.UIKit;

namespace TwitterSearch.UI.Touch
{
    // things in this class are only required in order to prevent the linker overoptimising!
    class LinkerIncludePlease
    {
        private void IncludeStuff()
        {
			UITextField textField = null;
			textField.Text = textField.Text + "";

			UIButton button = null;
			button.TouchUpInside += delegate(object sender, EventArgs e) {			
			};
        }

    }
}