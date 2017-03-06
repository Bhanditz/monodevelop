//
// MetadataExtensions.cs
//
// Author:
//       Lluis Sanchez <lluis@xamarin.com>
//       Therzok <teromario@yahoo.com>
//
// Copyright (c) 2013 Xamarin Inc.
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

using System;
using System.Collections.Generic;
using System.Reflection;

using CorApi2.Metadata;

namespace CorApi2.Extensions
{
	// [Xamarin] Expression evaluator.
	public static class MetadataExtensions
	{
		internal static bool TypeFlagsMatch (bool isPublic, bool isStatic, BindingFlags flags)
		{
			if (isPublic && (flags & BindingFlags.Public) == 0)
				return false;
			if (!isPublic && (flags & BindingFlags.NonPublic) == 0)
				return false;
			if (isStatic && (flags & BindingFlags.Static) == 0)
				return false;
			if (!isStatic && (flags & BindingFlags.Instance) == 0)
				return false;
			return true;
		}

		internal static Type MakeDelegate (Type retType, List<Type> argTypes)
		{
			throw new NotImplementedException ();
		}

		public static Type MakeArray (Type t, List<int> sizes, List<int> loBounds)
		{
			var mt = t as MetadataType;
			if (mt != null) {
				if (sizes == null) {
					sizes = new List<int> ();
					sizes.Add (1);
				}
				mt.m_arraySizes = sizes;
				mt.m_arrayLoBounds = loBounds;
				return mt;
			}
			if (sizes == null || sizes.Count == 1)
				return t.MakeArrayType ();
			return t.MakeArrayType (sizes.Capacity);
		}

		static Type MakeByRefTypeIfNeeded (Type t)
		{
			if (t.IsByRef)
				return t;
			var makeByRefType = t.MakeByRefType ();
			return makeByRefType;
		}

		public static Type MakeByRef (Type t)
		{
			var mt = t as MetadataType;
			if (mt != null) {
				mt.m_isByRef = true;
				return mt;
			}

			return MakeByRefTypeIfNeeded (t);
		}

		public static Type MakePointer (Type t)
		{
			var mt = t as MetadataType;
			if (mt != null) {
				mt.m_isPtr = true;
				return mt;
			}
			return MakeByRefTypeIfNeeded (t);
		}

		public static Type MakeGeneric (Type t, List<Type> typeArgs)
		{
			var mt = (MetadataType)t;
			mt.m_typeArgs = typeArgs;
			return mt;
		}
	}

	// [Xamarin] Expression evaluator.
}

