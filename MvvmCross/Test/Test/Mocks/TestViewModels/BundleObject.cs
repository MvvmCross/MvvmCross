// BundleObject.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Test.Mocks.TestViewModels
{
    using System;

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
            return this.TheString1.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var rhs = obj as BundleObject;
            if (rhs == null)
                return false;

            return
                this.TheBool1 == rhs.TheBool1
                && this.TheBool2 == rhs.TheBool2
                && this.TheGuid1 == rhs.TheGuid1
                && this.TheGuid2 == rhs.TheGuid2
                && this.TheInt1 == rhs.TheInt1
                && this.TheInt2 == rhs.TheInt2
                && this.TheString1 == rhs.TheString1
                && this.TheString2 == rhs.TheString2;
        }
    }
}