using Android.Runtime;
using Java.Interop;
using System;

namespace Java.Util
{
    public static partial class InteroperableArrays
    {
        #region JavaArray<T> NewArray(T[] array)

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

        #endregion

        #region T[] GetArray<T>(JniObject ..)

        public static T[] GetArray<T>(IntPtr handle, JniHandleOwnership ownership)
        {
            if (handle == IntPtr.Zero) return null;
            var @ref = new JniObjectReference(handle, (JniObjectReferenceType)ownership);
            return GetArray<T>(ref @ref, JniObjectReferenceOptions.CopyAndDispose);
        }

        public static T[] GetArray<T>(ref JniObjectReference handle, JniObjectReferenceOptions options)
        {
            return (T[])_GetArray<T>(ref handle, options);
        }

        internal static object _GetArray<T>(ref JniObjectReference handle, JniObjectReferenceOptions options)
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

        #endregion

        #region void ArrayCopy

        public static unsafe void ArrayCopy(int[] array, IntPtr dest)
        {
            var destSrc = new JniObjectReference(dest);
            fixed (int* t = array)
                JniEnvironment.Arrays.SetIntArrayRegion(destSrc, 0, array.Length, t);
        }

        public static unsafe void ArrayCopy(char[] array, IntPtr dest)
        {
            var destSrc = new JniObjectReference(dest);
            fixed (char* t = array)
                JniEnvironment.Arrays.SetCharArrayRegion(destSrc, 0, array.Length, t);
        }

        public static unsafe void ArrayCopy(short[] array, IntPtr dest)
        {
            var destSrc = new JniObjectReference(dest);
            fixed (short* t = array)
                JniEnvironment.Arrays.SetShortArrayRegion(destSrc, 0, array.Length, t);
        }

        public static unsafe void ArrayCopy(long[] array, IntPtr dest)
        {
            var destSrc = new JniObjectReference(dest);
            fixed (long* t = array)
                JniEnvironment.Arrays.SetLongArrayRegion(destSrc, 0, array.Length, t);
        }

        public static unsafe void ArrayCopy(float[] array, IntPtr dest)
        {
            var destSrc = new JniObjectReference(dest);
            fixed (float* t = array)
                JniEnvironment.Arrays.SetFloatArrayRegion(destSrc, 0, array.Length, t);
        }

        public static unsafe void ArrayCopy(double[] array, IntPtr dest)
        {
            var destSrc = new JniObjectReference(dest);
            fixed (double* t = array)
                JniEnvironment.Arrays.SetDoubleArrayRegion(destSrc, 0, array.Length, t);
        }

        public static unsafe void ArrayCopy(bool[] array, IntPtr dest)
        {
            var destSrc = new JniObjectReference(dest);
            fixed (bool* t = array)
                JniEnvironment.Arrays.SetBooleanArrayRegion(destSrc, 0, array.Length, t);
        }

        public static unsafe void ArrayCopy(sbyte[] array, IntPtr dest)
        {
            var destSrc = new JniObjectReference(dest);
            fixed (sbyte* t = array)
                JniEnvironment.Arrays.SetByteArrayRegion(destSrc, 0, array.Length, t);
        }

        public static unsafe void ArrayCopy(string[] array, IntPtr dest)
        {
            var destSrc = new JniObjectReference(dest);
            for (int i = 0; i < array.Length; i++)
                JniEnvironment.Arrays.SetObjectArrayElement(destSrc, i, new Java.Lang.String(array[i]).PeerReference);
        }

        public static unsafe void ArrayCopy<T>(T[] array, IntPtr dest) where T : IJavaPeerable
        {
            var destSrc = new JniObjectReference(dest);
            for (int i = 0; i < array.Length; i++)
                JniEnvironment.Arrays.SetObjectArrayElement(destSrc, i, array[i].PeerReference);
        }

        #endregion
    }
}
