// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace MvvmCross.Plugin.Email
{
    public interface IMvxComposeEmailTask
    {
        /// <summary>
        /// Compose an E-mail
        /// </summary>
        /// <param name="to">Who the e-mail is to</param>
        /// <param name="cc">Who you want to send a carbon copy to</param>
        /// <param name="subject">Subject of the e-mail</param>
        /// <param name="body">Body of the e-mail</param>
        /// <param name="isHtml">Set to true if the <see cref="body"/> contains HTML content</param>
        /// <param name="dialogTitle">Title of the dialog shown on Android</param>
        void ComposeEmail(string to, string cc = null, string subject = null, string body = null,
            bool isHtml = false, string dialogTitle = null);
    }
}
