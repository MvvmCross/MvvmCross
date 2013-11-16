// IMvxComposeEmailTask.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System.Net.Mime;
using System.Collections.Generic;
using System.IO;

namespace Cirrious.MvvmCross.Plugins.Email
{
	public class Attachment
	{
		public ContentType ContentType { get; set; }
		public string FileName { get; set; }
		public Stream Content { get; set; }
	}

    public interface IMvxComposeEmailTask
    {
        void ComposeEmail(string to, string cc, string subject, string body, bool isHtml);
		void ComposeEmail(string[] to, string[] cc, string subject, string body, bool isHtml, List<EmailAttachment> attachments);
		bool CanSendEmail();		
    }
}
