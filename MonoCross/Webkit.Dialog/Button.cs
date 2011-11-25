using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;

namespace Webkit.Dialog
{
    public class ButtonStyle
    {
        public const string Action = "iBAction ";
        public const string Cancel = "iBCancel ";
        public const string Classic = "iBClassic ";
        public const string Warn = "iBWarn ";
    }

    public abstract class Button
    {
        public abstract Control Control { get; }
        public string Caption { get; set; }
        public string Url { get; set; }
        public string ImageUrl { get; set; }
        public string Style { get; set; }

        public Button(string caption, string url)
        {
            Caption = caption;
            Url = url;
            Style = ButtonStyle.Classic;
        }
    }
}
