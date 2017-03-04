using System;
using System.Runtime.InteropServices;

using JetBrains.Annotations;

namespace CorApi.ComInterop
{
    public static unsafe class Com
    {
        public static TInterface QueryInteface<TInterface>([NotNull] object comobj) where TInterface : class
        {
            if(comobj == null)
                throw new ArgumentNullException("The COM object instance is NULL.", default(Exception));
            if(!Marshal.IsComObject(comobj))
                throw new ArgumentOutOfRangeException("comobj", comobj, "This object is not a COM object instance.");
            var comobjQI = comobj as TInterface;
            if(comobjQI == null)
                throw new ArgumentOutOfRangeException("comobj", comobj, string.Format("This COM object does not implement the {0} ({1}) interface.", typeof(TInterface).Name, typeof(TInterface).GUID.ToString("B").ToUpperInvariant()));
            return comobjQI;
        }

        public static TInterface QueryInteface<TInterface>([NotNull] void* punk) where TInterface : class
        {
            if(punk == null)
                throw new ArgumentNullException("The COM object instance is NULL.", default(Exception));
            object oUnk;
            try
            {
                oUnk = Marshal.GetObjectForIUnknown((IntPtr)punk);
            }
            catch(Exception ex)
            {
                throw new ArgumentOutOfRangeException("The IUnknown pointer is not a COM object. " + ex.Message, ex);
            }
            return QueryInteface<TInterface>(oUnk);
        }

        /// <summary>
        /// Assumes the parameter is a COM object's RCW, or a managed COMable object. Does QueryInterface for IUnknown on it and returns the raw IUnknown pointer, which also adds one more reference on the object.
        /// </summary>
        /// <param name="comobj"></param>
        /// <returns></returns>
        [NotNull]
        public static void* UnknownAddRef([NotNull] object comobj)
        {
            if(comobj == null)
                throw new ArgumentNullException(nameof(comobj));
            if(!Marshal.IsComObject(comobj))
                throw new ArgumentOutOfRangeException("comobj", comobj, "This object is not a COM object instance.");
            return ((void*)Marshal.GetIUnknownForObject(comobj));
        }

        public static void UnknownRelease([CanBeNull] void* punk)
        {
            if(punk != null)
                Marshal.Release((IntPtr)punk);
        }

        public static ReleaseToken UsingReference([NotNull] void** punk)
        {
            return new ReleaseToken(punk);
        }

        public struct ReleaseToken : IDisposable
        {
            private void** _punk;

            public ReleaseToken(void** punk)
            {
                if(punk == null)
                    throw new ArgumentNullException("punk");
                _punk = punk;
            }

            /// <inheritdoc />
            public void Dispose()
            {
                void** punk = _punk;
                _punk = null;
                if(punk == null)
                    return;
                UnknownRelease(*punk);
            }
        }
    }
}