using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Cirrious.MvvmCross.ViewModels
{
    public static class MvxNotifyPropertyChangedExtensions
    {
        private static TReturn RaiseAndSetIfChanged<T, TReturn>(T source, ref TReturn backingField, TReturn newValue, Action raiseAction)
            where T : IMvxNotifyPropertyChanged
        {
            if (EqualityComparer<TReturn>.Default.Equals(backingField, newValue) == false)
            {
                backingField = newValue;

                raiseAction();
            }

            return newValue;
        }

        public static TReturn RaiseAndSetIfChanged<T, TReturn>(this T source, ref TReturn backingField, TReturn newValue, Expression<Func<TReturn>> propertySelector)
            where T : IMvxNotifyPropertyChanged
        {
            return RaiseAndSetIfChanged(source, ref backingField, newValue, () => source.RaisePropertyChanged(propertySelector));
        }

        public static TReturn RaiseAndSetIfChanged<T, TReturn>(this T source, ref TReturn backingField, TReturn newValue, string propertyName)
            where T : IMvxNotifyPropertyChanged
        {
            return RaiseAndSetIfChanged(source, ref backingField, newValue, () => source.RaisePropertyChanged(propertyName));
        }

        public static TReturn RaiseAndSetIfChanged<T, TReturn>(this T source, ref TReturn backingField, TReturn newValue, PropertyChangedEventArgs args)
            where T : IMvxNotifyPropertyChanged
        {
            return RaiseAndSetIfChanged(source, ref backingField, newValue, () => source.RaisePropertyChanged(args));
        }
    }
}
