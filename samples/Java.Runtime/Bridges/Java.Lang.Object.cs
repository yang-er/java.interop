using Android.Runtime;
using Java.Interop;
using System;
using System.Collections.Generic;

namespace Java.Lang
{
    public partial class Object : JavaObject
    {
        private static readonly Dictionary<IntPtr, List<WeakReference>> instances =
            new Dictionary<IntPtr, List<WeakReference>>();
        protected virtual System.IntPtr ThresholdClass => class_ref;
        protected virtual System.Type ThresholdType => _members.ManagedPeerType;

        public Object(ref JniObjectReference reference, JniObjectReferenceOptions options) :
            base(ref reference, options)
        {
        }

        public static T GetObject<T>(IntPtr jnienv, IntPtr handle, JniHandleOwnership transfer) where T : class, IJavaPeerable
        {
            new JniTransition(jnienv).Dispose();
            var jobj = new JniObjectReference(handle, (JniObjectReferenceType)transfer);
            return GetObject<T>(ref jobj, JniObjectReferenceOptions.CopyAndDispose);
        }

        public static T GetObject<T>(IntPtr handle, JniHandleOwnership transfer) where T : class, IJavaPeerable
        {
            var jobj = new JniObjectReference(handle, (JniObjectReferenceType)transfer);
            return GetObject<T>(ref jobj, JniObjectReferenceOptions.CopyAndDispose);
        }

        public static unsafe T GetObject<T>(
            ref JniObjectReference jobj,
            JniObjectReferenceOptions options)
            where T : class, IJavaPeerable
        {
            return !jobj.IsValid ? default : (T)GetObject(ref jobj, options, typeof(T));
        }

        internal static IJavaPeerable GetObject(
            ref JniObjectReference jobj,
            JniObjectReferenceOptions options,
            Type targetType = null)
        {
            if (!jobj.IsValid) return null;
            return JniRuntime.CurrentRuntime.ValueManager.GetValue<IJavaPeerable>(ref jobj, options, targetType);
        }
    }
}
