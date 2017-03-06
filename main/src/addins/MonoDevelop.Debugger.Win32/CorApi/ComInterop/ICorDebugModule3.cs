using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace CorApi.ComInterop
{
  /// <summary>
  /// ICorDebugModule3 is a logical extension to ICorDebugModule.
  /// </summary>
  /// <example><code>
  /// /*
  /// * ICorDebugModule3 is a logical extension to ICorDebugModule.
  /// */
  ///[
  ///    object,
  ///    local,
  ///    uuid(86F012BF-FF15-4372-BD30-B6F11CAAE1DD),
  ///    pointer_default(unique)
  ///]
  ///interface ICorDebugModule3 : IUnknown
  ///{
  ///    /*
  ///     * CreateReaderForInMemorySymbols creates a debug symbol reader object (eg. 
  ///     * ISymUnmanagedReader) for a dynamic module.  This symbol reader becomes stale
  ///     * and is usually discarded whenever a LoadClass callback is delivered for the
  ///     * module.
  ///     *
  ///     * Arguments:
  ///     *    riid - The IID of the COM interface to return (typically IID_ISymUnmanagedReader)
  ///     *   ppObj - Where to store the reader interface.
  ///     *
  ///     * Return Value:
  ///     *   S_OK on success
  ///     *   Error hresults otherwise, including:
  ///     *   CORDBG_E_MODULE_LOADED_FROM_DISK if this isn't an in-memory or dynamic module
  ///     *   CORDBG_E_SYMBOLS_NOT_AVAILABLE if symbols weren't supplied by the application or aren't
  ///     *      yet available.
  ///     *
  ///     * Notes:
  ///     *   This API can also be used to create a symbol reader object for in-memory 
  ///     *   (non-dynamic) modules, but only after the symbols are first available
  ///     *   (indicated by the UpdateModuleSymbols callback).
  ///     *
  ///     *   This API returns a new reader instance every time it is called (like CoCreateInstance)
  ///     *   and so the debugger should cache the result and only request a new one when
  ///     *   the underlying data may have changed (i.e. a LoadClass event).
  ///     *
  ///     *   Dynamic modules do not have any symbols available until the first type has been
  ///     *   loaded into them (as indicated by the LoadClass callback).  
  ///     */
  ///    HRESULT CreateReaderForInMemorySymbols([in] REFIID riid,
  ///                                           [out][iid_is(riid)] void **ppObj);
  ///}
  /// </code></example>
    [InterfaceType (ComInterfaceType.InterfaceIsIUnknown)]
    [Guid ("86F012BF-FF15-4372-BD30-B6F11CAAE1DD")]
    [ComImport]
    public unsafe interface ICorDebugModule3
    {
      /// <summary>
      /// CreateReaderForInMemorySymbols creates a debug symbol reader object (eg. 
      /// ISymUnmanagedReader) for a dynamic module.  This symbol reader becomes stale
      /// and is usually discarded whenever a LoadClass callback is delivered for the
      /// module.
      /// 
      /// Arguments:
      ///    riid - The IID of the COM interface to return (typically IID_ISymUnmanagedReader)
      ///   ppObj - Where to store the reader interface.
      /// 
      /// Return Value:
      ///   S_OK on success
      ///   Error hresults otherwise, including:
      ///   CORDBG_E_MODULE_LOADED_FROM_DISK if this isn't an in-memory or dynamic module
      ///   CORDBG_E_SYMBOLS_NOT_AVAILABLE if symbols weren't supplied by the application or aren't
      ///      yet available.
      /// 
      /// Notes:
      ///   This API can also be used to create a symbol reader object for in-memory 
      ///   (non-dynamic) modules, but only after the symbols are first available
      ///   (indicated by the UpdateModuleSymbols callback).
      /// 
      ///   This API returns a new reader instance every time it is called (like CoCreateInstance)
      ///   and so the debugger should cache the result and only request a new one when
      ///   the underlying data may have changed (i.e. a LoadClass event).
      /// 
      ///   Dynamic modules do not have any symbols available until the first type has been
      ///   loaded into them (as indicated by the LoadClass callback).   
      /// </summary>
      /// <param name="riid"></param>
      /// <param name="ppObj"></param>
        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] System.Int32 CreateReaderForInMemorySymbols ([In] Guid *riid, void **ppObj);
    }
}