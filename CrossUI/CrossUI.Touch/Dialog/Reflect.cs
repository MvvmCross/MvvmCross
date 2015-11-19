// Reflect.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using CrossUI.Touch.Dialog.Elements;
using Foundation;
using UIKit;

namespace CrossUI.Touch.Dialog
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, Inherited = false)]
    public class EntryAttribute : Attribute
    {
        public EntryAttribute() : this(null)
        {
        }

        public EntryAttribute(string placeholder)
        {
            Placeholder = placeholder;
        }

        public string Placeholder;
        public UIKeyboardType KeyboardType;
    }

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, Inherited = false)]
    public class DateAttribute : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, Inherited = false)]
    public class TimeAttribute : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, Inherited = false)]
    public class CheckboxAttribute : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, Inherited = false)]
    public class MultilineAttribute : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, Inherited = false)]
    public class HtmlAttribute : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, Inherited = false)]
    public class SkipAttribute : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, Inherited = false)]
    public class PasswordAttribute : EntryAttribute
    {
        public PasswordAttribute(string placeholder) : base(placeholder)
        {
        }
    }

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, Inherited = false)]
    public class AlignmentAttribute : Attribute
    {
        public AlignmentAttribute(UITextAlignment alignment)
        {
            Alignment = alignment;
        }

        public UITextAlignment Alignment;
    }

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, Inherited = false)]
    public class RadioSelectionAttribute : Attribute
    {
        public string Target;

        public RadioSelectionAttribute(string target)
        {
            Target = target;
        }
    }

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, Inherited = false)]
    public class OnTapAttribute : Attribute
    {
        public OnTapAttribute(string method)
        {
            Method = method;
        }

        public string Method;
    }

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, Inherited = false)]
    public class CaptionAttribute : Attribute
    {
        public CaptionAttribute(string caption)
        {
            Caption = caption;
        }

        public string Caption;
    }

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, Inherited = false)]
    public class SectionAttribute : Attribute
    {
        public SectionAttribute()
        {
        }

        public SectionAttribute(string caption)
        {
            Caption = caption;
        }

        public SectionAttribute(string caption, string footer)
        {
            Caption = caption;
            Footer = footer;
        }

        public string Caption, Footer;
    }

    public class RangeAttribute : Attribute
    {
        public RangeAttribute(float low, float high)
        {
            Low = low;
            High = high;
        }

        public float Low, High;
        public bool ShowCaption;
    }

    public class BindingContext : IDisposable
    {
        public RootElement Root;
        private Dictionary<Element, MemberAndInstance> mappings;

        private class MemberAndInstance
        {
            public MemberAndInstance(MemberInfo mi, object o)
            {
                Member = mi;
                Obj = o;
            }

            public readonly MemberInfo Member;
            public readonly object Obj;
        }

        private static object GetValue(MemberInfo mi, object o)
        {
            var fi = mi as FieldInfo;
            if (fi != null)
                return fi.GetValue(o);
            var pi = mi as PropertyInfo;

            var getMethod = pi.GetGetMethod();
            return getMethod.Invoke(o, new object[0]);
        }

        private static void SetValue(MemberInfo mi, object o, object val)
        {
            var fi = mi as FieldInfo;
            if (fi != null)
            {
                fi.SetValue(o, val);
                return;
            }
            var pi = mi as PropertyInfo;
            var setMethod = pi.GetSetMethod();
            setMethod.Invoke(o, new[] {val});
        }

        private static string MakeCaption(string name)
        {
            var sb = new StringBuilder(name.Length);
            bool nextUp = true;

            foreach (char c in name)
            {
                if (nextUp)
                {
                    sb.Append(Char.ToUpper(c));
                    nextUp = false;
                }
                else
                {
                    if (c == '_')
                    {
                        sb.Append(' ');
                        continue;
                    }
                    if (Char.IsUpper(c))
                        sb.Append(' ');
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }

        // Returns the type for fields and properties and null for everything else
        private static Type GetTypeForMember(MemberInfo mi)
        {
            if (mi is FieldInfo)
                return ((FieldInfo) mi).FieldType;
            else if (mi is PropertyInfo)
                return ((PropertyInfo) mi).PropertyType;
            return null;
        }

        public BindingContext(object callbacks, object o, string title)
        {
            if (o == null)
                throw new ArgumentNullException(nameof(o));

            mappings = new Dictionary<Element, MemberAndInstance>();

            Root = new RootElement(title);
            Populate(callbacks, o, Root);
        }

        private void Populate(object callbacks, object o, RootElement root)
        {
            MemberInfo last_radio_index = null;
            var members = o.GetType().GetMembers(BindingFlags.DeclaredOnly | BindingFlags.Public |
                                                 BindingFlags.NonPublic | BindingFlags.Instance);

            Section section = null;

            foreach (var mi in members)
            {
                Type mType = GetTypeForMember(mi);

                if (mType == null)
                    continue;

                string caption = null;
                object[] attrs = mi.GetCustomAttributes(false);
                bool skip = false;
                foreach (var attr in attrs)
                {
                    if (attr is SkipAttribute || attr is System.Runtime.CompilerServices.CompilerGeneratedAttribute)
                        skip = true;
                    else if (attr is CaptionAttribute)
                        caption = ((CaptionAttribute) attr).Caption;
                    else if (attr is SectionAttribute)
                    {
                        if (section != null)
                            root.Add(section);
                        var sa = attr as SectionAttribute;
                        section = new Section(sa.Caption, sa.Footer);
                    }
                }
                if (skip)
                    continue;

                if (caption == null)
                    caption = MakeCaption(mi.Name);

                if (section == null)
                    section = new Section();

                Element element = null;
                if (mType == typeof (string))
                {
                    PasswordAttribute pa = null;
                    AlignmentAttribute align = null;
                    EntryAttribute ea = null;
                    object html = null;
                    Action invoke = null;
                    bool multi = false;

                    foreach (object attr in attrs)
                    {
                        if (attr is PasswordAttribute)
                            pa = attr as PasswordAttribute;
                        else if (attr is EntryAttribute)
                            ea = attr as EntryAttribute;
                        else if (attr is MultilineAttribute)
                            multi = true;
                        else if (attr is HtmlAttribute)
                            html = attr;
                        else if (attr is AlignmentAttribute)
                            align = attr as AlignmentAttribute;

                        if (attr is OnTapAttribute)
                        {
                            string mname = ((OnTapAttribute) attr).Method;

                            if (callbacks == null)
                            {
                                throw new Exception(
                                    "Your class contains [OnTap] attributes, but you passed a null object for `context' in the constructor");
                            }

                            var method = callbacks.GetType().GetMethod(mname);
                            if (method == null)
                                throw new Exception("Did not find method " + mname);
                            invoke = delegate { method.Invoke(method.IsStatic ? null : callbacks, new object[0]); };
                        }
                    }

                    var value = (string) GetValue(mi, o);
                    if (pa != null)
                        element = new EntryElement(caption, pa.Placeholder, value, true);
                    else if (ea != null)
                        element = new EntryElement(caption, ea.Placeholder, value) {KeyboardType = ea.KeyboardType};
                    else if (multi)
                        element = new MultilineElement(caption, value);
                    else if (html != null)
                        element = new HtmlElement(caption, value);
                    else
                    {
                        var selement = new StringElement(caption, value);
                        element = selement;

                        if (align != null)
                            selement.Alignment = align.Alignment;
                    }

                    if (invoke != null)
                        (element).Tapped += invoke;
                }
                else if (mType == typeof (float))
                {
                    var floatElement = new FloatElement(null, null, (float) GetValue(mi, o)) {Caption = caption};
                    element = floatElement;

                    foreach (object attr in attrs)
                    {
                        if (attr is RangeAttribute)
                        {
                            var ra = attr as RangeAttribute;
                            floatElement.MinValue = ra.Low;
                            floatElement.MaxValue = ra.High;
                            floatElement.ShowCaption = ra.ShowCaption;
                        }
                    }
                }
                else if (mType == typeof (bool))
                {
                    bool checkbox = false;
                    foreach (object attr in attrs)
                    {
                        if (attr is CheckboxAttribute)
                            checkbox = true;
                    }

                    if (checkbox)
                        element = new CheckboxElement(caption, (bool) GetValue(mi, o));
                    else
                        element = new BooleanElement(caption, (bool) GetValue(mi, o));
                }
                else if (mType == typeof (DateTime))
                {
                    var dateTime = (DateTime) GetValue(mi, o);
                    bool asDate = false, asTime = false;

                    foreach (object attr in attrs)
                    {
                        if (attr is DateAttribute)
                            asDate = true;
                        else if (attr is TimeAttribute)
                            asTime = true;
                    }

                    if (asDate)
                        element = new DateElement(caption, dateTime);
                    else if (asTime)
                        element = new TimeElement(caption, dateTime);
                    else
                        element = new DateTimeElement(caption, dateTime);
                }
                else if (mType.IsEnum)
                {
                    var csection = new Section();
                    ulong evalue = Convert.ToUInt64(GetValue(mi, o), null);
                    int idx = 0;
                    int selected = 0;

                    foreach (var fi in mType.GetFields(BindingFlags.Public | BindingFlags.Static))
                    {
                        ulong v = Convert.ToUInt64(GetValue(fi, null));

                        if (v == evalue)
                            selected = idx;

                        var ca = Attribute.GetCustomAttribute(fi, typeof (CaptionAttribute)) as CaptionAttribute;
                        csection.Add(new RadioElement(ca != null ? ca.Caption : MakeCaption(fi.Name)));
                        idx++;
                    }

                    element = new RootElement(caption, new RadioGroup(null, selected)) {csection};
                }
                else if (mType == typeof (UIImage))
                {
                    element = new ImageElement((UIImage) GetValue(mi, o));
                }
                else if (typeof (System.Collections.IEnumerable).IsAssignableFrom(mType))
                {
                    var csection = new Section();
                    int count = 0;

                    if (last_radio_index == null)
                        throw new Exception("IEnumerable found, but no previous int found");
                    foreach (var e in (IEnumerable) GetValue(mi, o))
                    {
                        csection.Add(new RadioElement(e.ToString()));
                        count++;
                    }
                    var selected = (int) GetValue(last_radio_index, o);
                    if (selected >= count || selected < 0)
                        selected = 0;
                    element = new RootElement(caption, new MemberRadioGroup(null, selected, last_radio_index))
                        {
                            csection
                        };
                    last_radio_index = null;
                }
                else if (typeof (int) == mType)
                {
                    if (attrs.OfType<RadioSelectionAttribute>().Any())
                    {
                        last_radio_index = mi;
                    }
                }
                else
                {
                    var nested = GetValue(mi, o);
                    if (nested != null)
                    {
                        var newRoot = new RootElement(caption);
                        Populate(callbacks, nested, newRoot);
                        element = newRoot;
                    }
                }

                if (element == null)
                    continue;
                section.Add(element);
                mappings[element] = new MemberAndInstance(mi, o);
            }
            root.Add(section);
        }

        private class MemberRadioGroup : RadioGroup
        {
            public readonly MemberInfo mi;

            public MemberRadioGroup(string key, int selected, MemberInfo mi) : base(key, selected)
            {
                this.mi = mi;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                foreach (var element in mappings.Keys)
                {
                    element.Dispose();
                }
                mappings = null;
            }
        }

        public void Fetch()
        {
            foreach (var dk in mappings)
            {
                Element element = dk.Key;
                MemberInfo mi = dk.Value.Member;
                object obj = dk.Value.Obj;

                if (element is DateTimeElement)
                    SetValue(mi, obj, ((DateTimeElement) element).Value);
                else if (element is FloatElement)
                    SetValue(mi, obj, ((FloatElement) element).Value);
                else if (element is BooleanElement)
                    SetValue(mi, obj, ((BooleanElement) element).Value);
                else if (element is CheckboxElement)
                    SetValue(mi, obj, ((CheckboxElement) element).Value);
                else if (element is EntryElement)
                {
                    var entry = (EntryElement) element;
                    entry.FetchAndUpdateValue();
                    SetValue(mi, obj, entry.Value);
                }
                else if (element is ImageElement)
                    SetValue(mi, obj, ((ImageElement) element).Value);
                else if (element is RootElement)
                {
                    var re = element as RootElement;
                    if (re.Group as MemberRadioGroup != null)
                    {
                        var group = re.Group as MemberRadioGroup;
                        SetValue(group.mi, obj, re.RadioSelected);
                    }
                    else if (re.Group as RadioGroup != null)
                    {
                        var mType = GetTypeForMember(mi);
                        var fi = mType.GetFields(BindingFlags.Public | BindingFlags.Static)[re.RadioSelected];

                        SetValue(mi, obj, fi.GetValue(null));
                    }
                }
            }
        }
    }
}