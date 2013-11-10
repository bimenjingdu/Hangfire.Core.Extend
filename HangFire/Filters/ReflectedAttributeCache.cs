﻿// This file is part of HangFire.
// Copyright © 2013 Sergey Odinokov.
// 
// HangFire is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// HangFire is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with HangFire.  If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Reflection;

namespace HangFire.Filters
{
    internal static class ReflectedAttributeCache
    {
        private static readonly ConcurrentDictionary<Type, ReadOnlyCollection<JobFilterAttribute>> TypeFilterAttributeCache 
            = new ConcurrentDictionary<Type, ReadOnlyCollection<JobFilterAttribute>>();

        public static ICollection<JobFilterAttribute> GetTypeFilterAttributes(Type type)
        {
            return GetAttributes(TypeFilterAttributeCache, type);
        }

        private static ReadOnlyCollection<TAttribute> GetAttributes<TMemberInfo, TAttribute>(
            ConcurrentDictionary<TMemberInfo, ReadOnlyCollection<TAttribute>> lookup, 
            TMemberInfo memberInfo)
            where TAttribute : Attribute
            where TMemberInfo : MemberInfo
        {
            Debug.Assert(memberInfo != null);
            Debug.Assert(lookup != null);

            return lookup.GetOrAdd(memberInfo, mi => new ReadOnlyCollection<TAttribute>((TAttribute[])memberInfo.GetCustomAttributes(typeof(TAttribute), inherit: true)));
        }
    }
}
