// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace MvvmCross.ViewModels
{
    public static class MvxNotifyPropertyChangedExtensions
    {
        private static bool RaiseIfChanging<TSource, TReturn>(
            TSource source, TReturn backingField, TReturn newValue,
            Func<bool> raiseAction)
            where TSource : IMvxNotifyPropertyChanged
        {
            if (EqualityComparer<TReturn>.Default.Equals(backingField, newValue) == false)
            {
                return raiseAction();
            }

            return false;
        }

        public static bool RaiseIfChanging<TSource, TReturn>(this TSource source,
            TReturn backingField,
            TReturn newValue, Expression<Func<TReturn>> propertySelector)
            where TSource : IMvxNotifyPropertyChanged
        {
            return RaiseIfChanging(source, backingField, newValue, () => source.RaisePropertyChanging(newValue, propertySelector));
        }

        public static bool RaiseIfChanging<TSource, TReturn>(this TSource source,
            TReturn backingField,
            TReturn newValue, [CallerMemberName] string propertyName = "")
            where TSource : IMvxNotifyPropertyChanged
        {
            return RaiseIfChanging(source, backingField, newValue, () => source.RaisePropertyChanging(newValue, propertyName));
        }

        public static bool RaiseIfChanging<TSource, TReturn>(this TSource source,
            TReturn backingField,
            TReturn newValue, MvxPropertyChangingEventArgs<TReturn> args)
            where TSource : IMvxNotifyPropertyChanged
        {
            return RaiseIfChanging(source, backingField, newValue, () => source.RaisePropertyChanging(args));
        }

        private static TReturn RaiseAndSetIfChanged<TSource, TReturn, TActionParameter>(
            TSource source, ref TReturn backingField, TReturn newValue,
            Func<TActionParameter, Task> raiseAction,
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
