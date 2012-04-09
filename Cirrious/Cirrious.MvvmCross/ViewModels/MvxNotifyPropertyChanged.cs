#region Copyright
// <copyright file="MvxNotifyPropertyChanged.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com
#endregion

using System.ComponentModel;
using System.Linq.Expressions;
using System;
using System.Reflection;
using Cirrious.MvvmCross.ExtensionMethods;

namespace Cirrious.MvvmCross.ViewModels
{
    public abstract class MvxNotifyPropertyChanged
        : MvxMainThreadDispatchingObject, INotifyPropertyChanged
    {
        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        protected void FirePropertyChanged<T>(Expression<Func<T>> property)
        {
            var name = this.GetPropertyNameFromExpression(property);
            FirePropertyChanged(name);
        }

        protected void FirePropertyChanged(string whichProperty)
        {
            // check for subscription before going multithreaded
            if (PropertyChanged == null)
                return;

            InvokeOnMainThread(
                    () =>
                        {
                            // take a copy - see RoadWarrior's answer on http://stackoverflow.com/questions/282653/checking-for-null-before-event-dispatching-thread-safe/282741#282741
                            var handler = PropertyChanged;

                            if (handler != null)
                                handler(this, new PropertyChangedEventArgs(whichProperty));
                        });
        }
    }
}