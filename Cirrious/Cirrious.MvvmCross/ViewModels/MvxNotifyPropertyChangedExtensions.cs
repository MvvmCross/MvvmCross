using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;

namespace Cirrious.MvvmCross.ViewModels
{
    public static class MvxNotifyPropertyChangedExtensions
    {
        private static TReturn RaiseAndSetIfChanged<T, TReturn, TActionParameter>(
            T source, ref TReturn backingField, TReturn newValue, Action<TActionParameter> raiseAction, TActionParameter raiseActionParameter)
            where T : IMvxNotifyPropertyChanged
        {
            if (EqualityComparer<TReturn>.Default.Equals(backingField, newValue) == false)
            {
                backingField = newValue;
                raiseAction(raiseActionParameter);
            }

            return newValue;
        }

        public static TReturn RaiseAndSetIfChanged<T, TReturn>(this T source, ref TReturn backingField, TReturn newValue, Expression<Func<TReturn>> propertySelector)
            where T : IMvxNotifyPropertyChanged
        {
            return RaiseAndSetIfChanged(source, ref backingField, newValue, source.RaisePropertyChanged, propertySelector);
        }

        public static TReturn RaiseAndSetIfChanged<T, TReturn>(this T source, ref TReturn backingField, TReturn newValue, [CallerMemberName] string propertyName = "")
            where T : IMvxNotifyPropertyChanged
        {
            return RaiseAndSetIfChanged(source, ref backingField, newValue, source.RaisePropertyChanged, propertyName);
        }

        public static TReturn RaiseAndSetIfChanged<T, TReturn>(this T source, ref TReturn backingField, TReturn newValue, PropertyChangedEventArgs args)
            where T : IMvxNotifyPropertyChanged
        {
            return RaiseAndSetIfChanged(source, ref backingField, newValue, source.RaisePropertyChanged, args);
        }
    }
}
