#region Copyright
// <copyright file="MvxLocationError.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com
#endregion
namespace Cirrious.MvvmCross.Interfaces.Platform.Location
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