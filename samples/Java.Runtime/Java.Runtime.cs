using Android.Runtime;
using Java.Interop;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

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

        public static JavaArray<int> NewArray(int[] array)
        {
            return array == null ? null : new JavaInt32Array(array);
        }

        public static JavaArray<short> NewArray(short[] array)
        {
            return array == null ? null : new JavaInt16Array(array);
        }

        public static JavaArray<long> NewArray(long[] array)
        {
            return array == null ? null : new JavaInt64Array(array);
        }

        public static JavaArray<float> NewArray(float[] array)
        {
            return array == null ? null : new JavaSingleArray(array);
        }

        public static JavaArray<double> NewArray(double[] array)
        {
            return array == null ? null : new JavaDoubleArray(array);
        }

        public static JavaArray<char> NewArray(char[] array)
        {
            return array == null ? null : new JavaCharArray(array);
        }

        public static JavaArray<sbyte> NewArray(sbyte[] array)
        {
            return array == null ? null : new JavaSByteArray(array);
        }

        public static JavaArray<bool> NewArray(bool[] array)
        {
            return array == null ? null : new JavaBooleanArray(array);
        }

        public static JavaArray<Java.Lang.String> NewArray(string[] array)
        {
            if (array == null) return null;
            var obj = new JavaObjectArray<Java.Lang.String>(array.Length);
            for (int i = 0; i < array.Length; i++)
                obj[i] = new Java.Lang.String(array[i]);
            return obj;
        }

        public static JavaArray<T> NewArray<T>(T[] array)
            where T : class, IJavaPeerable
        {
            return array == null ? null : new JavaObjectArray<T>(array);
        }

        public static object GetArray<T>(IntPtr handle, JniHandleOwnership ownership)
        {
            if (handle == IntPtr.Zero) return null;
            var @ref = new JniObjectReference(handle, (JniObjectReferenceType)ownership);
            return GetArray<T>(ref @ref, JniObjectReferenceOptions.CopyAndDispose);
        }

        public static unsafe void ArrayCopyNative<T>(T[] array, IntPtr dest)
        {
            var destSrc = new JniObjectReference(dest);
            if (typeof(IJavaPeerable).IsAssignableFrom(typeof(T)))
                for (int i = 0; i < array.Length; i++)
                    JniEnvironment.Arrays.SetObjectArrayElement(destSrc, i, ((IJavaPeerable)array[i]).PeerReference);
            else if (typeof(T) == typeof(string))
                for (int i = 0; i < array.Length; i++)
                    JniEnvironment.Arrays.SetObjectArrayElement(destSrc, i, new Java.Lang.String((string)(object)array[i]).PeerReference);
            else if (typeof(T) == typeof(int))
                fixed (int* t = (int[])(object)array)
                    JniEnvironment.Arrays.SetIntArrayRegion(destSrc, 0, array.Length, t);
            else if (typeof(T) == typeof(short))
                fixed (short* t = (short[])(object)array)
                    JniEnvironment.Arrays.SetShortArrayRegion(destSrc, 0, array.Length, t);
            else if (typeof(T) == typeof(long))
                fixed (long* t = (long[])(object)array)
                    JniEnvironment.Arrays.SetLongArrayRegion(destSrc, 0, array.Length, t);
            else if (typeof(T) == typeof(float))
                fixed (float* t = (float[])(object)array)
                    JniEnvironment.Arrays.SetFloatArrayRegion(destSrc, 0, array.Length, t);
            else if (typeof(T) == typeof(double))
                fixed (double* t = (double[])(object)array)
                    JniEnvironment.Arrays.SetDoubleArrayRegion(destSrc, 0, array.Length, t);
            else if (typeof(T) == typeof(sbyte))
                fixed (sbyte* t = (sbyte[])(object)array)
                    JniEnvironment.Arrays.SetByteArrayRegion(destSrc, 0, array.Length, t);
            else if (typeof(T) == typeof(bool))
                fixed (bool* t = (bool[])(object)array)
                    JniEnvironment.Arrays.SetBooleanArrayRegion(destSrc, 0, array.Length, t);
            else if (typeof(T) == typeof(int))
                fixed (char* t = (char[])(object)array)
                    JniEnvironment.Arrays.SetCharArrayRegion(destSrc, 0, array.Length, t);
            else
                throw new NotSupportedException();
        }

        public static object GetArray<T>(ref JniObjectReference handle, JniObjectReferenceOptions options)
        {
            if (!handle.IsValid) return null;
            if (typeof(T) == typeof(int))
                return new JavaInt32Array(ref handle, options).ToArray();
            else if (typeof(T) == typeof(sbyte))
                return new JavaSByteArray(ref handle, options).ToArray();
            else if (typeof(T) == typeof(double))
                return new JavaDoubleArray(ref handle, options).ToArray();
            else if (typeof(T) == typeof(float))
                return new JavaSingleArray(ref handle, options).ToArray();
            else if (typeof(T) == typeof(short))
                return new JavaInt16Array(ref handle, options).ToArray();
            else if (typeof(T) == typeof(long))
                return new JavaInt64Array(ref handle, options).ToArray();
            else if (typeof(T) == typeof(char))
                return new JavaCharArray(ref handle, options).ToArray();
            else if (typeof(T) == typeof(bool))
                return new JavaBooleanArray(ref handle, options).ToArray();
            return new JavaObjectArray<T>(ref handle, options).ToArray();
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
            Type type = null)
        {
            if (!jobj.IsValid) return null;

            lock (instances)
            {
                if (instances.TryGetValue(jobj.Handle, out var weakReferenceList))
                {
                    for (int index = 0; index < weakReferenceList.Count; ++index)
                    {
                        if (weakReferenceList[index].Target is IJavaPeerable target
                            && target.PeerReference.IsValid
                            && JniEnvironment.Types.IsSameObject(jobj, target.PeerReference)
                            && (type == null ? 1 : (type.IsAssignableFrom(target.GetType()) ? 1 : 0)) != 0)
                        {
                            JniObjectReference.Dispose(ref jobj, options);
                            return target;
                        }
                    }
                }
            }

            return Java.Interop.TypeManager.CreateInstance(ref jobj, options, type);
        }
    }

    public partial interface ICharSequence : IEnumerable<char>
    {

    }

    internal partial class ICharSequenceInvoker
    {
        public static IntPtr ToLocalJniHandle(ICharSequence vs)
        {
            var @ref = vs.PeerReference.NewLocalRef();
            return @ref.Handle;
        }
    }

    public partial class Throwable : JavaException
    {

    }

    /*public partial class Object2 : IJavaObject, IJavaObjectEx
    {
        private static readonly Dictionary<IntPtr, List<WeakReference>> instances =
            new Dictionary<IntPtr, List<WeakReference>>();
        private IntPtr key_handle;
        private IntPtr weak_handle;
        private JObjectRefType handle_type;
        private IntPtr handle;
        private int refs_added;
        private bool needsActivation;
        private bool isProxy;

        IntPtr IJavaObjectEx.KeyHandle
        {
            get => key_handle;
            set => key_handle = value;
        }

        bool IJavaObjectEx.IsProxy
        {
            get => isProxy;
            set => isProxy = value;
        }

        bool IJavaObjectEx.NeedsActivation
        {
            get => needsActivation;
            set => needsActivation = true;
        }

        IntPtr IJavaObjectEx.ToLocalJniHandle()
        {
            lock (this)
                return handle == IntPtr.Zero ? handle : JNIEnv.NewLocalRef(handle);
        }

        public Object(IntPtr _handle, JniHandleOwnership transfer)
        {
            if (handle != IntPtr.Zero)
            {
                needsActivation = true;
                _handle = handle;
                if (handle_type != JObjectRefType.Invalid)
                    return;
                transfer = JniHandleOwnership.DoNotTransfer;
            }
            
            SetHandle(_handle, transfer);
        }

        [OnDeserialized]
        internal void SetHandleOnDeserialized(StreamingContext context)
        {
            if (Handle != IntPtr.Zero) return;
            SetHandle(JNIEnv.StartCreateInstance(GetType(), "()V"), JniHandleOwnership.TransferLocalRef);
            JNIEnv.FinishCreateInstance(Handle, "()V");
        }

        public IntPtr Handle => handle;

        internal IntPtr GetThresholdClass() => ThresholdClass;

        internal Type GetThresholdType() => ThresholdType;

        protected void SetHandle(IntPtr value, JniHandleOwnership transfer)
        {
            RegisterInstance(this, value, transfer, out handle);
            handle_type = JObjectRefType.Global;
        }

        internal static void RegisterInstance(
            IJavaObject instance,
            IntPtr value,
            JniHandleOwnership transfer,
            out IntPtr handle)
        {
            if (instance is null)
                throw new ArgumentNullException(nameof(instance));

            if (value == IntPtr.Zero)
            {
                handle = value;
            }
            else
            {
                switch (transfer & (JniHandleOwnership.TransferLocalRef | JniHandleOwnership.TransferGlobalRef))
                {
                    case JniHandleOwnership.DoNotTransfer:
                        handle = JNIEnv.NewGlobalRef(value);
                        break;
                    case JniHandleOwnership.TransferLocalRef:
                        handle = JNIEnv.NewGlobalRef(value);
                        JNIEnv.DeleteLocalRef(value);
                        break;
                    case JniHandleOwnership.TransferGlobalRef:
                        handle = value;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(transfer), transfer,
                            $"Invalid `transfer` value: {transfer} on type {instance.GetType()}");
                }

                if (handle == IntPtr.Zero)
                    throw new InvalidOperationException($"Unable to allocate Global Reference for object '{instance}'!");
                IntPtr key = JNIEnv.IdentityHash(handle);
                if ((transfer & JniHandleOwnership.DoNotRegister) == JniHandleOwnership.DoNotTransfer)
                    _RegisterInstance(instance, key, handle);
                if (instance is IJavaObjectEx javaObjectEx)
                    javaObjectEx.KeyHandle = key;
            }
        }

        private static void _RegisterInstance(IJavaObject instance, IntPtr key, IntPtr handle)
        {
            lock (instances)
            {
                if (!instances.TryGetValue(key, out List<WeakReference> weakReferenceList1))
                {
                    weakReferenceList1 = new List<WeakReference>(1)
                    {
                        new WeakReference(instance, true)
                    };
                    
                    instances.Add(key, weakReferenceList1);
                }
                else
                {
                    bool flag = false;
                    for (int index = 0; index < weakReferenceList1.Count; ++index)
                    {
                        WeakReference current = weakReferenceList1[index];

                        if (ShouldReplaceMapping(current, handle))
                        {
                            flag = true;
                            weakReferenceList1.Remove(current);
                            weakReferenceList1.Add(new WeakReference(instance, true));
                            break;
                        }

                        var javaObject = (IJavaObject)current?.Target;
                        var ref2 = javaObject?.Handle ?? default;

                        if (ref2 != IntPtr.Zero && JNIEnv.IsSameObject(handle, ref2))
                        {
                            flag = true;
                            break;
                        }
                    }

                    if (flag) return;
                    weakReferenceList1.Add(new WeakReference(instance, true));
                }
            }
        }

        private static bool ShouldReplaceMapping(WeakReference current, IntPtr handle)
        {
            if (current == null)
                return true;
            object target = current.Target;
            if (target == null)
                return true;
            IJavaObject javaObject = (IJavaObject)target;
            return javaObject.Handle == IntPtr.Zero || JNIEnv.IsSameObject(javaObject.Handle, handle) && target is IJavaObjectEx javaObjectEx && javaObjectEx.IsProxy;
        }

        internal static void DeregisterInstance(object instance, IntPtr key_handle)
        {
            lock (instances)
            {
                if (!instances.TryGetValue(key_handle, out List<WeakReference> weakReferenceList))
                    return;

                for (int index = weakReferenceList.Count - 1; index >= 0; --index)
                {
                    WeakReference weakReference = weakReferenceList[index];
                    if (weakReference.Target == null || weakReference.Target == instance)
                        weakReferenceList.RemoveAt(index);
                }

                if (weakReferenceList.Count != 0)
                    return;
                instances.Remove(key_handle);
            }
        }

        internal static List<WeakReference> GetSurfacedObjects_ForDiagnosticsOnly()
        {
            lock (instances)
            {
                var weakReferenceList = new List<WeakReference>(instances.Count);
                foreach (KeyValuePair<IntPtr, List<WeakReference>> instance in instances)
                    weakReferenceList.AddRange(instance.Value);
                return weakReferenceList;
            }
        }

        internal static IJavaObject PeekObject(IntPtr handle, Type requiredType = null)
        {
            if (handle == IntPtr.Zero) return null;

            lock (instances)
            {
                if (instances.TryGetValue(JNIEnv.IdentityHash(handle), out List<WeakReference> weakReferenceList))
                {
                    for (int index = 0; index < weakReferenceList.Count; ++index)
                    {
                        if (weakReferenceList[index].Target is IJavaObject target && target.Handle != IntPtr.Zero && JNIEnv.IsSameObject(handle, target.Handle))
                            return requiredType != null && !requiredType.IsAssignableFrom(target.GetType()) ? null : target;
                    }
                }
            }

            return null;
        }

        public static T GetObject<T>(IntPtr jnienv, IntPtr handle, JniHandleOwnership transfer) where T : class, IJavaObject
        {
            JNIEnv.CheckHandle(jnienv);
            return GetObject<T>(handle, transfer);
        }

        public static T GetObject<T>(IntPtr handle, JniHandleOwnership transfer) where T : class, IJavaObject
        {
            return handle == IntPtr.Zero ? default : (T)GetObject(handle, transfer, typeof(T));
        }

        internal static IJavaObject GetObject(
            IntPtr handle,
            JniHandleOwnership transfer,
            Type type = null)
        {
            if (handle == IntPtr.Zero) return null;

            lock (instances)
            {
                if (instances.TryGetValue(JNIEnv.IdentityHash(handle), out List<WeakReference> weakReferenceList))
                {
                    for (int index = 0; index < weakReferenceList.Count; ++index)
                    {
                        if (weakReferenceList[index].Target is IJavaObject target && target.Handle != IntPtr.Zero && JNIEnv.IsSameObject(handle, target.Handle) && (type == null ? 1 : (type.IsAssignableFrom(target.GetType()) ? 1 : 0)) != 0)
                        {
                            JNIEnv.DeleteRef(handle, transfer);
                            return target;
                        }
                    }
                }
            }
            
            return TypeManager.CreateInstance(handle, transfer, type);
        }

        public T[] ToArray<T>() => JNIEnv.GetArray<T>(this.Handle);

        public static implicit operator Object(string value)
        {
            return value == null ? null : new ICharSequenceInvoker(JNIEnv.NewString(value), JniHandleOwnership.TransferLocalRef);
        }

        public static explicit operator float(Object value) => Convert.ToSingle(value);

        public static explicit operator double(Object value) => Convert.ToDouble(value);

        public static implicit operator Object(Object[] value) => value == null ? null : new Object(JNIEnv.NewArray(value), JniHandleOwnership.TransferLocalRef);

        public static explicit operator Object[](Object value) => value?.ToArray<Object>();

    }*/
}
