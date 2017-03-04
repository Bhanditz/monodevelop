using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using JetBrains.Annotations;

namespace CorApi.ComInterop
{
  /// <summary>
  ///  ICorDebugILFrame2 is a logical extension to ICorDebugILFrame.
  /// </summary>
  /// <example><code>
  ///  /*
  ///  * ICorDebugILFrame2 is a logical extension to ICorDebugILFrame.
  ///  */
  /// [
  ///     object,
  ///     local,
  ///     uuid(5D88A994-6C30-479b-890F-BCEF88B129A5),
  ///     pointer_default(unique)
  /// ]
  /// interface ICorDebugILFrame2 : IUnknown
  /// {
  ///     /*
  ///      * Performs an on-stack replacement for an outstanding function remap opportunity.
  ///      * This is used to update execution of an edited function to the latest version, 
  ///      * preserving the current frame state (such as the values of all locals). 
  ///      * This can only be called when a FunctionRemapOpportunity callback has been delivered
  ///      * for this leaf frame, and the callback has not yet been continued.  newILOffset
  ///      * is the offset into the new function at which execution should continue.
  ///      * When the remap has completed, a FunctionRemapComplete callback will be delivered.
  ///      */
  ///     HRESULT RemapFunction([in] ULONG32 newILOffset);
  /// 
  ///     /*
  ///      * EnumerateTypeParameters returns the type parameters active on a frame.
  ///      * This will include both the class type parameters (if any) followed by the method type
  ///      * parameters (if any).
  ///      * Use the metadata API IMetaDataImport2::EnumGenericParams to determine how many
  ///      * Class type parameters vs. Method Type parameters there are in this list.
  ///      * The type parameters will not always be available.
  ///      */
  /// 
  ///     HRESULT EnumerateTypeParameters([out] ICorDebugTypeEnum **ppTyParEnum);
  /// 
  /// }; 
  ///  </code></example>
  [Guid ("5D88A994-6C30-479B-890F-BCEF88B129A5")]
  [InterfaceType (ComInterfaceType.InterfaceIsIUnknown)]
  [ComImport]
  [SuppressMessage ("ReSharper", "BuiltInTypeReferenceStyle")]
  public interface ICorDebugILFrame2
  {
    /// <summary>
    /// Performs an on-stack replacement for an outstanding function remap opportunity.
    /// This is used to update execution of an edited function to the latest version,
    /// preserving the current frame state (such as the values of all locals).
    /// This can only be called when a FunctionRemapOpportunity callback has been delivered
    /// for this leaf frame, and the callback has not yet been continued.  newILOffset
    /// is the offset into the new function at which execution should continue.
    /// When the remap has completed, a FunctionRemapComplete callback will be delivered.
    /// </summary>
    /// <param name="newILOffset"></param>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void RemapFunction ([In] UInt32 newILOffset);

    /// <summary>
    /// EnumerateTypeParameters returns the type parameters active on a frame.
    /// This will include both the class type parameters (if any) followed by the method type
    /// parameters (if any).
    /// Use the metadata API IMetaDataImport2::EnumGenericParams to determine how many
    /// Class type parameters vs. Method Type parameters there are in this list.
    /// The type parameters will not always be available.
    /// </summary>
    /// <param name="ppTyParEnum"></param>
    [MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    [PreserveSig][MustUseReturnValue]Int32 EnumerateTypeParameters ([MarshalAs (UnmanagedType.Interface)] out ICorDebugTypeEnum ppTyParEnum);
  }
}