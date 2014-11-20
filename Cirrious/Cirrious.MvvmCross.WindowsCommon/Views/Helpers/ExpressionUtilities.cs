using System;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace Cirrious.MvvmCross.WindowsCommon.Views.Helpers
{
    /// <summary>Provides methods to handle lambda expressions. </summary>
    public class ExpressionUtilities
    {
        /// <summary>Returns the property name of the property specified in the given lambda (e.g. GetPropertyName(i => i.MyProperty)). </summary>
        /// <typeparam name="TClass">The type of the class with the property. </typeparam>
        /// <typeparam name="TProperty">The property type. </typeparam>
        /// <param name="expression">The lambda with the property. </param>
        /// <returns>The name of the property in the lambda. </returns>
#if !LEGACY
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static string GetPropertyName<TClass, TProperty>(Expression<Func<TClass, TProperty>> expression)
        {
            if (expression.Body is UnaryExpression)
                return ((MemberExpression)(((UnaryExpression)expression.Body).Operand)).Member.Name;
            return ((MemberExpression)expression.Body).Member.Name;
        }

        /// <summary>Returns the property name of the property specified in the given lambda (e.g. GetPropertyName(i => i.MyProperty)). </summary>
        /// <typeparam name="TProperty">The property type. </typeparam>
        /// <param name="expression">The lambda with the property. </param>
        /// <returns>The name of the property in the lambda. </returns>
#if !LEGACY
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static string GetPropertyName<TProperty>(Expression<Func<TProperty>> expression)
        {
            if (expression.Body is UnaryExpression)
                return ((MemberExpression)(((UnaryExpression)expression.Body).Operand)).Member.Name;
            return ((MemberExpression)expression.Body).Member.Name;
        }

        /// <summary>Returns the property name of the property specified in the given lambda (e.g. GetPropertyName(i => i.MyProperty)). </summary>
        /// <typeparam name="TClass">The type of the class with the property. </typeparam>
        /// <param name="expression">The lambda with the property. </param>
        /// <returns>The name of the property in the lambda. </returns>
#if !LEGACY
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static string GetPropertyName<TClass>(Expression<Func<TClass, object>> expression)
        {
            if (expression.Body is UnaryExpression)
                return ((MemberExpression)(((UnaryExpression)expression.Body).Operand)).Member.Name;
            return ((MemberExpression)expression.Body).Member.Name;
        }

        //        /// <summary>Returns the event name of the event specified in the given lambda (e.g. GetEventName(i => i.MyEvent += null)). </summary>
        //        /// <typeparam name="TClass">The type of the class with the event. </typeparam>
        //        /// <param name="expression">The lambda with the event. </param>
        //        /// <returns>The name of the event in the lambda. </returns>
        //#if !LEGACY
        //        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        //#endif
        //        public static string GetEventName<TClass>(Expression<Action<TClass>> expression)
        //        {
        //            if (expression.Body is UnaryExpression)
        //                return ((MemberExpression)(((UnaryExpression)expression.Body).Operand)).Member.Name;
        //            return ((MemberExpression)expression.Body).Member.Name;
        //        }
    }

    [Obsolete("Use ExpressionUtilities instead. 9/25/2014")]
    public class ExpressionHelper
    {
        [Obsolete("Use ExpressionUtilities.GetPropertyName instead. 9/25/2014")]
        public static string GetName<TClass, TProperty>(Expression<Func<TClass, TProperty>> expression)
        {
            return ExpressionUtilities.GetPropertyName(expression);
        }

        [Obsolete("Use ExpressionUtilities.GetPropertyName instead. 9/25/2014")]
        public static string GetName<TProperty>(Expression<Func<TProperty>> expression)
        {
            return ExpressionUtilities.GetPropertyName(expression);
        }

        [Obsolete("Use ExpressionUtilities.GetPropertyName instead. 9/25/2014")]
        public static string GetName<TClass>(Expression<Func<TClass, object>> expression)
        {
            return ExpressionUtilities.GetPropertyName(expression);
        }
    }
}
