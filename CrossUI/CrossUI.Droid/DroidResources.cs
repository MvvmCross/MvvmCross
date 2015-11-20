// DroidResources.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Android.Content;
using Android.Util;
using Android.Views;
using Android.Webkit;
using Android.Widget;
using System;

namespace CrossUI.Droid
{
#warning Turn into an interface - get rid of the static!

    public static class DroidResources
    {
        private static Type _resourceLayoutType;

        public static void Initialize(Type resourceLayoutType)
        {
            _resourceLayoutType = resourceLayoutType;
        }

        public static View LoadFloatElementLayout(Context context, ViewGroup parent, string layoutName)
        {
            var layout = LoadLayout(context, parent, layoutName);
            return layout;
        }

        public static void DecodeFloatElementLayout(Context context, View layout, out TextView label, out SeekBar slider,
                                                    out ImageView left, out ImageView right)
        {
            if (layout == null)
            {
                label = null;
                slider = null;
                left = null;
                right = null;
                return;
            }

            label =
                layout.FindViewById<TextView>(context.Resources.GetIdentifier("dialog_LabelField", "id",
                                                                              context.PackageName));
            slider =
                layout.FindViewById<SeekBar>(context.Resources.GetIdentifier("dialog_SliderField", "id",
                                                                             context.PackageName));
            left =
                layout.FindViewById<ImageView>(context.Resources.GetIdentifier("dialog_ImageLeft", "id",
                                                                               context.PackageName));
            right =
                layout.FindViewById<ImageView>(context.Resources.GetIdentifier("dialog_ImageRight", "id",
                                                                               context.PackageName));
        }

        public static View LoadLayout(Context context, ViewGroup parent, int resourceId)
        {
            try
            {
                var inflater = LayoutInflater.FromContext(context);

                return inflater.Inflate(resourceId, parent, false);
            }
            catch (InflateException ex)
            {
                Log.Error("Android.Dialog", "Inflate failed: " + ex.Cause.Message);
            }
            catch (Exception ex)
            {
                Log.Error("Android.Dialog", "LoadLayout failed: " + ex.Message);
            }
            return null;
        }

        public static View LoadLayout(Context context, ViewGroup parent, string layoutName)
        {
            try
            {
                var resourceId = FindResourceId(layoutName);

                return LoadLayout(context, parent, resourceId);
            }
            catch (InflateException ex)
            {
                Log.Error("Android.Dialog", "Inflate failed: " + ex.Cause.Message);
            }
            catch (Exception ex)
            {
                Log.Error("Android.Dialog", "LoadLayout failed: " + ex.Message);
            }
            return null;
        }

        public static int FindResourceId(string layoutName)
        {
            if (_resourceLayoutType == null)
                throw new Exception("You must call DroidResources.Initialize(Resource.Layout) before using Dialogs");

            // how to get the resources from the elements...
            var layoutField = _resourceLayoutType.GetField(layoutName);
            if (layoutField == null)
                throw new Exception("Could not find resource field " + layoutName);

            var resourceId = (int)layoutField.GetValue(null);
            return resourceId;
        }

        public static View LoadStringElementLayout(Context context, ViewGroup parent, string layoutName)
        {
            var layout = LoadLayout(context, parent, layoutName);
            if (layout == null)
            {
                Log.Error("Android.Dialog", "LoadStringElementLayout: Failed to load resource: " + layoutName);
            }
            return layout;
        }

        public static void DecodeStringElementLayout(Context context, View layout, out TextView label,
                                                     out TextView value)
        {
            if (layout == null)
            {
                label = null;
                value = null;
                return;
            }

            label =
                layout.FindViewById<TextView>(context.Resources.GetIdentifier("dialog_LabelField", "id",
                                                                              context.PackageName));
            value =
                layout.FindViewById<TextView>(context.Resources.GetIdentifier("dialog_ValueField", "id",
                                                                              context.PackageName));
        }

        public static View LoadButtonLayout(Context context, ViewGroup parent, string layoutName)
        {
            var layout = LoadLayout(context, parent, layoutName);
            if (layout == null)
            {
                Log.Error("Android.Dialog", "LoadButtonLayout: Failed to load resource: " + layoutName);
            }
            return layout;
        }

        public static void DecodeButtonLayout(Context context, View layout, out Button button)
        {
            button =
                layout.FindViewById<Button>(context.Resources.GetIdentifier("dialog_Button", "id", context.PackageName));
        }

        public static View LoadMultilineElementLayout(Context context, ViewGroup parent, string layoutName, out EditText value)
        {
            var layout = LoadLayout(context, parent, layoutName);
            if (layout != null)
            {
                value =
                    layout.FindViewById<EditText>(context.Resources.GetIdentifier("dialog_ValueField", "id",
                                                                                  context.PackageName));
            }
            else
            {
                Log.Error("Android.Dialog", "LoadMultilineElementLayout: Failed to load resource: " + layoutName);
                value = null;
            }
            return layout;
        }

        public static View LoadBooleanElementLayout(Context context, ViewGroup parent, string layoutName)
        {
            var layout = LoadLayout(context, parent, layoutName);
            return layout;
        }

        public static void DecodeBooleanElementLayout(Context context, View layout, out TextView label,
                                                      out TextView subLabel, out View value)
        {
            if (layout != null)
            {
                label =
                    layout.FindViewById<TextView>(context.Resources.GetIdentifier("dialog_LabelField", "id",
                                                                                  context.PackageName));
                value =
                    layout.FindViewById<View>(context.Resources.GetIdentifier("dialog_BoolField", "id",
                                                                              context.PackageName));
                var id = context.Resources.GetIdentifier("dialog_LabelSubtextField", "id", context.PackageName);
                subLabel = (id >= 0) ? layout.FindViewById<TextView>(id) : null;
            }
            else
            {
                label = null;
                value = null;
                subLabel = null;
            }
        }

        public static View DecodeStringEntryLayout(Context context, View layout, out TextView label, out EditText value)
        {
            if (layout != null)
            {
                label =
                    layout.FindViewById<TextView>(context.Resources.GetIdentifier("dialog_LabelField", "id",
                                                                                  context.PackageName));
                value =
                    layout.FindViewById<EditText>(context.Resources.GetIdentifier("dialog_ValueField", "id",
                                                                                  context.PackageName));
            }
            else
            {
                label = null;
                value = null;
            }
            return layout;
        }

        public static View LoadStringEntryLayout(Context context, ViewGroup parent, string layoutName)
        {
            var layout = LoadLayout(context, parent, layoutName);
            if (layout == null)
            {
                Log.Error("Android.Dialog", "LoadStringEntryLayout: Failed to load resource: " + layoutName);
            }
            return layout;
        }

        public static View LoadHtmlLayout(Context context, ViewGroup parent, string layoutName, out WebView webView)
        {
            var layout = LoadLayout(context, parent, layoutName);
            if (layout != null)
            {
                webView =
                    layout.FindViewById<WebView>(context.Resources.GetIdentifier("dialog_HtmlField", "id",
                                                                                 context.PackageName));
            }
            else
            {
                Log.Error("Android.Dialog", "LoadHtmlLayout: Failed to load resource: " + layoutName);
                webView = null;
            }
            return layout;
        }

        public static View LoadEntryButtonLayout(Context context, ViewGroup parent, string layoutName, out TextView label, out EditText value, out ImageButton button)
        {
            var layout = LoadLayout(context, parent, layoutName);
            if (layout != null)
            {
                label =
                    layout.FindViewById<TextView>(context.Resources.GetIdentifier("dialog_LabelField", "id",
                                                                                  context.PackageName));
                value =
                    layout.FindViewById<EditText>(context.Resources.GetIdentifier("dialog_ValueField", "id",
                                                                                  context.PackageName));
                button =
                    layout.FindViewById<ImageButton>(context.Resources.GetIdentifier("dialog_Button", "id",
                                                                                     context.PackageName));
            }
            else
            {
                Log.Error("Android.Dialog", "LoadStringEntryLayout: Failed to load resource: " + layoutName);
                label = null;
                value = null;
                button = null;
            }
            return layout;
        }

        public static View LoadAchievementsElementLayout(Context context, ViewGroup parent, string layoutName)
        {
            var layout = LoadLayout(context, parent, layoutName);
            if (layout == null)
            {
                Log.Error("Android.Dialog",
                          "LoadAchievementsElementLayout: Failed to load resource: " +
                          layoutName);
            }
            return layout;
        }

        public static View DecodeAchievementsElementLayout(Context context, View layout, out TextView caption,
                                                           out TextView description, out TextView percentageComplete,
                                                           out ImageView achivementImage)
        {
            if (layout == null)
            {
                achivementImage = null;
                caption = null;
                description = null;
                percentageComplete = null;
            }
            else
            {
                achivementImage =
                    layout.FindViewById<ImageView>(context.Resources.GetIdentifier("dialog_ImageRight", "id",
                                                                                   context.PackageName));
                caption =
                    layout.FindViewById<TextView>(context.Resources.GetIdentifier("dialog_LabelField", "id",
                                                                                  context.PackageName));
                description =
                    layout.FindViewById<TextView>(context.Resources.GetIdentifier("dialog_LabelSubtextField", "id",
                                                                                  context.PackageName));
                percentageComplete =
                    layout.FindViewById<TextView>(context.Resources.GetIdentifier("dialog_LabelPercentageField", "id",
                                                                                  context.PackageName));
            }
            return layout;
        }
    }
}