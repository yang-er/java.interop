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
}
