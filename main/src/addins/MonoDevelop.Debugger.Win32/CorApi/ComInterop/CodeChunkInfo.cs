using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace CorApi.ComInterop
{
/// <summary>
/// 
/// </summary>
/// <example><code>
/// from: &lt;cordebug.idl&gt;
///        typedef struct _CodeChunkInfo
///        {
///            CORDB_ADDRESS startAddr;
///            ULONG32 length;
///        } CodeChunkInfo;
/// </code></example>
    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    [SuppressMessage ("ReSharper", "BuiltInTypeReferenceStyle")]
public struct CodeChunkInfo
    {
      [ComAliasName("CORDB_ADDRESS")]
        public UInt64 startAddr;
        public UInt32 length;
    }
}