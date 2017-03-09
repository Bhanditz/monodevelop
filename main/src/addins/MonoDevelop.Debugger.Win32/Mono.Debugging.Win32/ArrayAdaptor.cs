// ArrayAdaptor.cs
//
// Author:
//   Lluis Sanchez Gual <lluis@novell.com>
//
// Copyright (c) 2008 Novell, Inc (http://www.novell.com)
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
//
//

using System;
using System.Collections;

using CorApi.ComInterop;

using JetBrains.Annotations;

using Mono.Debugging.Evaluation;

namespace Mono.Debugging.Win32
{
	unsafe class ArrayAdaptor: ICollectionAdaptor
	{
		readonly CorEvaluationContext ctx;
		readonly CorValRef<ICorDebugArrayValue> valRef;

		public ArrayAdaptor (EvaluationContext ctx, CorValRef<ICorDebugArrayValue> valRef)
		{
			this.ctx = (CorEvaluationContext) ctx;
			this.valRef = valRef;
		}

		public int[] GetLowerBounds()
		{
			ICorDebugArrayValue array = valRef.Val;
			if(array == null)
				return new int[] { };

			uint nRank = 0;
			array.GetRank(&nRank).AssertSucceeded("array.GetRank(&nRank)");

			int isHasBaseIndicies;
			array.HasBaseIndicies(&isHasBaseIndicies).AssertSucceeded("array.HasBaseIndicies(&isHasBaseIndicies)");
			if(isHasBaseIndicies == 0)
				return new int[nRank];

			var baseindices = new uint[nRank];
			fixed(uint* pind = baseindices)
				array.GetBaseIndicies(((uint)baseindices.Length), pind).AssertSucceeded("array.GetBaseIndicies (((uint)baseindices.Length), pind)");
			var baseindicesAsInts = new int[nRank];
			for(uint a = nRank; a-- > 0;)
				baseindicesAsInts[a] = ((int)baseindices[a]);
			return baseindicesAsInts;
		}

		public int[] GetDimensions()
		{
			ICorDebugArrayValue array = valRef.Val;
			if(array == null)
				return new int[] { };

			uint nRank = 0;
			array.GetRank(&nRank).AssertSucceeded("array.GetRank(&nRank)");

			var dims = new uint[nRank];
			fixed(uint* pdims = dims)
				array.GetDimensions(((uint)dims.Length), pdims).AssertSucceeded("array.GetDimensions(((uint)dims.Length), pdims)");
			var dimsAsInts = new int[nRank];
			for(uint a = nRank; a-- > 0;)
				dimsAsInts[a] = ((int)dims[a]);
			return dimsAsInts;
		}

		public object GetElement([NotNull] int[] indicesAsInts)
		{
			if(indicesAsInts == null)
				throw new ArgumentNullException(nameof(indicesAsInts));
			return new CorValRef(() =>
			{
				ICorDebugArrayValue array = valRef.Val;
				if(array == null)
					return null;

				if(indicesAsInts.Length == 0)
					throw new ArgumentOutOfRangeException(nameof(indicesAsInts), indicesAsInts, "The indices array must not be empty.");

				uint nRank = 0;
				array.GetRank(&nRank).AssertSucceeded("array.GetRank(&nRank)");
				if(indicesAsInts.Length != nRank)
					throw new ArgumentOutOfRangeException(nameof(indicesAsInts), indicesAsInts, $"The number of indices supplied, {indicesAsInts.Length:N0}, does not match the array rank of {nRank:N0}.");

				var indices = new uint[nRank];
				for(uint a = nRank; a-- > 0;)
					indices[a] = ((uint)indicesAsInts[a]);
				fixed(uint* pindices = indices)
				{
					ICorDebugValue value;
					array.GetElement(nRank, pindices, out value).AssertSucceeded("array.GetElement (nRank, pindices, out value)");
					return value;
				}
			});
		}

		public Array GetElements (int[] indices, int count)
		{
			// TODO: FIXME: the point of this method is to be more efficient than getting 1 array element at a time...
			var elements = new ArrayList ();

			int[] idx = new int[indices.Length];
			for (int i = 0; i < indices.Length; i++)
				idx[i] = indices[i];

			for (int i = 0; i < count; i++) {
				elements.Add (GetElement ((int[])idx.Clone ()));
				idx[idx.Length - 1]++;
			}

			return elements.ToArray ();
		}
		
		public void SetElement (int[] indices, object val)
		{
			CorValRef it = (CorValRef) GetElement (indices);
			valRef.Invalidate ();
			it.SetValue (ctx, (CorValRef) val);
		}

		public object ElementType
		{
			get
			{
				ICorDebugType exacttype;
				Com.QueryInteface<ICorDebugValue2>(valRef.Val).GetExactType(out exacttype).AssertSucceeded("Com.QueryInteface<ICorDebugValue2>(valRef.Val).GetExactType(out exacttype)");
				ICorDebugType firsttypeparam;
				exacttype.GetFirstTypeParameter(out firsttypeparam).AssertSucceeded("exacttype.GetFirstTypeParameter(out firsttypeparam)");
				return firsttypeparam;
			}
		}
	}
}
