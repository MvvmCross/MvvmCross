// INotifyDataErrorInfo.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System.Collections;

// ReSharper disable CheckNamespace

namespace System.ComponentModel
// ReSharper restore CheckNamespace
{
    public interface INotifyDataErrorInfo
    {
        event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        IEnumerable GetErrors(string propertyName);

        bool HasErrors { get; }
    }
}