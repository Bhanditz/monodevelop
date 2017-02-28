namespace CorApi.ComInterop
{
  /// <summary>
  /// The STGTY enumeration values are used in the type member of the STATSTG structure to indicate the type of the storage element. A storage element is a storage object, a stream object, or a byte-array object (LOCKBYTES).
  /// </summary>
  /// <example><code>
  ///     /* Storage element types */
  ///    typedef enum tagSTGTY
  ///    {
  ///        STGTY_STORAGE   = 1,
  ///        STGTY_STREAM    = 2,
  ///        STGTY_LOCKBYTES = 3,
  ///        STGTY_PROPERTY  = 4
  ///    } STGTY;
  /// </code></example>
  public enum STGTY : uint
  {
    /// <summary>
    /// Indicates that the storage element is a storage object.
    /// </summary>
    STGTY_STORAGE = 1,

    /// <summary>
    /// Indicates that the storage element is a stream object.
    /// </summary>
    STGTY_STREAM = 2,

    /// <summary>
    /// Indicates that the storage element is a byte-array object.
    /// </summary>
    STGTY_LOCKBYTES = 3,

    /// <summary>
    /// Indicates that the storage element is a property storage object.
    /// </summary>
    STGTY_PROPERTY = 4
  }
}