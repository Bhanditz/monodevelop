namespace CorApi.ComInterop
{
  public static class ValueTypeEx
  {
    #region Operations

    public static bool ToBool(this int value)
    {
      return value != 0;
    }

    public static bool ToBool(this uint value)
    {
      return value != 0;
    }

    public static bool ToBool(this byte value)
    {
      return value != 0;
    }

    public static int ToInt(this bool value)
    {
      return value ? 1 : 0;
    }

    #endregion
  }
}