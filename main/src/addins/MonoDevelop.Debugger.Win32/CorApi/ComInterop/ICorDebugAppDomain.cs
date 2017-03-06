using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using JetBrains.Annotations;

namespace CorApi.ComInterop
{
    /// <summary>
    /// AppDomain interface
    /// </summary>
    /// <example><code>[
    ///     object,
    ///     local,
    ///     uuid(3d6f5f63-7538-11d3-8d5b-00104b35e7ef),
    ///     pointer_default(unique)
    /// ]</code></example>
    [Guid ("3D6F5F63-7538-11D3-8D5B-00104B35E7EF")]
    [InterfaceType (ComInterfaceType.InterfaceIsIUnknown)]
    [ComImport]
    [SuppressMessage ("ReSharper", "BuiltInTypeReferenceStyle")]
    public unsafe interface ICorDebugAppDomain : ICorDebugController
    {
        /// <inheritdoc cref="ICorDebugController.Stop"/>
        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] new System.Int32 Stop ([In] UInt32 dwTimeoutIgnored);

        /// <inheritdoc cref="ICorDebugController.Continue"/>
        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] new System.Int32 Continue ([In] Int32 fIsOutOfBand);

        /// <inheritdoc cref="ICorDebugController.IsRunning"/>
        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] new System.Int32 IsRunning (Int32 *pbRunning);

        /// <inheritdoc cref="ICorDebugController.HasQueuedCallbacks"/>
        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] new System.Int32 HasQueuedCallbacks ([MarshalAs (UnmanagedType.Interface), In] ICorDebugThread pThread, Int32 *pbQueued);

        /// <inheritdoc cref="ICorDebugController.EnumerateThreads"/>
        [MethodImpl(MethodImplOptions.InternalCall | MethodImplOptions.PreserveSig, MethodCodeType = MethodCodeType.Runtime)]
        [PreserveSig]
        [MustUseReturnValue]
        new Int32 EnumerateThreads ([MarshalAs (UnmanagedType.Interface)] out ICorDebugThreadEnum ppThreads);

        /// <inheritdoc cref="ICorDebugController.SetAllThreadsDebugState"/>
        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] new System.Int32 SetAllThreadsDebugState ([In] CorDebugThreadState state,
            [MarshalAs (UnmanagedType.Interface), In] ICorDebugThread pExceptThisThread);

        /// <inheritdoc cref="ICorDebugController.Detach"/>
        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] new System.Int32 Detach ();

        /// <inheritdoc cref="ICorDebugController.Terminate"/>
        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] new System.Int32 Terminate ([In] UInt32 exitCode);

        /// <inheritdoc cref="ICorDebugController.CanCommitChanges"/>
      [Obsolete ("DEPRECATED")]
      [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] new System.Int32 CanCommitChanges ([In] UInt32 cSnapshots, /*ICorDebugEditAndContinueSnapshot*/void** pSnapshots, /*ICorDebugErrorInfoEnum*/void** pError);

        /// <inheritdoc cref="ICorDebugController.CommitChanges"/>
      [Obsolete ("DEPRECATED")]
      [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] new System.Int32 CommitChanges ([In] UInt32 cSnapshots, /*ICorDebugEditAndContinueSnapshot*/void** pSnapshots, /*ICorDebugErrorInfoEnum*/void** pError);

        /// <summary>
        /// GetProcess returns the process containing the app domain
        /// </summary>
        /// <param name="ppProcess"></param>
        /// <example><code>HRESULT GetProcess([out] ICorDebugProcess **ppProcess);</code></example>
        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] System.Int32 GetProcess ([MarshalAs (UnmanagedType.Interface)] out ICorDebugProcess ppProcess);

        /// <summary>
        /// EnumerateAssemblies enumerates all assemblies in the app domain
        /// </summary>
        /// <param name="ppAssemblies"></param>
        /// <example><code>HRESULT EnumerateAssemblies([out] ICorDebugAssemblyEnum **ppAssemblies);</code></example>
        [MethodImpl(MethodImplOptions.InternalCall | MethodImplOptions.PreserveSig, MethodCodeType = MethodCodeType.Runtime)]
        [PreserveSig]
        [MustUseReturnValue]
        new Int32 EnumerateAssemblies ([MarshalAs (UnmanagedType.Interface)] out ICorDebugAssemblyEnum ppAssemblies);

        /// <summary>
        /// GetModuleFromMetaDataInterface returns the ICorDebugModule with
        /// the given metadata interface.
        /// </summary>
        /// <param name="pIMetaData"></param>
        /// <param name="ppModule"></param>
        /// <example>
        ///     <code>
        ///         HRESULT GetModuleFromMetaDataInterface([in] IUnknown *pIMetaData, [out] ICorDebugModule **ppModule);
        ///     </code>
        /// </example>
        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] System.Int32 GetModuleFromMetaDataInterface (
            [MarshalAs (UnmanagedType.IUnknown), In] object pIMetaData,
            [MarshalAs (UnmanagedType.Interface)] out ICorDebugModule ppModule);

        /// <summary>
        /// EnumerateBreakpoints returns an enum (ICorDebugBreakpointEnum) of all active
        /// breakpoints in the app domain.  This includes all types of breakpoints :
        /// function breakpoints, data breakpoints, etc.
        /// </summary>
        /// <param name="ppBreakpoints"></param>
        /// <example><code>HRESULT EnumerateBreakpoints([out] ICorDebugBreakpointEnum **ppBreakpoints);</code></example>
        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] System.Int32 EnumerateBreakpoints ([MarshalAs (UnmanagedType.Interface)] out ICorDebugBreakpointEnum ppBreakpoints);

        /// <summary>
        /// EnumerateSteppers returns an enum of all active steppers in the app domain.
        /// </summary>
        /// <param name="ppSteppers"></param>
        /// <example><code>HRESULT EnumerateSteppers([out] ICorDebugStepperEnum **ppSteppers);</code></example>
        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] System.Int32 EnumerateSteppers ([MarshalAs (UnmanagedType.Interface)] out ICorDebugStepperEnum ppSteppers);

        /// <summary>
        /// DEPRECATED.  Always returns TRUE in V3 (attaching is process-wide).
        /// </summary>
        /// <param name="pbAttached"></param>
        /// <example><code>HRESULT IsAttached([out] BOOL *pbAttached);</code></example>
        [Obsolete("Always returns TRUE in V3 (attaching is process-wide).")]
        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] System.Int32 IsAttached (Int32 *pbAttached);

        /// <summary>
        /// GetName returns the name of the app domain.
        ///
        /// Usage pattern:
        /// *pcchName is always set to the length of pInputString (including NULL) in characters. This lets
        ///   callers know the full size of buffer they'd need to allocate to get the full string.
        ///
        /// if (cchName == 0) then we're in "query" mode:
        ///     This fails if szName is non-null or pcchName is null
        ///     Else this function will set pcchName to let the caller know how large of a buffer to allocate
        ///     and return S_OK.
        ///
        /// if (cchName != 0) then
        ///     This fails if szName is null.
        ///     Else this copies as much as can fit into szName (it will always null terminate szName) and returns S_OK.
        ///     pcchName can be null. If it's non-null, we set it.
        ///
        /// The expected usage pattern is that a client will call once to get the size of a buffer needed for the name,
        ///  allocate the buffer, and then call a 2nd time to fill in the buffer.
        ///
        /// The rest of the GetName() functions have the same semantics for the parameters unless otherwise noted.
        /// </summary>
        /// <param name="cchName"></param>
        /// <param name="pcchName"></param>
        /// <param name="szName"></param>
        /// <example>
        ///     <code>
        /// HRESULT GetName([in] ULONG32 cchName,
        ///                [out] ULONG32 *pcchName,
        ///                [out, size_is(cchName), length_is(*pcchName)] WCHAR szName[]);
        ///     </code>
        /// </example>
        [MustUseReturnValue("HResult")]
        [PreserveSig]
        [MethodImpl(MethodImplOptions.InternalCall | MethodImplOptions.PreserveSig, MethodCodeType = MethodCodeType.Runtime)]
        Int32 GetName ([In] UInt32 cchName, UInt32 *pcchName, [Out] UInt16* szName);

        /// <summary>
        /// GetObject returns a reference to the System.AppDomain object which represents this AppDomain
        /// from within the runtime.
        ///
        /// Note: this object is lazily initialized within the runtime and may return NULL if the object
        ///       does not yet exist. This case will return S_FALSE and is not considered a failure.
        /// </summary>
        /// <param name="ppObject"></param>
        /// <example><code>HRESULT GetObject([out] ICorDebugValue **ppObject);</code></example>
        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] System.Int32 GetObject ([MarshalAs (UnmanagedType.Interface)] out ICorDebugValue ppObject);

        /// <summary>
        /// DEPRECATED.  This does nothing in V3.  Attaching is process-wide.
        /// </summary>
        /// <example><code>HRESULT Attach();</code></example>
        [Obsolete]
        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] System.Int32 Attach ();

        /// <summary>
        /// Get the ID of this app domain. The ID will be unique within the
        /// containing process.
        /// </summary>
        /// <param name="pId"></param>
        /// <example><code>HRESULT GetID([out] ULONG32 *pId);</code></example>
        [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        [System.Runtime.InteropServices.PreserveSigAttribute] [JetBrains.Annotations.MustUseReturnValueAttribute] System.Int32 GetID (UInt32 *pId);
    }
}