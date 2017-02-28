using System;
using System.Runtime.InteropServices;

namespace CorApi.ComInterop
{
  /// <summary>
  /// The <see cref="SECURITY_ATTRIBUTES"/> structure contains the security descriptor for an object and specifies whether the handle retrieved by specifying this structure is inheritable. This structure provides security settings for objects created by various functions, such as CreateFile, CreatePipe, CreateProcess, RegCreateKeyEx, or RegSaveKeyEx.
  /// </summary>
  [StructLayout(LayoutKind.Sequential)]
  public unsafe struct SECURITY_ATTRIBUTES
  {
    /// <summary>
    /// The size, in bytes, of this structure. Set this value to the size of the SECURITY_ATTRIBUTES structure.
    /// </summary>
    public UInt32 nLength;

    /// <summary>
    /// A pointer to a security descriptor for the object that controls the sharing of it. If NULL is specified for this member, the object is assigned the default security descriptor of the calling process. This is not the same as granting access to everyone by assigning a NULL discretionary access control list (DACL). The default security descriptor is based on the default DACL of the access token belonging to the calling process. By default, the default DACL in the access token of a process allows access only to the user represented by the access token. If other users must access the object, you can either create a security descriptor with the appropriate access, or add ACEs to the DACL that grants access to a group of users.
    /// </summary>
    public void* lpSecurityDescriptor;

    /// <summary>
    /// A Boolean value that specifies whether the returned handle is inherited when a new process is created. If this member is TRUE, the new process inherits the handle.
    /// </summary>
    public Int32 bInheritHandle;
  }
}