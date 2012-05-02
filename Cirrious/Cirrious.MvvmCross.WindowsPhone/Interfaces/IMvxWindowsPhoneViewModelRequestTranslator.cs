#region Copyright
// <copyright file="IMvxWindowsPhoneViewModelRequestTranslator.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com
#endregion

using System;
using Cirrious.MvvmCross.Views;

namespace Cirrious.MvvmCross.WindowsPhone.Interfaces
{
    public interface IMvxWindowsPhoneViewModelRequestTranslator
    {
        MvxShowViewModelRequest GetRequestFromXamlUri(Uri viewUri);
        Uri GetXamlUriFor(MvxShowViewModelRequest request);
    }
}