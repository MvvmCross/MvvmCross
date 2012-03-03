#region Copyright
// <copyright file="MvxTextSettingExtensions.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com
#endregion

using System.ComponentModel;
using Android.App;
using Android.Views;
using Android.Widget;
using Cirrious.MvvmCross.Binding.Interfaces;
using Cirrious.MvvmCross.Interfaces.Converters;
using Cirrious.MvvmCross.Interfaces.Localization;
using Cirrious.MvvmCross.Interfaces.Platform.Diagnostics;

namespace Cirrious.MvvmCross.Binding.Android.ExtensionMethods
{
    public static class MvxTextSettingExtensions
    {
        public static void BindTextViewText(this Activity activity, int targetViewId, INotifyPropertyChanged source, string sourcePropertyName, IMvxValueConverter converter = null, object converterParameter = null)
        {
            BindTextViewText<string>(activity, targetViewId, source, sourcePropertyName, converter, converterParameter);
        }

        public static void BindTextViewText<TSourceType>(this Activity activity, int targetViewId, INotifyPropertyChanged source, string sourcePropertyName, IMvxValueConverter converter = null, object converterParameter = null)
        {
            var description = new MvxBindingDescription()
            {
                SourcePropertyPath = sourcePropertyName,
                Converter = converter,
                ConverterParameter = converterParameter,
                TargetName = "Text",
                Mode = MvxBindingMode.OneWay
            };
            activity.BindView<TextView>(targetViewId, source, description);
        }

        public static void SetTextViewText(this Activity activity, int textViewId, IMvxLanguageBinder languageBinder, string key)
        {
            var textView = activity.FindViewById<TextView>(textViewId);
            SetTextViewTextInner(textViewId, textView, languageBinder, key);
        }

        public static void SetTextViewText(this View view, int textViewId, IMvxLanguageBinder languageBinder, string key)
        {
            var textView = view.FindViewById<TextView>(textViewId);
            SetTextViewTextInner(textViewId, textView, languageBinder, key);
        }

        private static void SetTextViewTextInner(int textViewId, TextView textView, IMvxLanguageBinder languageBinder, string key)
        {
            if (textView == null)
            {
                MvxBindingTrace.Trace(MvxTraceLevel.Error, "textView not found for binding " + textViewId);
                return;
            }

            textView.Text = languageBinder.GetText(key).Replace("\r", "\n");
        }
    }
}