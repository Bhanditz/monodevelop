using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using JetBrains.Annotations;

namespace CorApi.ComInterop
{
  /// <summary>
  /// ICorDebugThread2 is a logical extension to ICorDebugThread.
  /// </summary>
  /// <example><code>
  /// 
  ////*
  ///  * ICorDebugThread2 is a logical extension to ICorDebugThread.
  ///  */
  ///[
  ///    object,
  ///    local,
  ///    uuid(2BD956D9-7B07-4bef-8A98-12AA862417C5),
  ///    pointer_default(unique)
  ///]
  ///interface ICorDebugThread2 : IUnknown
  ///{
  ///
  ///    typedef struct _COR_ACTIVE_FUNCTION
  ///    {
  ///        ICorDebugAppDomain *pAppDomain;   // Pointer to the owning AppDomain of the below IL Offset.
  ///        ICorDebugModule *pModule;         // Pointer to the owning Module of the below IL Offset.
  ///        ICorDebugFunction2 *pFunction;    // Pointer to the owning Function of the below IL Offset.
  ///        ULONG32 ilOffset;                 // IL Offset of the frame.
  ///        ULONG32 flags;                    // Bit mask of flags, currently unused.  Reserved.
  ///    } COR_ACTIVE_FUNCTION;
  ///
  ///    /*
  ///     * Retrieves the active functions for the given threads' frames. This
  ///     * includes AppDomain ID, Module ID, Funtion ID and IL offset for
  ///     * each active statement on the stack.  A flags field is also included
  ///     * for future information about the frame that might need to be conveyed.
  ///     *
  ///     * If pFunctions is NULL, returns only the number of functions that
  ///     * is on the stack in pcFunctions.
  ///     */
  ///
  ///    HRESULT GetActiveFunctions([in] ULONG32 cFunctions,
  ///                               [out] ULONG32 *pcFunctions,
  ///                               [in, out, size_is(cFunctions), length_is(*pcFunctions)]
  ///                               COR_ACTIVE_FUNCTION pFunctions[]
  ///                               );
  ///    /*
  ///     * Returns 0 if not part of a connection
  ///     * Maps to a SPID in SQL Server
  ///     */
  ///    HRESULT GetConnectionID(
  ///        [out] CONNID *pdwConnectionId);
  ///
  ///    /*
  ///     * Return the TASKID of this thread.
  ///    */
  ///    HRESULT GetTaskID(
  ///        [out] TASKID *pTaskId);
  ///
  ///    /*
  ///     * Return the OS Thread ID
  ///     */
  ///    HRESULT GetVolatileOSThreadID(
  ///        [out] DWORD *pdwTid);
  ///
  ///    /*
  ///     * Allow the debugger to intercept the current exception on a thread.  It can be
  ///     * called between an Exception callback and the associated call to ICorDebugProcess::Continue.
  ///     *
  ///     * pFrame specifies where we should intercept the exception.  It must be a valid ICDFrame pointer,
  ///     * which can be obtained from a stackwalk.  However, you must not call Continue() between
  ///     * doing the stackwalk and calling this function.
  ///     */
  ///    HRESULT InterceptCurrentException(
  ///        [in] ICorDebugFrame *pFrame);
  ///}
  ///
  /// </code></example>
    [InterfaceType (ComInterfaceType.InterfaceIsIUnknown)]
    [Guid ("2BD956D9-7B07-4BEF-8A98-12AA862417C5")]
    [ComImport]
    [SuppressMessage("ReSharper", "BuiltInTypeReferenceStyle")]
  public unsafe interface ICorDebugThread2
    {
      /// <summary>
      /// Retrieves the active functions for the given threads' frames. This
      /// includes AppDomain ID, Module ID, Funtion ID and IL offset for
      /// each active statement on the stack.  A flags field is also included
      /// for future information about the frame that might need to be conveyed.
      /// 
      /// If pFunctions is NULL, returns only the number of functions that
      /// is on the stack in pcFunctions.
      /// </summary>
      /// <param name="cFunctions"></param>
      /// <param name="pcFunctions"></param>
      /// <param name="pFunctions"></param>
        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        [PreserveSig][MustUseReturnValue]Int32 GetActiveFunctions ([In] UInt32 cFunctions, UInt32* pcFunctions, COR_ACTIVE_FUNCTION* pFunctions);

      /// <summary>
      /// Returns 0 if not part of a connection
      /// Maps to a SPID in SQL Server
      /// </summary>
      /// <param name="pdwConnectionId"></param>
        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] System.Int32 GetConnectionID ([ComAliasName("CONNID")]UInt32* pdwConnectionId);

      /// <summary>
      /// Return the TASKID of this thread.
      /// </summary>
      /// <param name="pTaskId"></param>
        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] System.Int32 GetTaskID ([ComAliasName("TASKID")] UInt64 *pTaskId);

      /// <summary>
      /// Return the OS Thread ID
      /// </summary>
      /// <param name="pdwTid"></param>
        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] System.Int32 GetVolatileOSThreadID (UInt32 *pdwTid);

      /// <summary>
      /// Allow the debugger to intercept the current exception on a thread.  It can be
      /// called between an Exception callback and the associated call to ICorDebugProcess::Continue.
      /// 
      /// pFrame specifies where we should intercept the exception.  It must be a valid ICDFrame pointer,
      /// which can be obtained from a stackwalk.  However, you must not call Continue() between
      /// doing the stackwalk and calling this function.
      /// </summary>
      /// <param name="pFrame"></param>
        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] System.Int32 InterceptCurrentException ([MarshalAs (UnmanagedType.Interface), In] ICorDebugFrame pFrame);
    }
}