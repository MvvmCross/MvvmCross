// MvxLocationError.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Plugins.Location
{
    public class MvxLocationError
    {
        public MvxLocationError(MvxLocationErrorCode code)
        {
            Code = code;
        }

        public MvxLocationErrorCode Code { get; private set; }
    }
}