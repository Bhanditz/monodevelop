using System.Collections.Generic;
using System.Text;
using System.Threading;

using CorApi.Pinvoke;

using CorApi2.debug;

using Microsoft.Win32.SafeHandles;

namespace CorApi2.Extensions
{
	public static unsafe class CorProcessExtensions
	{
		internal static void TrackStdOutput (this CorProcess proc, SafeFileHandle outputPipe, SafeFileHandle errorPipe)
		{
			var outputReader = new Thread (delegate () {
				proc.ReadOutput (outputPipe, false);
			});
			outputReader.Name = "Debugger output reader";
			outputReader.IsBackground = true;
			outputReader.Start ();

			var errorReader = new Thread (delegate () {
				proc.ReadOutput (errorPipe, true);
			});
			errorReader.Name = "Debugger error reader";
			errorReader.IsBackground = true;
			errorReader.Start ();
		}

		// [Xamarin] Output redirection.
		private static void ReadOutput(this CorProcess proc, SafeFileHandle pipe, bool isStdError)
		{
			var buffer = new byte[256];
			uint nBytesRead;

			try
			{
				while(true)
				{
					fixed(byte* pBuffer = buffer)
					{
						if((Kernel32Dll.ReadFile(((void*)pipe.DangerousGetHandle()), pBuffer, (uint)buffer.Length, &nBytesRead, null) == 0) || (nBytesRead == 0))
							break; // pipe done - normal exit path.
					}

					string s = Encoding.Default.GetString(buffer, 0, (int)nBytesRead);
					List<CorTargetOutputEventHandler> list;
					if(events.TryGetValue(proc, out list))
					{
						foreach(CorTargetOutputEventHandler del in list)
							del(proc, new CorTargetOutputEventArgs(s, isStdError));
					}
				}
			}
			catch
			{
			}
		}

		public static void RegisterStdOutput (this CorProcess proc, CorTargetOutputEventHandler handler)
		{
			proc.OnProcessExit += delegate {
				RemoveEventsFor (proc);
			};

			List<CorTargetOutputEventHandler> list;
			if (!events.TryGetValue (proc, out list))
				list = new List<CorTargetOutputEventHandler> ();
			list.Add (handler);

			events [proc] = list;
		}

		static void RemoveEventsFor (CorProcess proc)
		{
			events.Remove (proc);
		}

		// [Xamarin] Output redirection.
		static readonly Dictionary<CorProcess, List<CorTargetOutputEventHandler>> events = new Dictionary<CorProcess, List<CorTargetOutputEventHandler>> ();
	}
}