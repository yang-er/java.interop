using System;
using System.Threading;
using Java.Interop;
using String = Java.Lang.String;

namespace Hello
{
    public static class MainClass
    {
        private static unsafe void TryExplicitlyCallJni()
        {
            Console.WriteLine("Part2: Check JNI Calls...");
            
            var vm = JniRuntime.CurrentRuntime;
            Console.WriteLine("# JniEnvironment.EnvironmentPointer={0}", JniEnvironment.EnvironmentPointer);
            Console.WriteLine("vm.SafeHandle={0}", vm.InvocationPointer);
            var t = new JniType("java/lang/Object");
            var c = t.GetConstructor("()V");
            var o = t.NewObject(c, null);
            var m = t.GetInstanceMethod("hashCode", "()I");
            int i = JniEnvironment.InstanceMethods.CallIntMethod(o, m);
            Console.WriteLine("java.lang.Object={0}", o);
            Console.WriteLine("hashcode={0}", i);
            JniObjectReference.Dispose(ref o);
            t.Dispose();
            
            // var o = JniTypes.FindClass ("java/lang/Object");
            /*
            var waitForCreation = new CountdownEvent (1);
            var exitThread = new CountdownEvent (1);
            var t = new Thread (() => {
                var vm2 = new JavaVMBuilder ().CreateJavaVM ();
                waitForCreation.Signal ();
                exitThread.Wait ();
            });
            t.Start ();
            waitForCreation.Wait ();
            */
        }

        private static void TryImplicitlyInteroperate()
        {
            Console.WriteLine("Part3: Call by real-interop");
            
            var item = new Java.Lang.Object();
            Console.WriteLine(item.ToString());
            
            var item2 = new Java.Lang.String("Hello java!");
            Console.WriteLine(((Java.Lang.Object)item2).ToString());
            
            var item3 = new Java.Util.HashMap();
            Console.WriteLine(item3.ToString());
            
            var map = new Java.Util.HashMap<Java.Lang.String, Java.Lang.Integer>();
            map.Add((String)"One hundred", 100);
            map.Add((String)"Two hundred", 200);
            map.Add((String) "Hello", -100000);

            foreach (var kvp in map)
            {
                var key = (string) kvp.Key;
                var value = (int) kvp.Value;
                Console.WriteLine($"{key} : {value}");
            }
            
            Console.WriteLine(map.ToString());
        }

        private static void ListInvocationPoints(string type)
        {
            foreach (var h in JniRuntime.GetAvailableInvocationPointers())
                Console.WriteLine("{1}: GetCreatedJavaVMs: {0}", h, type);
        }
        
        public static unsafe void Main(string[] args)
        {
            JreRuntime.Initialize("/usr/lib/jvm/java-11-openjdk-amd64/lib/server/libjvm.so");
            Console.WriteLine("Hello World!");
            bool hosted = false;
            JniRuntime vm = null;

            try
            {
                vm = JniRuntime.CurrentRuntime;
                hosted = vm != null;
            }
            catch (InvalidOperationException e)
            {
                Console.WriteLine(e);
            }

            ListInvocationPoints("PRE");

            if (vm == null)
                vm = new JreRuntimeOptions().CreateJreVM();

            TryExplicitlyCallJni();
            ListInvocationPoints("WITHIN");
            TryImplicitlyInteroperate();
            
            if (!hosted) vm.Dispose();
            ListInvocationPoints("POST");
        }
    }
}