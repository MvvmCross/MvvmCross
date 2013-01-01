// IMvxComposeEmailTask.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace Cirrious.MvvmCross.Plugins.Email
{
    public interface IMvxComposeEmailTask
    {
        void ComposeEmail(string to, string cc, string subject, string body, bool isHtml);
    }
}