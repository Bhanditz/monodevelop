using System;
using System.Diagnostics.CodeAnalysis;

namespace CorApi.Pinvoke
{
  [SuppressMessage("ReSharper", "BuiltInTypeReferenceStyle")]
  public static class CLSCTX
  {
    public static readonly UInt32 CLSCTX_INPROC_SERVER = 0x1;

    public static readonly UInt32 CLSCTX_INPROC_HANDLER = 0x2;

    public static readonly UInt32 CLSCTX_LOCAL_SERVER = 0x4;

    public static readonly UInt32 CLSCTX_INPROC_SERVER16 = 0x8;

    public static readonly UInt32 CLSCTX_REMOTE_SERVER = 0x10;

    public static readonly UInt32 CLSCTX_INPROC_HANDLER16 = 0x20;

    public static readonly UInt32 CLSCTX_RESERVED1 = 0x40;

    public static readonly UInt32 CLSCTX_RESERVED2 = 0x80;

    public static readonly UInt32 CLSCTX_RESERVED3 = 0x100;

    public static readonly UInt32 CLSCTX_RESERVED4 = 0x200;

    public static readonly UInt32 CLSCTX_NO_CODE_DOWNLOAD = 0x400;

    public static readonly UInt32 CLSCTX_RESERVED5 = 0x800;

    public static readonly UInt32 CLSCTX_NO_CUSTOM_MARSHAL = 0x1000;

    public static readonly UInt32 CLSCTX_ENABLE_CODE_DOWNLOAD = 0x2000;

    public static readonly UInt32 CLSCTX_NO_FAILURE_LOG = 0x4000;

    public static readonly UInt32 CLSCTX_DISABLE_AAA = 0x8000;

    public static readonly UInt32 CLSCTX_ENABLE_AAA = 0x10000;

    public static readonly UInt32 CLSCTX_FROM_DEFAULT_CONTEXT = 0x20000;

    public static readonly UInt32 CLSCTX_ACTIVATE_32_BIT_SERVER = 0x40000;

    public static readonly UInt32 CLSCTX_ACTIVATE_64_BIT_SERVER = 0x80000;

    public static readonly UInt32 CLSCTX_ENABLE_CLOAKING = 0x100000;

    public static readonly UInt32 CLSCTX_APPCONTAINER = 0x400000;

    public static readonly UInt32 CLSCTX_ACTIVATE_AAA_AS_IU = 0x800000;

    public static readonly UInt32 CLSCTX_PS_DLL = 0x80000000;
  }
}