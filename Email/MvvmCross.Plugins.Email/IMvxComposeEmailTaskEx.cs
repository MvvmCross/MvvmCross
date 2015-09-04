// IMvxComposeEmailTaskEx.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System.Collections.Generic;

namespace MvvmCross.Plugins.Email
{
    public interface IMvxComposeEmailTaskEx
        : IMvxComposeEmailTask
    {
        /// <summary>
        /// Compose an E-mail
        /// </summary>
        /// <param name="to"><see cref="IEnumerable{T}"/> of <see cref="string"/> containing the e-mail addresses 
        ///     you want to send the e-mail to</param>
        /// <param name="cc"><see cref="IEnumerable{T}"/> of <see cref="string"/> containing the e-mail addresses 
        ///     you want to send a carbon copy to</param>
        /// <param name="subject">Subject of the e-mail</param>
        /// <param name="body">Body of the e-mail</param>
        /// <param name="isHtml">Set to true if the <see cref="body"/> contains HTML content</param>
        /// <param name="attachments"><see cref="IEnumerable{T}"/> of <see cref="EmailAttachment"/> containing 
        ///     attachments</param>
        /// <param name="dialogTitle">Title of the dialog shown on Android</param>
        void ComposeEmail(IEnumerable<string> to, IEnumerable<string> cc = null, string subject = null,
            string body = null, bool isHtml = false, IEnumerable<EmailAttachment> attachments = null,
            string dialogTitle = null);
        bool CanSendEmail { get; }
        bool CanSendAttachments { get; }
    }
}