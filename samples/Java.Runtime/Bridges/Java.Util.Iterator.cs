using Android.Runtime;
using Java.Interop;
using Java.Lang;
using Java.Util;
using System.Collections;
using System.Collections.Generic;

namespace Java
{
    namespace Lang
    {
        [Register("java/lang/Iterable", "", "Java.Lang.IIterableInvoker")]
        [global::Java.Interop.JavaTypeParameters(new string[] { "T" })]
        public partial interface IIterable<T> : IIterable where T : Java.Lang.Object
        {
            [Register("iterator", "()Ljava/util/Iterator;", "GetIteratorHandler:Java.Lang.IIterableInvoker, ")]
            new Java.Util.IIterator<T> Iterator();
        }

		[Register("java/lang/Iterable", DoNotGenerateAcw = true)]
        [JniTypeSignature("java/lang/Iterable", GenerateJavaPeer = false)]
		internal partial class IIterableInvoker<T> : IIterableInvoker, IIterable<T> where T : Java.Lang.Object
		{
			public IIterableInvoker(ref JniObjectReference handle, JniObjectReferenceOptions options) : base(ref handle, options) { }

			public new unsafe Java.Util.IIterator<T> Iterator()
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

	namespace Util
    {
        [Register("java/util/Iterator", "", "Java.Util.IIteratorInvoker")]
        [global::Java.Interop.JavaTypeParameters(new string[] { "E" })]
        public partial interface IIterator<E> : IIterator where E : Java.Lang.Object
        {
            [Register("next", "()Ljava/lang/Object;", "GetNextHandler:Java.Util.IIteratorInvoker, ")]
            new E Next();
        }

        [JniTypeSignature("java/util/Iterator", GenerateJavaPeer = false)]
		[Register("java/util/Iterator", DoNotGenerateAcw = true)]
		internal partial class IIteratorInvoker<E> : IIteratorInvoker, IIterator<E> where E : Java.Lang.Object
		{
			public IIteratorInvoker(ref JniObjectReference handle, JniObjectReferenceOptions options) : base(ref handle, options)
			{
			}

            public unsafe new E Next()
            {
                const string __id = "next.()Ljava/lang/Object;";
                try {
                    var __rm = JniPeerMembers.InstanceMethods.InvokeAbstractObjectMethod(__id, this, null);
                    return GetObject<E>(ref __rm, JniObjectReferenceOptions.CopyAndDispose);
                } finally {
                }
            }
        }

		public static class JavaIteratorExtensions
        {
            private static IEnumerable ToBeEnumerable(this IIterator iterator)
            {
                while (iterator.HasNext) yield return iterator.Next();
            }

            private static IEnumerable<E> ToBeEnumerable<E>(this IIterator<E> iterator) where E : Java.Lang.Object
            {
                while (iterator.HasNext) yield return iterator.Next();
            }

            public static IEnumerable AsEnumerable(this IIterable iterable)
                => iterable.Iterator().ToBeEnumerable();

            public static IEnumerable<T> AsEnumerable<T>(this IIterable<T> iterable) where T : Java.Lang.Object
                => iterable.Iterator().ToBeEnumerable();

            public static IEnumerator AsEnumerator(this IIterator iterator)
                => iterator.ToBeEnumerable().GetEnumerator();

            public static IEnumerator<E> AsEnumerator<E>(this IIterator<E> iterator) where E : Java.Lang.Object
                => iterator.ToBeEnumerable().GetEnumerator();
        }
    }
}
