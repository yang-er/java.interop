using System;

namespace Java.Interop
{
    public static partial class PeerableExtensions
    {
        public static TResult JavaCast<TResult>(this IJavaPeerable instance) where TResult : class, IJavaPeerable
        {
            if (instance == null) return default;
            if (instance is TResult result) return result;
            Type type = typeof(TResult);
            if (type.IsClass)
                return (TResult)CastClass(instance, type);
            var peer = instance.PeerReference;
            if (type.IsInterface)
                return (TResult)Java.Lang.Object.GetObject(ref peer, JniObjectReferenceOptions.Copy, type);
            throw new NotSupportedException($"Unable to convert type '{instance.GetType().FullName}' to '{type.FullName}'.");
        }

        private static IJavaPeerable CastClass(IJavaPeerable instance, Type resultType)
        {
            var reference = instance.PeerReference;
            return JniRuntime.CurrentRuntime.ValueManager.GetValue<IJavaPeerable>(ref reference, JniObjectReferenceOptions.Copy, resultType);
        }

        internal static IJavaPeerable JavaCast(IJavaPeerable instance, Type resultType)
        {
            if (resultType == null)
                throw new ArgumentNullException(nameof(resultType));
            if (instance == null)
                return null;
            if (resultType.IsAssignableFrom(instance.GetType()))
                return instance;
            if (resultType.IsClass)
                return CastClass(instance, resultType);
            var peer = instance.PeerReference;
            if (resultType.IsInterface)
                return Java.Lang.Object.GetObject(ref peer, JniObjectReferenceOptions.Copy, resultType);
            throw new NotSupportedException($"Unable to convert type '{instance.GetType().FullName}' to '{resultType.FullName}'.");
        }
    }
}
