#region Copyright
// <copyright file="MvxBasePropertyInfoSourceBinding.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com
#endregion

using System.ComponentModel;
using System.Reflection;
using Cirrious.MvvmCross.Interfaces.Platform.Diagnostics;

namespace Cirrious.MvvmCross.Binding.Bindings.Source
{
    public abstract class MvxBasePropertyInfoSourceBinding : MvxBaseSourceBinding
    {
        private readonly PropertyInfo _propertyInfo;
        private readonly string _propertyName;

        protected MvxBasePropertyInfoSourceBinding(object source, string propertyName)
            : base(source)
        {
            _propertyName = propertyName;

            if (Source == null)
            {
                MvxBindingTrace.Trace(
                    MvxTraceLevel.Warning,                 
                    "Unable to bind to source is null"
                    , propertyName);
                return;
            }

            _propertyInfo = source.GetType().GetProperty(propertyName);
            if (_propertyInfo == null)
            {
                MvxBindingTrace.Trace(
                    MvxTraceLevel.Warning,
                    "Unable to bind: source property source not found {0} on {1}"
                    , propertyName,
                    source.GetType().Name);
            }

            var sourceNotify = Source as INotifyPropertyChanged;
            if (sourceNotify != null)
                sourceNotify.PropertyChanged += new PropertyChangedEventHandler(SourcePropertyChanged);
        }

        protected string PropertyName { get { return _propertyName; } }
        protected PropertyInfo PropertyInfo { get { return _propertyInfo; } }

        protected override void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                var sourceNotify = Source as INotifyPropertyChanged;
                if (sourceNotify != null)
                    sourceNotify.PropertyChanged -= SourcePropertyChanged;
            }
        }

        private void SourcePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == PropertyName)
                OnBoundPropertyChanged();            
        }

        protected abstract void OnBoundPropertyChanged();
    }
}