using System;
using System.Collections.Generic;
using System.Linq;

using JetBrains.Annotations;

namespace CorApi.ComInterop
{
    public static unsafe class ICorDebugFrameEx
    {
        public static CorFrameType GetFrameType([NotNull] this ICorDebugFrame frame)
        {
            if(frame == null)
                throw new ArgumentNullException(nameof(frame));
            var ilframe = frame as ICorDebugILFrame;
            if(ilframe != null)
                return CorFrameType.ILFrame;

            var iframe = frame as ICorDebugInternalFrame;
            if(iframe != null)
                return CorFrameType.InternalFrame;

            return CorFrameType.NativeFrame;
        }

        public static ICorDebugValue GetLocalVariable([NotNull] this ICorDebugFrame frame, uint index)
        {
            if(frame == null)
                throw new ArgumentNullException(nameof(frame));

            var ilframe = frame as ICorDebugILFrame;
            if(ilframe == null)
                return null;

            ICorDebugValue value;
            int hr = ilframe.GetLocalVariable(index, out value);
            if(hr == (int)HResults2.CORDBG_E_IL_VAR_NOT_AVAILABLE)
                return null; // If you are stopped in the Prolog, the variable may not be available. CORDBG_E_IL_VAR_NOT_AVAILABLE is returned after dubugee triggers StackOverflowException
            hr.AssertSucceeded($"Could not get local variable by index {index}.");
            return value;
        }

        /// <summary>
        ///           'TypeParameters' returns an enumerator that goes yields generic args from
        ///         both the class and the method. To enumerate just the generic args on the
        ///         method, we need to skip past the class args. We have to get that skip value
        ///         from the metadata. This is a helper function to efficiently get an enumerator that skips
        ///         to a given spot (likely past the class generic args).
        /// </summary>
        /// <param name="frame"></param>
        /// <param name="skip"></param>
        /// <returns></returns>
        public static IList<ICorDebugType> GetTypeParamEnumWithSkip([NotNull] this ICorDebugFrame frame, int skip)
        {
            if(skip < 0)
                throw new ArgumentException("Skip parameter must be positive");

            var il2 = frame as ICorDebugILFrame2;
            if(il2 == null)
                return new ICorDebugType[] { };

            ICorDebugTypeEnum @enum;
            il2.EnumerateTypeParameters(out @enum).AssertSucceeded("Could not enumerate the IL frame type parameters.");
            return @enum.ToList<ICorDebugTypeEnum, ICorDebugType>((corenum, celt, values, fetched) => corenum.Next(celt, values, fetched)).Skip(skip).ToList();
        }

        public enum CorFrameType
        {
            ILFrame,

            NativeFrame,

            InternalFrame
        }
    }
}