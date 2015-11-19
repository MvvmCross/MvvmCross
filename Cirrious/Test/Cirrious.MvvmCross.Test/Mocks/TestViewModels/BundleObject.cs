// BundleObject.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;

namespace Cirrious.MvvmCross.Test.Mocks.TestViewModels
{
    public class BundleObject
    {
        public string TheString1 { get; set; }
        public string TheString2 { get; set; }
        public bool TheBool1 { get; set; }
        public bool TheBool2 { get; set; }
        public int TheInt1 { get; set; }
        public int TheInt2 { get; set; }
        public Guid TheGuid1 { get; set; }
        public Guid TheGuid2 { get; set; }

        public override int GetHashCode()
        {
            return TheString1.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var rhs = obj as BundleObject;
            if (rhs == null)
                return false;

            return
                TheBool1 == rhs.TheBool1
                && TheBool2 == rhs.TheBool2
                && TheGuid1 == rhs.TheGuid1
                && TheGuid2 == rhs.TheGuid2
                && TheInt1 == rhs.TheInt1
                && TheInt2 == rhs.TheInt2
                && TheString1 == rhs.TheString1
                && TheString2 == rhs.TheString2;
        }
    }
}