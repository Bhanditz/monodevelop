using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading;

using CorApi.ComInterop;
using CorApi.Pinvoke;

using CorApi2.debug;

using Microsoft.Win32.SafeHandles;

namespace CorApi2.Extensions
{
	public unsafe class CorDebugProcessOutputRedirection
	{
		private readonly ICorDebugProcess _process;

		/// <inheritdoc />
		public CorDebugProcessOutputRedirection(ICorDebugProcess process)
		{
			_process = process;
		}

		public void TearDownOutputRedirection (SafeFileHandle outReadPipe, SafeFileHandle errorReadPipe, Action closehandles)
		{
			if (outReadPipe != null) {
				// Close pipe handles (do not continue to modify the parent).
				// You need to make sure that no handles to the write end of the
				// output pipe are maintained in this process or else the pipe will
				// not close when the child process exits and the ReadFile will hang.
				closehandles();

				TrackStdOutput (outReadPipe, errorReadPipe);
			}
		}

		void TrackStdOutput (SafeFileHandle outputPipe, SafeFileHandle errorPipe)
		{
			var outputReader = new Thread (delegate () {
				ReadOutput (outputPipe, false);
			});
			outputReader.Name = "Debugger output reader";
			outputReader.IsBackground = true;
			outputReader.Start ();

			var errorReader = new Thread (delegate () {
				ReadOutput (errorPipe, true);
			});
			errorReader.Name = "Debugger error reader";
			errorReader.IsBackground = true;
			errorReader.Start ();
		}

		[HandleProcessCorruptedStateExceptions]
		private void ReadOutput(SafeFileHandle pipe, bool isStdError)
		{
			var buffer = new byte[256];

			try
			{
				while(true)
				{
					uint nBytesRead;
					fixed(byte* pBuffer = buffer)
					{
						if((Kernel32Dll.ReadFile(((void*)pipe.DangerousGetHandle()), pBuffer, (uint)buffer.Length, &nBytesRead, null) == 0) || (nBytesRead == 0))
							break; // pipe done - normal exit path.
					}

					string s = Encoding.Default.GetString(buffer, 0, (int)nBytesRead);
					List<CorTargetOutputEventHandler> list = _events;
					foreach(CorTargetOutputEventHandler del in list)
						del(_process, new CorTargetOutputEventArgs(s, isStdError));
				}
			}
			catch
			{
				// ignored
			}
		}

		public void RegisterStdOutput (CorTargetOutputEventHandler handler)
		{
			_events.Add (handler);
		}

		// [Xamarin] Output redirection.
		readonly List<CorTargetOutputEventHandler> _events = new List<CorTargetOutputEventHandler>();

		public const int CREATE_REDIRECT_STD = 0x40000000;

		public static void CreateHandles (STARTUPINFOW si, out SafeFileHandle outReadPipeSafeHandle, out SafeFileHandle errorReadPipeSafeHandle, out Action closehandles)
		{
			closehandles = () => { };

			si.dwFlags |= 0x00000100; /*			STARTF_USESTDHANDLES*/
			var sa = new SECURITY_ATTRIBUTES ();
			sa.bInheritHandle = 1;
			void* curProc = Kernel32Dll.GetCurrentProcess();

			void* outWritePipe, outReadPipeTmp;
			if (Kernel32Dll.CreatePipe (&outReadPipeTmp, &outWritePipe, &sa, 0)==0)
				throw new Exception ("Pipe creation failed", new Win32Exception());
			SafeFileHandle outWritePipeSafeHandle = new SafeFileHandle((IntPtr)outWritePipe, true);
			SafeFileHandle outReadPipeTmpSafeHandle = new SafeFileHandle((IntPtr)outReadPipeTmp, true);

			// Create the child error pipe.
			void* errorWritePipe, errorReadPipeTmp;
			if (Kernel32Dll.CreatePipe (&errorReadPipeTmp, &errorWritePipe, &sa, 0)==0)
				throw new Exception ("Pipe creation failed");
			SafeFileHandle errorWritePipeSafeHandle = new SafeFileHandle(((IntPtr)errorWritePipe), true);
			SafeFileHandle errorReadPipeTmpSafeHandle = new SafeFileHandle(((IntPtr)errorReadPipeTmp), true);

			// Create new output read and error read handles. Set
			// the Properties to FALSE. Otherwise, the child inherits the
			// properties and, as a result, non-closeable handles to the pipes
			// are created.
			void* outReadPipe;
			if (Kernel32Dll.DuplicateHandle (curProc, outReadPipeTmp, curProc, &outReadPipe, 0, 0, DUPLICATE.DUPLICATE_SAME_ACCESS)==0)
				throw new Exception ("Pipe creation failed");
			outReadPipeSafeHandle = new SafeFileHandle(((IntPtr)outReadPipe), true);
			void* errorReadPipe;
			if (Kernel32Dll.DuplicateHandle (curProc, errorReadPipeTmp, curProc, &errorReadPipe, 0, 0, DUPLICATE.DUPLICATE_SAME_ACCESS)==0)
				throw new Exception ("Pipe creation failed");
			errorReadPipeSafeHandle = new SafeFileHandle(((IntPtr)errorReadPipe), true);

			Kernel32Dll.CloseHandle (curProc);

			// Close inheritable copies of the handles you do not want to be
			// inherited.
			outReadPipeTmpSafeHandle.Close ();
			errorReadPipeTmpSafeHandle.Close ();

			si.hStdInput = Kernel32Dll.GetStdHandle ((uint)StdHandles.STD_INPUT_HANDLE);
			si.hStdOutput = outWritePipe;
			closehandles += outWritePipeSafeHandle.Close;
			si.hStdError = errorWritePipe;
			closehandles += errorWritePipeSafeHandle.Close;
		}

		public static void SetupOutputRedirection (STARTUPINFOW si, ref int flags, out SafeFileHandle outReadPipe, out SafeFileHandle errorReadPipe, out Action closehandles)
		{
			if ((flags & CREATE_REDIRECT_STD) != 0) {
				CreateHandles (si, out outReadPipe, out errorReadPipe, out closehandles);
				flags &= ~CREATE_REDIRECT_STD;
			}
			else {
				outReadPipe = null;
				errorReadPipe = null;
				si.hStdInput = null;
				si.hStdOutput = null;
				si.hStdError = null;
				closehandles = () => { };
			}
		}
	}
}