using System;

namespace CorApi.Pinvoke
{
	public static class DUPLICATE
	{
		/// <summary>
		/// Closes the source handle. This occurs regardless of any error status returned.
		/// </summary>
		public static readonly UInt32 DUPLICATE_CLOSE_SOURCE = 0x00000001;

		/// <summary>
		/// Ignores the dwDesiredAccess parameter. The duplicate handle has the same access as the source.
		/// </summary>
		public static readonly UInt32 DUPLICATE_SAME_ACCESS = 0x00000002;
	}
}