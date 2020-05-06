using Android.Runtime;
using Java.Interop;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Java.Util
{
    public partial class HashMap : IDictionary, IJavaDictionary
    {
        object IDictionary.this[object key] { get => Get((Java.Lang.Object)key); set => Put((Java.Lang.Object)key, (Java.Lang.Object)value); }
        public bool IsFixedSize => false;
        public bool IsReadOnly => false;
        System.Collections.ICollection IDictionary.Keys => KeySet();
        System.Collections.ICollection IDictionary.Values => Values();
        public int Count => Size();
        public bool IsSynchronized => false;
        public object SyncRoot => throw new InvalidOperationException();
        void IDictionary.Add(object key, object value) => Put((Java.Lang.Object)key, (Java.Lang.Object)value);
        void IDictionary.Clear() => Clear();
        bool IDictionary.Contains(object key) => ContainsKey((Java.Lang.Object)key);
        void IDictionary.Remove(object key) => Remove((Java.Lang.Object)key);
        IEnumerator IEnumerable.GetEnumerator() => ((IDictionary)this).GetEnumerator();
        IDictionaryEnumerator IDictionary.GetEnumerator() => new DictionaryEnumerator(Entries.GetEnumerator());

        void System.Collections.ICollection.CopyTo(Array array, int arrayIndex)
        {
            int count = Count;
            if (arrayIndex + count > array.Length || arrayIndex < 0)
                throw new IndexOutOfRangeException();
            foreach (var item in this)
                array.SetValue(item, arrayIndex++);
        }

        internal virtual unsafe ICollection<IMapEntryInvoker> Entries
        {
            [Register("entrySet", "()Ljava/util/Set;", "GetEntrySetHandler")]
            get
            {
                const string __id = "entrySet.()Ljava/util/Set;";
                try {
                    var __rm = _members.InstanceMethods.InvokeVirtualObjectMethod(__id, this, null);
                    return new ISetInvoker<IMapEntryInvoker>(ref __rm, Interop.JniObjectReferenceOptions.CopyAndDispose);
                } finally {
                }
            }
        }

        private class DictionaryEnumerator : IDictionaryEnumerator
        {
            private readonly IEnumerator<IMapEntry> _enumerator;
            public DictionaryEnumerator(IEnumerator<IMapEntry> enumerator) => _enumerator = enumerator;
            public object Key => _enumerator.Current.Key;
            public object Value => _enumerator.Current.Value;
            public object Current => Entry;
            public void Reset() => _enumerator.Reset();
            public bool MoveNext() => _enumerator.MoveNext();
            public DictionaryEntry Entry => new DictionaryEntry(_enumerator.Current.Key, _enumerator.Current.Value);
        }

        public HashMap(IDictionary source) : this()
        {
            foreach (var key in source.Keys) ((IDictionary)this)[key] = source[key];
        }
    }

    [JniTypeSignature ("java/util/HashMap", GenerateJavaPeer = false)]
    [Register("java/util/HashMap", DoNotGenerateAcw = true)]
    [JavaTypeParameters(new string[] { "K", "V" })]
    public partial class HashMap<K, V> : HashMap, IDictionary<K, V>, IJavaDictionary<K, V> where K : Java.Lang.Object where V : Java.Lang.Object
    {
        protected HashMap(ref JniObjectReference reference, JniObjectReferenceOptions options) : base(ref reference, options) { }

        [Register(".ctor", "()V", "")]
        public unsafe HashMap() : base() { }

        [Register(".ctor", "(I)V", "")]
        public unsafe HashMap(int p0) : base(p0) { }

        [Register(".ctor", "(IF)V", "")]
        public unsafe HashMap(int p0, float p1) : base(p0, p1) { }

        public HashMap(IDictionary<K, V> source) : base()
        {
            foreach (var kvp in source) this[kvp.Key] = kvp.Value;
        }

        public void Add(K key, V value) => Put(key, value);
        public void Add(KeyValuePair<K, V> item) => Put(item.Key, item.Value);
        public V this[K key] { get => Get(key).JavaCast<V>(); set => Put(key, value); }

        public unsafe ICollection<K> Keys
        {
            get
            {
                const string __id = "keySet.()Ljava/util/Set;";
                try {
                    var __rm = JniPeerMembers.InstanceMethods.InvokeVirtualObjectMethod(__id, this, null);
                    return new ISetInvoker<K>(ref __rm, JniObjectReferenceOptions.CopyAndDispose);
                } finally {
                }
            }
        }

        public unsafe new ICollection<V> Values
        {
            get
            {
                const string __id = "values.()Ljava/util/Collection;";
                try {
                    var __rm = JniPeerMembers.InstanceMethods.InvokeVirtualObjectMethod(__id, this, null);
                    return new ICollectionInvoker<V>(ref __rm, JniObjectReferenceOptions.CopyAndDispose);
                } finally {
                }
            }
        }

        public bool Contains(KeyValuePair<K, V> item)
        {
            return base.ContainsKey(item.Key) && base.Get(item.Key) == item.Value;
        }

        public bool ContainsKey(K key) => base.ContainsKey(key);

        public void CopyTo(KeyValuePair<K, V>[] array, int arrayIndex)
        {
            int count = Count;
            if (arrayIndex + count > array.Length || arrayIndex < 0)
                throw new IndexOutOfRangeException();
            foreach (var item in this)
                array[arrayIndex++] = item;
        }

        public IEnumerator<KeyValuePair<K, V>> GetEnumerator()
        {
            return Entries
                .Select(kvp => new KeyValuePair<K, V>(kvp.Key.JavaCast<K>(), kvp.Value.JavaCast<V>()))
                .GetEnumerator();
        }

        public bool Remove(K key)
        {
            if (!base.ContainsKey(key)) return false;
            base.Remove(key);
            return true;
        }

        public bool Remove(KeyValuePair<K, V> item) => base.Remove(item.Key, item.Value);

        public bool TryGetValue(K key, out V value)
        {
            if (ContainsKey(key))
            {
                value = this[key];
                return true;
            }
            else
            {
                value = null;
                return false;
            }
        }
    }
    
    partial class IMapInvoker : IDictionary, IJavaDictionary
    {
        object IDictionary.this[object key] { get => Get((Java.Lang.Object)key); set => Put((Java.Lang.Object)key, (Java.Lang.Object)value); }
        public bool IsFixedSize => false;
        public bool IsReadOnly => false;
        System.Collections.ICollection IDictionary.Keys => KeySet();
        System.Collections.ICollection IDictionary.Values => Values();
        public int Count => Size();
        public bool IsSynchronized => false;
        public object SyncRoot => throw new InvalidOperationException();
        void IDictionary.Add(object key, object value) => Put((Java.Lang.Object)key, (Java.Lang.Object)value);
        void IDictionary.Clear() => Clear();
        bool IDictionary.Contains(object key) => ContainsKey((Java.Lang.Object)key);
        void IDictionary.Remove(object key) => Remove((Java.Lang.Object)key);
        IEnumerator IEnumerable.GetEnumerator() => ((IDictionary)this).GetEnumerator();
        IDictionaryEnumerator IDictionary.GetEnumerator() => new DictionaryEnumerator(Entries.GetEnumerator());

        void System.Collections.ICollection.CopyTo(Array array, int arrayIndex)
        {
            int count = Count;
            if (arrayIndex + count > array.Length || arrayIndex < 0)
                throw new IndexOutOfRangeException();
            foreach (var item in this)
                array.SetValue(item, arrayIndex++);
        }

        internal virtual unsafe ICollection<IMapEntryInvoker> Entries
        {
            [Register("entrySet", "()Ljava/util/Set;", "GetEntrySetHandler")]
            get
            {
                const string __id = "entrySet.()Ljava/util/Set;";
                try
                {
                    var __rm = _members.InstanceMethods.InvokeVirtualObjectMethod(__id, this, null);
                    return new ISetInvoker<IMapEntryInvoker>(ref __rm, Interop.JniObjectReferenceOptions.CopyAndDispose);
                }
                finally
                {
                }
            }
        }

        private class DictionaryEnumerator : IDictionaryEnumerator
        {
            private readonly IEnumerator<IMapEntry> _enumerator;
            public DictionaryEnumerator(IEnumerator<IMapEntry> enumerator) => _enumerator = enumerator;
            public object Key => _enumerator.Current.Key;
            public object Value => _enumerator.Current.Value;
            public object Current => Entry;
            public void Reset() => _enumerator.Reset();
            public bool MoveNext() => _enumerator.MoveNext();
            public DictionaryEntry Entry => new DictionaryEntry(_enumerator.Current.Key, _enumerator.Current.Value);
        }
    }

    [JniTypeSignature ("java/util/Map", GenerateJavaPeer = false)]
    [Register("java/util/Map", DoNotGenerateAcw = true)]
    [JavaTypeParameters(new string[] { "K", "V" })]
    partial class IMapInvoker<K, V> : IMapInvoker, IDictionary<K, V>, IJavaDictionary<K, V> where K : Java.Lang.Object where V : Java.Lang.Object
    {
        protected IMapInvoker(ref JniObjectReference reference, JniObjectReferenceOptions options) : base(ref reference, options) { }

        public void Add(K key, V value) => Put(key, value);
        public void Add(KeyValuePair<K, V> item) => Put(item.Key, item.Value);
        public V this[K key] { get => Get(key).JavaCast<V>(); set => Put(key, value); }

        public unsafe ICollection<K> Keys
        {
            get
            {
                const string __id = "keySet.()Ljava/util/Set;";
                try
                {
                    var __rm = JniPeerMembers.InstanceMethods.InvokeVirtualObjectMethod(__id, this, null);
                    return new ISetInvoker<K>(ref __rm, JniObjectReferenceOptions.CopyAndDispose);
                }
                finally
                {
                }
            }
        }

        public unsafe new ICollection<V> Values
        {
            get
            {
                const string __id = "values.()Ljava/util/Collection;";
                try
                {
                    var __rm = JniPeerMembers.InstanceMethods.InvokeVirtualObjectMethod(__id, this, null);
                    return new ICollectionInvoker<V>(ref __rm, JniObjectReferenceOptions.CopyAndDispose);
                }
                finally
                {
                }
            }
        }

        public bool Contains(KeyValuePair<K, V> item)
        {
            return base.ContainsKey(item.Key) && base.Get(item.Key) == item.Value;
        }

        public bool ContainsKey(K key) => base.ContainsKey(key);

        public void CopyTo(KeyValuePair<K, V>[] array, int arrayIndex)
        {
            int count = Count;
            if (arrayIndex + count > array.Length || arrayIndex < 0)
                throw new IndexOutOfRangeException();
            foreach (var item in this)
                array[arrayIndex++] = item;
        }

        public IEnumerator<KeyValuePair<K, V>> GetEnumerator()
        {
            return Entries
                .Select(kvp => new KeyValuePair<K, V>(kvp.Key.JavaCast<K>(), kvp.Value.JavaCast<V>()))
                .GetEnumerator();
        }

        public bool Remove(K key)
        {
            if (!base.ContainsKey(key)) return false;
            base.Remove(key);
            return true;
        }

        public bool Remove(KeyValuePair<K, V> item)
        {
            if (!TryGetValue(item.Key, out V val))
                return false;
            if (!JniEnvironment.Types.IsSameObject(val.PeerReference, item.Key.PeerReference))
                return false;
            Remove(item.Key);
            return true;
        }

        public bool TryGetValue(K key, out V value)
        {
            if (ContainsKey(key))
            {
                value = this[key];
                return true;
            }
            else
            {
                value = null;
                return false;
            }
        }
    }
}
