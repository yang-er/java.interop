using Java.Interop;
using System;

namespace Java.Lang
{
    public partial class Throwable : JavaException
    {
        public static Throwable FromException(Exception ex)
        {
            if (ex is Throwable ex2) return ex2;
            return new Throwable(ex.ToString());
        }
    }
}
