// MvxTextSettingExtensions.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System.ComponentModel;
using Android.App;
using Android.Views;
using Android.Widget;
using Cirrious.MvvmCross.Binding.Interfaces;
using Cirrious.MvvmCross.Interfaces.Converters;
using Cirrious.MvvmCross.Interfaces.Platform.Diagnostics;
using Cirrious.MvvmCross.Localization.Interfaces;
using Cirrious.MvvmCross.Binding.Droid.Interfaces.Views;
using Cirrious.MvvmCross.Binding.Droid.Binders;
using Cirrious.MvvmCross.Binding.ExtensionMethods;
using Cirrious.MvvmCross.Binding.Droid.Interfaces.Binders;

namespace Cirrious.MvvmCross.Binding.Droid.ExtensionMethods
{
    public static class MvxTextSettingExtensions
    {
        /// <summary>
        /// Creates a custom binding for the <see cref="TextView"/>.
        /// Remember to call <see cref="IMvxViewBindingManager.UnbindView"/> on the view after you're done
        /// using it.
        /// </summary>
        public static void BindToText(this TextView view, INotifyPropertyChanged source,
                                      string sourcePropertyName, IMvxValueConverter converter = null,
                                      object converterParameter = null)
        {
            var activity = view.Context as IMvxBindingActivity;
            if (activity == null)
                return;

            var description = new MvxBindingDescription
            {
                SourcePropertyPath = sourcePropertyName,
                Converter = converter,
                ConverterParameter = converterParameter,
                TargetName = "Text",
                Mode = MvxBindingMode.OneWay
            }.ToEnumerable();

            var tag = view.GetBindingTag ();
            if (tag != null) {
                MvxBindingTrace.Trace(
                    MvxTraceLevel.Warning,
                    "Replacing binding tag for a TextView " + view.Id);
            }

            view.SetBindingTag (new MvxViewBindingTag (description));
            activity.BindingManager.BindView (view, source);
        }

        /// <summary>
        /// Sets the TextView text to a localized version of <paramref name="key"/>.
        /// </summary>
        public static void SetText(this TextView view, IMvxLanguageBinder languageBinder, string key) {
            view.Text = languageBinder.GetText(key).Replace("\r", "\n");
        }
    }
}