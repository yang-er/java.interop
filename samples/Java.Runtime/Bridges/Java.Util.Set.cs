using Java.Interop;
using Java.Lang;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Java.Util
{
    partial class ISetInvoker : System.Collections.ICollection, IJavaSet
    {
        public int Count => Size();

        bool System.Collections.ICollection.IsSynchronized => false;

        object System.Collections.ICollection.SyncRoot => throw new InvalidOperationException();

        void System.Collections.ICollection.CopyTo(Array array, int arrayIndex)
        {
            if (array.Rank != 1)
                throw new InvalidOperationException();
            int count = Count;
            if (arrayIndex + count > array.Length || arrayIndex < 0)
                throw new IndexOutOfRangeException();
            foreach (var item in this)
                array.SetValue(item, arrayIndex++);
        }

        IEnumerator IEnumerable.GetEnumerator() => Iterator().AsEnumerator();
    }

    [JniTypeSignature ("java/util/Set", GenerateJavaPeer = false)]
    [global::Android.Runtime.Register("java/util/Set", DoNotGenerateAcw = true)]
    [JavaTypeParameters(new string[] { "T" })]
    internal partial class ISetInvoker<T> : ISetInvoker, ICollection<T>, IIterable<T>, IJavaSet<T> where T : Java.Lang.Object
    {
        public ISetInvoker(ref JniObjectReference reference, JniObjectReferenceOptions options) : base(ref reference, options) { }

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
}
