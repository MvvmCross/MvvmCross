// InternalStyleable.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using Android.Runtime;

namespace CrossUI.Droid.Dialog
{
    [Register("android/R$styleable", DoNotGenerateAcw = true)]
    public class InternalStyleable : Java.Lang.Object
    {
        internal static IntPtr java_class_handle;
        private static IntPtr id_ctor;
        private static IntPtr scrollViewFieldId;
        private static IntPtr scrollView_fillViewportFieldId;

        internal static IntPtr class_ref
        {
            get { return JNIEnv.FindClass("android/R$styleable", ref java_class_handle); }
        }

        protected override IntPtr ThresholdClass
        {
            get { return class_ref; }
        }

        protected override Type ThresholdType
        {
            get { return typeof(InternalStyleable); }
        }

        internal InternalStyleable(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {
        }

        [Register(".ctor", "()V", "")]
        public InternalStyleable()
            : base(IntPtr.Zero, JniHandleOwnership.DoNotTransfer)
        {
            if (this.Handle != IntPtr.Zero)
                return;
            if (this.GetType() != typeof(InternalStyleable))
            {
                this.SetHandle(JNIEnv.CreateInstance(GetType(), "()V", new JValue[0]),
                               JniHandleOwnership.TransferLocalRef);
            }
            else
            {
                if (id_ctor == IntPtr.Zero)
                    id_ctor = JNIEnv.GetMethodID(class_ref, "<init>", "()V");
                this.SetHandle(JNIEnv.NewObject(class_ref, id_ctor), JniHandleOwnership.TransferLocalRef);
            }
        }

        public static int[] ScrollView
        {
            get
            {
                if (scrollViewFieldId == IntPtr.Zero)
                    scrollViewFieldId = JNIEnv.GetStaticFieldID(class_ref, "ScrollView", "[I");
                IntPtr ret = JNIEnv.GetStaticObjectField(class_ref, scrollViewFieldId);
                return GetObject<JavaArray<int>>(ret, JniHandleOwnership.TransferLocalRef).ToArray<int>();
            }
        }

        public static int[] ListView
        {
            get
            {
                if (scrollViewFieldId == IntPtr.Zero)
                    scrollViewFieldId = JNIEnv.GetStaticFieldID(class_ref, "ListView", "[I");
                IntPtr ret = JNIEnv.GetStaticObjectField(class_ref, scrollViewFieldId);
                return GetObject<JavaArray<int>>(ret, JniHandleOwnership.TransferLocalRef).ToArray<int>();
            }
        }

        public static int ListView_divider
        {
            get
            {
                if (scrollView_fillViewportFieldId == IntPtr.Zero)
                    scrollView_fillViewportFieldId = JNIEnv.GetStaticFieldID(class_ref, "ScrollView_fillViewport", "I");
                return JNIEnv.GetStaticIntField(class_ref, scrollView_fillViewportFieldId);
            }
        }
    }
}