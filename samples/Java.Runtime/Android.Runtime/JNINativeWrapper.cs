using Java.Interop;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading;

namespace Android.Runtime
{
    public static class JNINativeWrapper
    {
        private static MethodInfo mono_unhandled_exception_method;
        private static MethodInfo exception_handler_method;
        private static MethodInfo wait_for_bridge_processing_method;

        internal class DynamicMethodNameCounter
        {
            private static long dynamicMethodNameCounter;

            internal static string GetUniqueName()
            {
                return Interlocked.Increment(ref dynamicMethodNameCounter).ToString(CultureInfo.InvariantCulture);
            }
        }

        internal static void UnhandledException(System.Exception e)
        {
            // AndroidEnvironment.UnhandledException -> AppDomain.CurrentDomain.UnhandledException?
            JniEnvironment.Runtime.RaisePendingException(e);
        }

        private static void get_runtime_types()
        {
            if (mono_unhandled_exception_method != null)
                return;
            mono_unhandled_exception_method = typeof(Debugger).GetMethod("Mono_UnhandledException", BindingFlags.Static | BindingFlags.NonPublic);
            if (mono_unhandled_exception_method == null)
                JniEnvironment.Runtime.FailFast("Cannot find System.Diagnostics.Debugger.Mono_UnhandledException");
            exception_handler_method = typeof(JNINativeWrapper).GetMethod("UnhandledException", BindingFlags.Static | BindingFlags.NonPublic);
            if (exception_handler_method == null)
                JniEnvironment.Runtime.FailFast("Cannot find AndroidEnvironment.UnhandledException");
            wait_for_bridge_processing_method = typeof(JniEnvironment.References).GetMethod("WaitForBridgeProcessing", BindingFlags.Static | BindingFlags.Public);
            if (wait_for_bridge_processing_method == null)
                JniEnvironment.Runtime.FailFast("Cannot find JNIEnv.WaitForBridgeProcessing");
        }

        public static Delegate CreateDelegate(Delegate dlg)
        {
            if (dlg == null)
                throw new ArgumentNullException();
            if (dlg.Target != null)
                throw new ArgumentException();
            if (dlg.Method == null)
                throw new ArgumentException();
            get_runtime_types();
            Type returnType = dlg.Method.ReturnType;
            ParameterInfo[] parameters = dlg.Method.GetParameters();
            Type[] parameterTypes = new Type[parameters.Length];
            for (int index = 0; index < parameters.Length; ++index)
                parameterTypes[index] = parameters[index].ParameterType;
            DynamicMethod dynamicMethod = new DynamicMethod(DynamicMethodNameCounter.GetUniqueName(), returnType, parameterTypes, typeof(DynamicMethodNameCounter), true);
            ILGenerator ilGenerator = dynamicMethod.GetILGenerator();
            LocalBuilder local = null;
            if (returnType != typeof(void))
                local = ilGenerator.DeclareLocal(returnType);
            ilGenerator.Emit(OpCodes.Call, wait_for_bridge_processing_method);
            Label label = ilGenerator.BeginExceptionBlock();
            for (int index = 0; index < parameterTypes.Length; ++index)
                ilGenerator.Emit(OpCodes.Ldarg, index);
            ilGenerator.Emit(OpCodes.Call, dlg.Method);
            if (local != null)
                ilGenerator.Emit(OpCodes.Stloc, local);
            ilGenerator.Emit(OpCodes.Leave, label);
            bool flag = Debugger.IsAttached;// || !JNIEnv.PropagateExceptions;

            if (flag)
            {
                ilGenerator.BeginExceptFilterBlock();
                ilGenerator.Emit(OpCodes.Call, mono_unhandled_exception_method);
                ilGenerator.Emit(OpCodes.Ldc_I4_1);
                ilGenerator.BeginCatchBlock(null);
            }
            else
            {
                ilGenerator.BeginCatchBlock(typeof(Exception));
            }

            ilGenerator.Emit(OpCodes.Dup);
            ilGenerator.Emit(OpCodes.Call, exception_handler_method);
            if (flag)
                ilGenerator.Emit(OpCodes.Throw);
            ilGenerator.EndExceptionBlock();
            if (local != null)
                ilGenerator.Emit(OpCodes.Ldloc, local);
            ilGenerator.Emit(OpCodes.Ret);
            return dynamicMethod.CreateDelegate(dlg.GetType());
        }
    }
}
