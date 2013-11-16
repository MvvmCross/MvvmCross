// IMvxComposeEmailTaskEx.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System.Collections.Generic;

namespace Cirrious.MvvmCross.Plugins.Email
{
    public interface IMvxComposeEmailTaskEx
        : IMvxComposeEmailTask
    {
        void ComposeEmail(IEnumerable<string> to, IEnumerable<string> cc, string subject, string body, bool isHtml, IEnumerable<EmailAttachment> attachments);
        bool CanSendEmail { get; }
        bool CanSendAttachments { get; }
    }
}