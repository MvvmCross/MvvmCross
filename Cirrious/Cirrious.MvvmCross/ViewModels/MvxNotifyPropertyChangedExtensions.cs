using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace Cirrious.MvvmCross.ViewModels
{
    public static class MvxNotifyPropertyChangedExtensions
    {
        private static TReturn RaiseAndSetIfChanged<TSource, TReturn, TActionParameter>(
            TSource source, ref TReturn backingField, TReturn newValue,
            Action<TActionParameter> raiseAction,
            TActionParameter raiseActionParameter)
            where TSource : IMvxNotifyPropertyChanged
        {
            if (EqualityComparer<TReturn>.Default.Equals(backingField, newValue) == false)
            {
                backingField = newValue;
                raiseAction(raiseActionParameter);
            }

            return newValue;
        }

        public static TReturn RaiseAndSetIfChanged<TSource, TReturn>(this TSource source,
            ref TReturn backingField,
            TReturn newValue, Expression<Func<TReturn>> propertySelector)
            where TSource : IMvxNotifyPropertyChanged
        {
            return RaiseAndSetIfChanged(source, ref backingField, newValue, source.RaisePropertyChanged,
                propertySelector);
        }

        public static TReturn RaiseAndSetIfChanged<TSource, TReturn>(this TSource source,
            ref TReturn backingField,
            TReturn newValue, [CallerMemberName] string propertyName = "")
            where TSource : IMvxNotifyPropertyChanged
        {
            return RaiseAndSetIfChanged(source, ref backingField, newValue, source.RaisePropertyChanged,
                propertyName);
        }

        public static TReturn RaiseAndSetIfChanged<TSource, TReturn>(this TSource source,
            ref TReturn backingField,
            TReturn newValue, PropertyChangedEventArgs args)
            where TSource : IMvxNotifyPropertyChanged
        {
            return RaiseAndSetIfChanged(source, ref backingField, newValue, source.RaisePropertyChanged,
                args);
        }
    }
}
