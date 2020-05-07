using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SpaceCtrl.Front.Models.Common;

namespace SpaceCtrl.Front.Extensions
{
    public static class QueryExt
    {
        public static IEnumerable<TData> ToPagedList<TData>(this IOrderedEnumerable<TData> orderQuery, Pagination filter)
        {
            if (filter.Size == 0) throw new InvalidOperationException($"{nameof(filter.Size)} value can't be zero");
            var skip = filter.Size * filter.Index;
            return orderQuery.Skip((int)skip).Take((int)filter.Size);
        }

        public static IQueryable<TData> ToPagedList<TData>(this IOrderedQueryable<TData> orderQuery, Pagination filter)
        {
            if (filter.Size == 0) throw new InvalidOperationException($"{nameof(filter.Size)} value can't be zero");
            var skip = filter.Size * filter.Index;
            return orderQuery.Skip((int)skip).Take((int)filter.Size);
        }

        public static Task<List<Dropdown>> ToDropDownListAsync<T>(this DbSet<T> dbSet) where T : class, IDropDown
        {
            return dbSet.Select(x => new Dropdown(x.Id, x.Name)).ToListAsync();
        }
    }
}