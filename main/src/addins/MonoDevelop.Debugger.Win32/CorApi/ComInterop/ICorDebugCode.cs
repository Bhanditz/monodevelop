using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace CorApi.ComInterop
{
  /// <summary>
  ///  ICorDebugCode represents an IL or native code blob.
  ///  For methods that take offsets, the units are the same as the units on the CordbCode object.
  ///  (eg, IL offsets for an IL code object, and native offsets for a native code object)
  ///  V2 allows multiple code-regions. CordbCode presents an abstraction where these
  ///  are merged together in a single linear, continuous space. So if the code is split
  ///  with 0x5 bytes at address 0x1000, and 0x10 bytes at address 0x2000,
  ///  then:
  ///  - GetAddress() yields a start address of 0x1000.
  ///  - GetSize() is the size of the merged regions = 0x5+ 0x10 = 0x15 bytes.
  ///  - The (Offset --&gt; Address) mapping is:
  ///         0x0  --&gt; 0x1000
  ///         0x1  --&gt; 0x1001
  ///         0x4  --&gt; 0x1004
  ///         0x5  --&gt; 0x2000
  ///         0x6  --&gt; 0x2001
  ///         0x15 --&gt; 0x2010
  ///  A caller can get the specific code regions via ICorDebugCode2.
  /// </summary>
  /// <example><code>
  ///  
  ///  /*
  ///     ICorDebugCode represents an IL or native code blob.
  ///  
  ///     For methods that take offsets, the units are the same as the units on the CordbCode object.
  ///     (eg, IL offsets for an IL code object, and native offsets for a native code object)
  ///  
  ///     V2 allows multiple code-regions. CordbCode presents an abstraction where these
  ///     are merged together in a single linear, continuous space. So if the code is split
  ///     with 0x5 bytes at address 0x1000, and 0x10 bytes at address 0x2000, 
  ///     then:
  ///     - GetAddress() yields a start address of 0x1000.
  ///     - GetSize() is the size of the merged regions = 0x5+ 0x10 = 0x15 bytes.
  ///     - The (Offset --&gt; Address) mapping is:
  ///            0x0  --&gt; 0x1000
  ///            0x1  --&gt; 0x1001
  ///            0x4  --&gt; 0x1004
  ///            0x5  --&gt; 0x2000
  ///            0x6  --&gt; 0x2001
  ///            0x15 --&gt; 0x2010
  /// 
  ///     A caller can get the specific code regions via ICorDebugCode2.
  /// */
  /// [
  ///     object,
  ///     local,
  ///     uuid(CC7BCAF4-8A68-11d2-983C-0000F808342D),
  ///     pointer_default(unique)
  /// ]
  /// interface ICorDebugCode : IUnknown
  /// {
  ///     /*
  ///      * IsIL returns whether the code is IL (as opposed to native.)
  ///      */
  /// 
  ///     HRESULT IsIL([out] BOOL *pbIL);
  /// 
  ///     /*
  ///      * GetFunction returns the function for the code.
  ///      */
  /// 
  ///     HRESULT GetFunction([out] ICorDebugFunction **ppFunction);
  /// 
  ///     /*
  ///      * GetAddress returns the address of the code.
  ///      */
  /// 
  ///     HRESULT GetAddress([out] CORDB_ADDRESS *pStart);
  /// 
  ///     /*
  ///      * GetSize returns the size in bytes of the code.
  ///      */
  /// 
  ///     HRESULT GetSize([out] ULONG32 *pcBytes);
  /// 
  ///     /*
  ///      * CreateBreakpoint creates a breakpoint in the function at the
  ///      * given offset. 
  ///      *
  ///      * If this code is IL code, and there is a jitted native version
  ///      * of the code, the breakpoint will be applied in the jitted code
  ///      * as well.  (The same is true if the code is later jitted.)
  ///      *
  ///      */
  /// 
  ///     HRESULT CreateBreakpoint([in] ULONG32 offset,
  ///                              [out] ICorDebugFunctionBreakpoint **ppBreakpoint);
  /// 
  ///     /*
  ///      * GetCode returns the code of the method, suitable for disassembly.  Note
  ///      * that instruction boundaries aren't checked.
  ///      * This glues together multiple code-regions into a single binary stream.
  ///      * Caller must use ICorDebugCode2::GetCodeChunks to get (start,size) for
  ///      * code chunks to be able to properly resolve addresses embedded in the instructions.
  ///      */
  /// 
  ///     HRESULT GetCode([in] ULONG32 startOffset, [in] ULONG32 endOffset,
  ///                     [in] ULONG32 cBufferAlloc,
  ///                     [out, size_is(cBufferAlloc),
  ///                           length_is(*pcBufferSize)] BYTE buffer[],
  ///                     [out] ULONG32 *pcBufferSize);
  /// 
  ///     /*
  ///      * GetVersionNumber returns the 1 based number identifying the
  ///      * version of the code that this ICorDebugCode corresponds to.  The
  ///      * version number is incremented each time the function is Edit-And-
  ///      * Continue'd.
  ///      */
  /// 
  ///     HRESULT GetVersionNumber([out] ULONG32 *nVersion);
  /// 
  ///     /*
  ///      * GetILToNativeMapping returns a map from IL offsets to native
  ///      * offsets for this code. An array of COR_DEBUG_IL_TO_NATIVE_MAP
  ///      * structs will be returned, and some of the ilOffsets in this array
  ///      * map be the values specified in CorDebugIlToNativeMappingTypes.
  ///      *
  ///      * Note: this method is only valid for ICorDebugCodes representing
  ///      * native code that was jitted from IL code.
  ///      * Note: There is no ordering to the array of elements returned, nor
  ///      * should you assume that there is or will be.
  ///      */
  ///     HRESULT GetILToNativeMapping([in] ULONG32 cMap,
  ///                                  [out] ULONG32 *pcMap,
  ///                                  [out, size_is(cMap), length_is(*pcMap)]
  ///                                  COR_DEBUG_IL_TO_NATIVE_MAP map[]);
  /// 
  ///     /*
  ///      * Not implemented.
  ///      */
  ///     HRESULT GetEnCRemapSequencePoints([in] ULONG32 cMap,
  ///                                       [out] ULONG32 *pcMap,
  ///                                       [out, size_is(cMap), length_is(*pcMap)]
  ///                                       ULONG32 offsets[]);
  /// };
  ///  </code></example>
  [InterfaceType (ComInterfaceType.InterfaceIsIUnknown)]
  [Guid ("CC7BCAF4-8A68-11D2-983C-0000F808342D")]
  [ComImport]
  [SuppressMessage ("ReSharper", "BuiltInTypeReferenceStyle")]
  public unsafe interface ICorDebugCode
  {
    /// <summary>
    /// IsIL returns whether the code is IL (as opposed to native.)
    /// </summary>
    /// <param name="pbIL"></param>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] System.Int32 IsIL (Int32* pbIL);

    /// <summary>
    /// GetFunction returns the function for the code.
    /// </summary>
    /// <param name="ppFunction"></param>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] System.Int32 GetFunction ([MarshalAs (UnmanagedType.Interface)] out ICorDebugFunction ppFunction);

    /// <summary>
    /// GetAddress returns the address of the code.
    /// </summary>
    /// <param name="pStart"></param>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] System.Int32 GetAddress ([ComAliasName ("CORDB_ADDRESS")] UInt64* pStart);

    /// <summary>
    /// GetSize returns the size in bytes of the code.
    /// </summary>
    /// <param name="pcBytes"></param>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] System.Int32 GetSize (UInt32* pcBytes);

    /// <summary>
    /// CreateBreakpoint creates a breakpoint in the function at the
    /// given offset.
    /// If this code is IL code, and there is a jitted native version
    /// of the code, the breakpoint will be applied in the jitted code
    /// as well.  (The same is true if the code is later jitted.)
    /// </summary>
    /// <param name="offset"></param>
    /// <param name="ppBreakpoint"></param>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] System.Int32 CreateBreakpoint ([In] UInt32 offset, [MarshalAs (UnmanagedType.Interface)] out ICorDebugFunctionBreakpoint ppBreakpoint);

    /// <summary>
    /// GetCode returns the code of the method, suitable for disassembly.  Note
    /// that instruction boundaries aren't checked.
    /// This glues together multiple code-regions into a single binary stream.
    /// Caller must use ICorDebugCode2::GetCodeChunks to get (start,size) for
    /// code chunks to be able to properly resolve addresses embedded in the instructions.
    /// </summary>
    /// <param name="startOffset"></param>
    /// <param name="endOffset"></param>
    /// <param name="cBufferAlloc"></param>
    /// <param name="buffer"></param>
    /// <param name="pcBufferSize"></param>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] System.Int32 GetCode ([In] UInt32 startOffset, [In] UInt32 endOffset, [In] UInt32 cBufferAlloc, Byte* buffer, UInt32* pcBufferSize);

    /// <summary>
    /// GetVersionNumber returns the 1 based number identifying the
    /// version of the code that this ICorDebugCode corresponds to.  The
    /// version number is incremented each time the function is Edit-And-
    /// Continue'd.
    /// </summary>
    /// <param name="nVersion"></param>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] System.Int32 GetVersionNumber (UInt32* nVersion);

    /// <summary>
    /// GetILToNativeMapping returns a map from IL offsets to native
    /// offsets for this code. An array of COR_DEBUG_IL_TO_NATIVE_MAP
    /// structs will be returned, and some of the ilOffsets in this array
    /// map be the values specified in CorDebugIlToNativeMappingTypes.
    /// Note: this method is only valid for ICorDebugCodes representing
    /// native code that was jitted from IL code.
    /// Note: There is no ordering to the array of elements returned, nor
    /// should you assume that there is or will be.
    /// </summary>
    /// <param name="cMap"></param>
    /// <param name="pcMap"></param>
    /// <param name="map"></param>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] System.Int32 GetILToNativeMapping ([In] UInt32 cMap, UInt32* pcMap, COR_DEBUG_IL_TO_NATIVE_MAP* map);

    /// <summary>
    /// Not implemented.
    /// </summary>
    /// <param name="cMap"></param>
    /// <param name="pcMap"></param>
    /// <param name="offsets"></param>
    [Obsolete ("Not implemented.")]
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] System.Int32 GetEnCRemapSequencePoints ([In] UInt32 cMap, UInt32* pcMap, UInt32* offsets);
  }
}