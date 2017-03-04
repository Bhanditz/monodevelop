//---------------------------------------------------------------------
//  This file is part of the CLR Managed Debugger (mdbg) Sample.
// 
//  Copyright (C) Microsoft Corporation.  All rights reserved.
//---------------------------------------------------------------------
using System;
using System.Diagnostics;
using CorApi.ComInterop;

namespace Microsoft.Samples.Debugging.CorDebug
{
    /** 
     * collects functionality which requires runnint code inside the debuggee.
     */
    public sealed  class CorEval : WrapperBase
    {
        private ICorDebugEval m_eval;

        internal CorEval (ICorDebugEval e)
            :base(e)
        {
            m_eval = e;
        }



        public void CallFunction (ICorDebugFunction managedFunction, ICorDebugValue[] arguments)
        {
            CorApi.ComInterop.ICorDebugValue[] values = null;
            if(arguments!=null)
            {
                values = new CorApi.ComInterop.ICorDebugValue[arguments.Length];
                for(int i=0;i<arguments.Length;i++)
                    values[i] = arguments[i].m_val;
            }
            m_eval.CallFunction(managedFunction.m_function,
                                (uint) (arguments==null?0:arguments.Length),
                                 values);
        }

        public void CallParameterizedFunction (ICorDebugFunction managedFunction, ICorDebugType[] argumentTypes, ICorDebugValue[] arguments)
        {
            CorApi.ComInterop.ICorDebugType[] types = null;
            int typesLength = 0;
            CorApi.ComInterop.ICorDebugValue[] values = null;
            int valuesLength = 0;
            
            ICorDebugEval2 eval2 = (ICorDebugEval2) m_eval;

            if (argumentTypes != null)
            {
                types = new CorApi.ComInterop.ICorDebugType[argumentTypes.Length];
                for (int i = 0; i < argumentTypes.Length; i++)
                    types[i] = argumentTypes[i].m_type;
                typesLength = types.Length;
            }
            if (arguments != null)
            {
                values = new CorApi.ComInterop.ICorDebugValue[arguments.Length];
                for (int i = 0; i < arguments.Length; i++)
                    values[i] = arguments[i].m_val;
                valuesLength = values.Length;
            }
            eval2.CallParameterizedFunction(managedFunction.m_function, (uint)typesLength, types, (uint)valuesLength, values);
        }

        public ICorDebugValue CreateValueForType(ICorDebugType type)
        {
            CorApi.ComInterop.ICorDebugValue val = null;
            ICorDebugEval2 eval2 = (ICorDebugEval2) m_eval;
            eval2.CreateValueForType(type.m_type, out val);
            return val==null?null:new ICorDebugValue (val);
        }

        public void NewParameterizedObject(ICorDebugFunction managedFunction, ICorDebugType[] argumentTypes, ICorDebugValue[] arguments)
        {
    
            CorApi.ComInterop.ICorDebugType[] types = null;
            int typesLength = 0;
            CorApi.ComInterop.ICorDebugValue[] values = null;
            int valuesLength = 0;
            ICorDebugEval2 eval2 = (ICorDebugEval2) m_eval;

            if (argumentTypes != null)
            {
                types = new CorApi.ComInterop.ICorDebugType[argumentTypes.Length];
                for (int i = 0; i < argumentTypes.Length; i++)
                    types[i] = argumentTypes[i].m_type;
                typesLength = types.Length;
            }
            if (arguments != null)
            {
                values = new CorApi.ComInterop.ICorDebugValue[arguments.Length];
                for (int i = 0; i < arguments.Length; i++)
                    values[i] = arguments[i].m_val;
                valuesLength = values.Length;
            }
            eval2.NewParameterizedObject(managedFunction.m_function, (uint)typesLength, types, (uint)valuesLength, values);
        }

        public void NewParameterizedObjectNoConstructor(ICorDebugClass managedClass, ICorDebugType[] argumentTypes)
        {
            CorApi.ComInterop.ICorDebugType[] types = null;
            int typesLength=0;
            ICorDebugEval2 eval2 = (ICorDebugEval2) m_eval;
            if (argumentTypes != null)
            {
                types = new CorApi.ComInterop.ICorDebugType[argumentTypes.Length];
                for (int i = 0; i < argumentTypes.Length; i++)
                    types[i] = argumentTypes[i].m_type;
                typesLength = types.Length;
            }
            eval2.NewParameterizedObjectNoConstructor(managedClass.m_class, (uint)typesLength, types);
        }

        public void NewParameterizedArray(ICorDebugType type, int rank, int dims, int lowBounds)
        {
            ICorDebugEval2 eval2 = (ICorDebugEval2) m_eval;
            uint udims = (uint)dims;
            uint ulowBounds = (uint)lowBounds;
            eval2.NewParameterizedArray(type.m_type, (uint)rank, ref udims, ref ulowBounds);
        }


        /** Create an object w/o invoking its constructor. */
        public void NewObjectNoContstructor (ICorDebugClass c)
        {
            m_eval.NewObjectNoConstructor (c.m_class);
        }

        /** allocate a string w/ the given contents. */
        public void NewString (string value)
        {
            m_eval.NewString (value);
        }

        public void NewArray (CorElementType type, ICorDebugClass managedClass, int rank, 
                              int dimensions, int lowBounds)
        {
            uint udims = (uint)dimensions;
            uint ulowBounds = (uint)lowBounds;
            m_eval.NewArray (type, managedClass.m_class, (uint)rank, ref udims, ref ulowBounds);
        }

        /** Does the Eval have an active computation? */
        public bool IsActive ()
        {
            int r = 0;
            m_eval.IsActive (out r);
            return !(r==0);
        }

        /** Abort the current computation. */
        public void Abort ()
        {
            m_eval.Abort ();
        }

        /** Rude abort the current computation. */
        public void RudeAbort ()
        {
            ICorDebugEval2 eval2 = (ICorDebugEval2) m_eval;
            eval2.RudeAbort ();
        }

        /** Result of the evaluation.  Valid only after the eval is complete. */
        public ICorDebugValue Result
        {
            get
            {
                CorApi.ComInterop.ICorDebugValue v = null;
                m_eval.GetResult (out v);
                return (v==null)?null:new ICorDebugValue (v);
            }
        }

        /** The thread that this eval was created in. */
        public ICorDebugThread Thread
        {
            get
            {
                CorApi.ComInterop.ICorDebugThread t = null;
                m_eval.GetThread (out t);
                return (t==null)?null:new ICorDebugThread (t);
            }
        }

        /** Create a Value to use it in a Function Evaluation. */
        public ICorDebugValue CreateValue (CorElementType type, ICorDebugClass managedClass)
        {
            CorApi.ComInterop.ICorDebugValue v = null;
            m_eval.CreateValue (type, managedClass==null?null:managedClass.m_class, out v);
            return (v==null)?null:new ICorDebugValue (v);
        }
    } /* class Eval */
} /* namespace */
