using System;
using System.Diagnostics.CodeAnalysis;

using CorApi.Pinvoke;

using JetBrains.Annotations;

namespace CorApi.ComInterop
{
    [SuppressMessage("ReSharper", "BuiltInTypeReferenceStyle")]
    public static unsafe class LpcwstrHelper
    {
        [ThreadStatic]
        private static UInt16[] _buffer;

        [NotNull]
        public static string GetString([NotNull] GetLpcwstrDelegate func, [NotNull] string faultmsg)
        {
            if(func == null)
                throw new ArgumentNullException(nameof(func));
            if(faultmsg == null)
                throw new ArgumentNullException(nameof(faultmsg));
            ushort[] buffer = _buffer ?? new UInt16[MemoryUtil.ByteBufferUnderlohSize / sizeof(UInt16)];
            _buffer = null;

            try
            {
                // Call with the current buffer size
                UInt32 cchActual;
                int hr;
                uint cchBuffer = (uint)buffer.Length - 1; // To always have a zero at the end no matter what
                fixed(UInt16* pBuffer = buffer)
                    hr = func(cchBuffer, pBuffer, &cchActual);

                if((hr != (int)HResults.E_INSUFFICIENT_BUFFER) || (cchActual <= cchBuffer))
                {
                    // Does not say it won't fit
                    hr.AssertSucceeded(faultmsg); // Check that no errors
                    buffer[buffer.Length - 1] = 0; // Ensure again it's not an infinite ASCIIZ
                    fixed(UInt16* pBuffer = buffer)
                        return new string((char*)pBuffer);
                }
                else
                {
                    // Too short, make a new buffer
                    buffer = new UInt16[Math.Max(cchActual, buffer.Length << 1)];
                    cchBuffer = (uint)buffer.Length - 1;
                    fixed(UInt16* pBuffer = buffer)
                        func(cchBuffer, pBuffer, &cchActual).AssertSucceeded(faultmsg); // This time won't check for nonfitting strings anymore
                    buffer[buffer.Length - 1] = 0; // Ensure again it's not an infinite ASCIIZ
                    fixed(UInt16* pBuffer = buffer)
                        return new string((char*)pBuffer);
                }
            }
            finally
            {
                _buffer = buffer;
            }
        }

        public delegate Int32 GetLpcwstrDelegate(UInt32 cchBuffer, UInt16*pBuffer, UInt32*pcchActual);
    }
}