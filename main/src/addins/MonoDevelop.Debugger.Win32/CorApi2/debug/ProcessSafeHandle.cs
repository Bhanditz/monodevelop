using System;

using CorApi.Pinvoke;

using Microsoft.Win32.SafeHandles;

namespace Microsoft.Samples.Debugging.CorDebug
{
    public class ProcessSafeHandle : SafeHandleZeroOrMinusOneIsInvalid
    {
        private ProcessSafeHandle()
            : base(true)
        {
        }

        public unsafe ProcessSafeHandle(void* handle, bool ownsHandle)
            : base(ownsHandle)
        {
            SetHandle((IntPtr)handle);
        }

        public unsafe void* Value => (void*)DangerousGetHandle();

        protected override unsafe bool ReleaseHandle()
        {
            return Kernel32Dll.CloseHandle(Value) != 0;
        }
    }
}