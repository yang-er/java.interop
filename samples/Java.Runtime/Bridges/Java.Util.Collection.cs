using Android.Runtime;
using Java.Interop;
using Java.Lang;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Java.Util
{
    partial class ICollectionInvoker : IJavaCollection
    {
        public int Count => Size();
        bool System.Collections.ICollection.IsSynchronized => false;
        object System.Collections.ICollection.SyncRoot => throw new InvalidOperationException();
        IEnumerator IEnumerable.GetEnumerator() => Iterator().AsEnumerator();

        void System.Collections.ICollection.CopyTo(Array array, int arrayIndex)
        {
            int count = Count;
            if (arrayIndex + count > array.Length || arrayIndex < 0)
                throw new IndexOutOfRangeException();
            foreach (var item in this)
                array.SetValue(item, arrayIndex++);
        }
    }

    [Register("java/util/Collection", DoNotGenerateAcw = true)]
    internal partial class ICollectionInvoker<T> : ICollectionInvoker, ICollection<T>, IJavaCollection<T> where T : Java.Lang.Object
    {
        public ICollectionInvoker(ref JniObjectReference reference, JniObjectReferenceOptions options) : base(ref reference, options) { }

        public bool IsReadOnly => true;
        public void Add(T item) => base.Add(item);
        public bool Contains(T item) => base.Contains(item);
        public bool Remove(T item) => base.Remove(item);
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        public IEnumerator<T> GetEnumerator() => Iterator().AsEnumerator();

        public void CopyTo(T[] array, int arrayIndex)
        {
            int count = Count;
            if (arrayIndex + count > array.Length || arrayIndex < 0)
                throw new IndexOutOfRangeException();
            foreach (var item in this)
                array[arrayIndex++] = item;
        }

        public unsafe new IIterator<T> Iterator()
        {
            const string __id = "iterator.()Ljava/util/Iterator;";
            try {
                var __rm = JniPeerMembers.InstanceMethods.InvokeAbstractObjectMethod(__id, this, null);
                return new IIteratorInvoker<T>(ref __rm, JniObjectReferenceOptions.CopyAndDispose);
            } finally {
            }
        }
    }

    public static partial class InteroperableArrays
    {
        public static IJavaSet ToLocalJavaSet(ref JniObjectReference reference, JniObjectReferenceOptions options)
        {
            return new ISetInvoker(ref reference, options);
        }

        public static IJavaCollection ToLocalJavaCollection(ref JniObjectReference reference, JniObjectReferenceOptions options)
        {
            return new ICollectionInvoker(ref reference, options);
        }

        public static IJavaDictionary ToLocalJavaDictionary(ref JniObjectReference reference, JniObjectReferenceOptions options)
        {
            return new IMapInvoker(ref reference, options);
        }

        public static IJavaList ToLocalJavaList(ref JniObjectReference reference, JniObjectReferenceOptions options)
        {
            return new IListInvoker(ref reference, options);
        }

        public static IJavaSet ToLocalJavaSet(IntPtr handle, JniHandleOwnership ownership)
        {
            var reference = new JniObjectReference(handle, (JniObjectReferenceType)ownership);
            var options = JniObjectReferenceOptions.None;
            return new ISetInvoker(ref reference, options);
        }

        public static IJavaCollection ToLocalJavaCollection(IntPtr handle, JniHandleOwnership ownership)
        {
            var reference = new JniObjectReference(handle, (JniObjectReferenceType)ownership);
            var options = JniObjectReferenceOptions.None;
            return new ICollectionInvoker(ref reference, options);
        }

        public static IJavaDictionary ToLocalJavaDictionary(IntPtr handle, JniHandleOwnership ownership)
        {
            var reference = new JniObjectReference(handle, (JniObjectReferenceType)ownership);
            var options = JniObjectReferenceOptions.None;
            return new IMapInvoker(ref reference, options);
        }

        public static IJavaList ToLocalJavaList(IntPtr handle, JniHandleOwnership ownership)
        {
            var reference = new JniObjectReference(handle, (JniObjectReferenceType)ownership);
            var options = JniObjectReferenceOptions.None;
            return new IListInvoker(ref reference, options);
        }

        public static Java.Util.ICollection ToInteroperableCollection(this System.Collections.ICollection instance)
        {
            return instance is Java.Util.ICollection ? (Java.Util.ICollection)instance : new ArrayList(instance.Cast<Java.Lang.Object>());
        }

        public static Java.Util.ArrayList<T> ToInteroperableCollection<T>(this ICollection<T> instance) where T : Java.Lang.Object
        {
            return instance is ArrayList<T> ? (ArrayList<T>)instance : new ArrayList<T>(instance);
        }

        public static Java.Util.IList ToInteroperableCollection(this System.Collections.IList instance)
        {
            return instance is Java.Util.IList ? (Java.Util.IList)instance : new ArrayList(instance.Cast<Java.Lang.Object>());
        }

        public static Java.Util.HashMap ToInteroperableCollection(this IDictionary instance)
        {
            return instance is HashMap ic2 ? ic2 : new HashMap(instance);
        }

        public static Java.Util.HashMap<K, V> ToInteroperableCollection<K, V>(this IDictionary<K, V> instance) where K : Java.Lang.Object where V : Java.Lang.Object
        {
            return instance is HashMap<K, V> ic2 ? ic2 : new HashMap<K, V>(instance);
        }
    }
}

namespace Java.Interop
{
    public interface IJavaSet : System.Collections.ICollection, Java.Util.ISet, IJavaCollection
    {
    }

    public interface IJavaSet<T> : System.Collections.Generic.ICollection<T>, Java.Util.ISet, IJavaCollection<T>
    {
    }

    public interface IJavaList : System.Collections.IList, Java.Util.IList, IJavaCollection
    {
    }

    public interface IJavaList<T> : System.Collections.Generic.IList<T>, Java.Util.ISet, IJavaCollection<T>
    {
    }

    public interface IJavaCollection : System.Collections.ICollection, Java.Util.ICollection
    {
    }

    public interface IJavaCollection<T> : System.Collections.Generic.ICollection<T>, Java.Util.ICollection
    {
    }

    public interface IJavaDictionary : System.Collections.IDictionary, Java.Util.IMap
    {
    }

    public interface IJavaDictionary<K, V> : System.Collections.Generic.IDictionary<K, V>, Java.Util.IMap
    {
    }
}
