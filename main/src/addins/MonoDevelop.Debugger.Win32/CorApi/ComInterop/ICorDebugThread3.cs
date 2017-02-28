using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace CorApi.ComInterop
{
  /// <summary>
  /// ICorDebugThread3 is a logical extension to ICorDebugThread.
  /// </summary>
  /// <example><code>
  /// 
  ////*
  /// * ICorDebugThread3 is a logical extension to ICorDebugThread.
  /// */
  ///[
  ///    object,
  ///    local,
  ///    uuid(F8544EC3-5E4E-46c7-8D3E-A52B8405B1F5),
  ///    pointer_default(unique)
  ///]
  ///interface ICorDebugThread3 : IUnknown
  ///{
  ///    HRESULT CreateStackWalk([out] ICorDebugStackWalk **ppStackWalk);
  ///
  ///    HRESULT GetActiveInternalFrames([in] ULONG32 cInternalFrames,
  ///                                    [out] ULONG32 *pcInternalFrames,
  ///                                    [in, out, size_is(cInternalFrames), length_is(*pcInternalFrames)]
  ///                                    ICorDebugInternalFrame2 * ppInternalFrames[]
  ///                                    );
  ///};
  ///
  /// </code></example>
    [Guid ("F8544EC3-5E4E-46C7-8D3E-A52B8405B1F5")]
    [InterfaceType (ComInterfaceType.InterfaceIsIUnknown)]
    [ComImport]
    public unsafe interface ICorDebugThread3
    {
        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void CreateStackWalk ([MarshalAs (UnmanagedType.Interface)] out ICorDebugStackWalk ppStackWalk);

        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void GetActiveInternalFrames ([In] UInt32 cInternalFrames, UInt32* pcInternalFrames, void** ppInternalFrames);
    }
}