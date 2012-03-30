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

namespace Cirrious.MvvmCross.ViewModels
{
    public abstract class MvxNotifyPropertyChanged
        : MvxMainThreadDispatchingObject, INotifyPropertyChanged
    {
        private const string WrongExpressionMessage = "Wrong expression\nshould be called as\nFirePropertyChange(() => PropertyName);";

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        protected void FirePropertyChanged<T>(Expression<Func<T>> property)
        {
            if (property == null)
            {
                throw new ArgumentNullException("property");
            }

            var memberExpression = property.Body as MemberExpression;
            if (memberExpression == null)
            {
                throw new ArgumentException(WrongExpressionMessage, "property");
            }

            var member = memberExpression.Member as PropertyInfo;
            if (member == null)
            {
                throw new ArgumentException(WrongExpressionMessage, "property");
            }

            if (member.DeclaringType == null)
            {
                throw new ArgumentException(WrongExpressionMessage, "property");
            }

#if NETFX_CORE
            if (!member.DeclaringType.GetTypeInfo().IsAssignableFrom(GetType().GetTypeInfo()))
            {
                throw new ArgumentException(WrongExpressionMessage, "property");
            }

            if (member.GetMethod.IsStatic)
            {
                throw new ArgumentException(WrongExpressionMessage, "property");
            }
#else
            if (!member.DeclaringType.IsAssignableFrom(GetType()))
            {
                throw new ArgumentException(WrongExpressionMessage, "property");
            }

            if (member.GetGetMethod(true).IsStatic)
            {
                throw new ArgumentException(WrongExpressionMessage, "property");
            }
#endif
            FirePropertyChanged(member.Name);
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