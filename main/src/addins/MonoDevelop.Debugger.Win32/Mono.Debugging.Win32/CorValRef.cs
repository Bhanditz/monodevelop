using System.Runtime.InteropServices;

using CorApi.ComInterop;

using CorApi2.debug;

using Mono.Debugging.Client;

namespace Mono.Debugging.Win32
{
	public class CorValRef : CorValRef<ICorDebugValue>
	{
		public CorValRef (ICorDebugValue val) : base (val)
		{
		}

		public CorValRef (ICorDebugValue val, ValueLoader loader) : base (val, loader)
		{
		}

		public CorValRef (ValueLoader loader) : base (loader)
		{
		}
	}
}
