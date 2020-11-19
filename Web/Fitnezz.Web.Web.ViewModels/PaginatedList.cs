﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fitnezz.Web.Web.ViewModels.Workouts;
using Microsoft.EntityFrameworkCore;

namespace Fitnezz.Web.Web.ViewModels
{
    public class PaginatedList<T> : List<T>
    {
        public int PageIndex { get; set; }

        public int TotalPages { get; set; }

        public PaginatedList(List<T> items, int count, int pageIndex, int pageSize)
        {
            PageIndex = pageIndex;
            TotalPages = (int) Math.Ceiling(count / (double) pageSize);
            this.AddRange(items);
        }

        public PaginatedList()
        {
        }

        public bool PreviousPage
        {
            get { return (PageIndex > 1); }
        }

        public bool NextPage
        {
            get
            {
                return (PageIndex < TotalPages);
            }
        }

        public async Task<PaginatedList<T>> CreateAsync(IQueryable<T> source, int pageIndex, int pageSize)
        {
            var count = await source.CountAsync();
            var items = await source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
            return new PaginatedList<T>(items,count,pageIndex,pageSize);
        }

    }
}