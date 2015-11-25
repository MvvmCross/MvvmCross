// EmailAttachment.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System.IO;

namespace MvvmCross.Plugins.Email
{
    public class EmailAttachment
    {
        public string ContentType { get; set; }
        public string FileName { get; set; }
        public Stream Content { get; set; }
    }
}