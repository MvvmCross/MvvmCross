// DataErrorsChangedEventArgs.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace System.ComponentModel
// ReSharper restore CheckNamespace
{
    public sealed class DataErrorsChangedEventArgs : EventArgs
    {
        private readonly string _propertyName;

        public DataErrorsChangedEventArgs(string propertyName)
        {
            _propertyName = propertyName;
        }

        public string PropertyName
        {
            get { return _propertyName; }
        }
    }
}