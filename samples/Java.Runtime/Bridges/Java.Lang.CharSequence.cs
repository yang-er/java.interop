using System;
using System.Collections.Generic;

namespace Java.Lang
{
    public partial interface ICharSequence : IEnumerable<char>
    {

    }

    internal partial class ICharSequenceInvoker
    {
        public static IntPtr ToLocalJniHandle(ICharSequence vs)
        {
            var @ref = vs.PeerReference.NewLocalRef();
            return @ref.Handle;
        }
    }
    
    partial class String
    {
        public static explicit operator String(string value) => new String(value);

        public static explicit operator string(String value) => value.ToString();
    }
}
