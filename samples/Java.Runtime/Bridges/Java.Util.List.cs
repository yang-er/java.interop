using Android.Runtime;
using Java.Interop;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Java.Util
{
    public partial class ArrayList : System.Collections.IList, IJavaList
    {
        object System.Collections.IList.this[int index] { get => Get(index); set => Set(index, (Java.Lang.Object)value); }
        public bool IsFixedSize => false;
        public bool IsReadOnly => false;
        public int Count => Size();
        public bool IsSynchronized => false;
        public object SyncRoot => throw new NotSupportedException();
        bool System.Collections.IList.Contains(object value) => Contains((Java.Lang.Object)value);
        IEnumerator IEnumerable.GetEnumerator() => Iterator().AsEnumerator();
        int System.Collections.IList.IndexOf(object value) => IndexOf((Java.Lang.Object)value);
        void System.Collections.IList.Insert(int index, object value) => Add(index, (Java.Lang.Object)value);
        void System.Collections.IList.Remove(object value) => Remove((Java.Lang.Object)value);
        public void RemoveAt(int index) => Remove(index);

        public ArrayList(IEnumerable<Java.Lang.Object> objects) : this()
        {
            if (objects == null)
            {
                Dispose();
                throw new ArgumentNullException(nameof(objects));
            }

            foreach (var item in objects) Add(item);
        }

        public int Add(object value)
        {
            if (!Add((Java.Lang.Object)value)) return -1;
            return Count - 1;
        }
        
        public void CopyTo(Array array, int arrayIndex)
        {
            if (array.Rank != 1)
                throw new InvalidOperationException();
            int count = Count;
            if (arrayIndex + count > array.Length || arrayIndex < 0)
                throw new IndexOutOfRangeException();
            for (int i = arrayIndex; i < arrayIndex + count; i++)
                array.SetValue(Get(i - arrayIndex), i);
        }
    }

    [Register("java/util/ArrayList", DoNotGenerateAcw = true)]
    [JavaTypeParameters(new string[] { "T" })]
    public partial class ArrayList<T> :
        Java.Util.ArrayList,
        Java.Lang.IIterable<T>,
        IJavaList<T>,
        IList<T> where T : Java.Lang.Object
    {
        public void Add(T item) => base.Add(item);
        public bool Contains(T item) => base.Contains(item);
        public int IndexOf(T item) => base.IndexOf(item);
        public void Insert(int index, T item) => Add(index, item);
        public bool Remove(T item) => base.Remove(item);
        public IEnumerator<T> GetEnumerator() => Iterator().AsEnumerator();

        public unsafe T this[int index]
        {
            set => Set(index, value);
            get
            {
                const string __id = "get.(I)Ljava/lang/Object;";
                try {
                    JniArgumentValue* __args = stackalloc JniArgumentValue[1];
                    __args[0] = new JniArgumentValue(index);
                    var __rm = JniPeerMembers.InstanceMethods.InvokeVirtualObjectMethod(__id, this, __args);
                    return GetObject<T>(ref __rm, JniObjectReferenceOptions.CopyAndDispose);
                } finally {
                }
            }
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            int count = Count;
            if (arrayIndex + count > array.Length || arrayIndex < 0)
                throw new IndexOutOfRangeException();
            for (int i = arrayIndex; i < arrayIndex + count; i++)
                array[i] = this[i - arrayIndex];
        }

        public unsafe new IIterator<T> Iterator()
        {
            const string __id = "iterator.()Ljava/util/Iterator;";
            try {
                var __rm = JniPeerMembers.InstanceMethods.InvokeVirtualObjectMethod(__id, this, null);
                return new IIteratorInvoker<T>(ref __rm, JniObjectReferenceOptions.CopyAndDispose);
            } finally {
            }
        }

        protected ArrayList(ref JniObjectReference reference, JniObjectReferenceOptions options) : base(ref reference, options) { }
        public ArrayList() : base() { }
        public ArrayList(int p0) : base(p0) { }
        public ArrayList(IEnumerable<T> p0) : base(p0) { }
    }
    
    internal partial class IListInvoker : System.Collections.IList, IJavaList
    {
        object System.Collections.IList.this[int index] { get => Get(index); set => Set(index, (Java.Lang.Object)value); }
        public bool IsFixedSize => false;
        public bool IsReadOnly => false;
        public int Count => Size();
        public bool IsSynchronized => false;
        public object SyncRoot => throw new NotSupportedException();
        bool System.Collections.IList.Contains(object value) => Contains((Java.Lang.Object)value);
        IEnumerator IEnumerable.GetEnumerator() => Iterator().AsEnumerator();
        int System.Collections.IList.IndexOf(object value) => IndexOf((Java.Lang.Object)value);
        void System.Collections.IList.Insert(int index, object value) => Add(index, (Java.Lang.Object)value);
        void System.Collections.IList.Remove(object value) => Remove((Java.Lang.Object)value);
        public void RemoveAt(int index) => Remove(index);

        public int Add(object value)
        {
            if (!Add((Java.Lang.Object)value)) return -1;
            return Count - 1;
        }

        public void CopyTo(Array array, int arrayIndex)
        {
            if (array.Rank != 1)
                throw new InvalidOperationException();
            int count = Count;
            if (arrayIndex + count > array.Length || arrayIndex < 0)
                throw new IndexOutOfRangeException();
            for (int i = arrayIndex; i < arrayIndex + count; i++)
                array.SetValue(Get(i - arrayIndex), i);
        }
    }

    [Register("java/util/ArrayList", DoNotGenerateAcw = true)]
    [JavaTypeParameters(new string[] { "T" })]
    internal partial class IListInvoker<T> :
        Java.Util.IListInvoker,
        Java.Lang.IIterable<T>,
        IJavaList<T>,
        IList<T> where T : Java.Lang.Object
    {
        public void Add(T item) => base.Add(item);
        public bool Contains(T item) => base.Contains(item);
        public int IndexOf(T item) => base.IndexOf(item);
        public void Insert(int index, T item) => Add(index, item);
        public bool Remove(T item) => base.Remove(item);
        public IEnumerator<T> GetEnumerator() => Iterator().AsEnumerator();

        public unsafe T this[int index]
        {
            set => Set(index, value);
            get
            {
                const string __id = "get.(I)Ljava/lang/Object;";
                try
                {
                    JniArgumentValue* __args = stackalloc JniArgumentValue[1];
                    __args[0] = new JniArgumentValue(index);
                    var __rm = JniPeerMembers.InstanceMethods.InvokeVirtualObjectMethod(__id, this, __args);
                    return GetObject<T>(ref __rm, JniObjectReferenceOptions.CopyAndDispose);
                }
                finally
                {
                }
            }
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            int count = Count;
            if (arrayIndex + count > array.Length || arrayIndex < 0)
                throw new IndexOutOfRangeException();
            for (int i = arrayIndex; i < arrayIndex + count; i++)
                array[i] = this[i - arrayIndex];
        }

        public unsafe new IIterator<T> Iterator()
        {
            const string __id = "iterator.()Ljava/util/Iterator;";
            try
            {
                var __rm = JniPeerMembers.InstanceMethods.InvokeVirtualObjectMethod(__id, this, null);
                return new IIteratorInvoker<T>(ref __rm, JniObjectReferenceOptions.CopyAndDispose);
            }
            finally
            {
            }
        }

        protected IListInvoker(ref JniObjectReference reference, JniObjectReferenceOptions options) : base(ref reference, options) { }
    }
}
