#region License
// /****************************** Module Header ******************************\
// Module Name:  Commodity.cs
// Project:    Pis-Service
// Copyright (c) Microsoft Corporation.
// 
// <Description of the file>
// This source is subject to the Microsoft Public License.
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL.
// All other rights reserved.
// 
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
// EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED 
// WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
// \***************************************************************************/
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using App.Data;
using App.Data.Caching;
using App.Domain;

namespace App.Service.Master
{
    public class Commodity
    {
        private const string cacheName = "App.master.Commodity";

        private static readonly ICacheManager _cacheManager = new MemoryCacheManager();

        public static List<Data.Domain.Commodity> GetList()
        {
            string key = string.Format(cacheName);

            List<Data.Domain.Commodity> list = _cacheManager.Get(key, () =>
            {
                using (var db = new RepositoryFactory(new EfDbContext()))
                {
                    IQueryable<Data.Domain.Commodity> tb =
                        db.CreateRepository<Data.Domain.Commodity>().Table.Select(e => e);
                    return tb.ToList();
                }
            });

            return list;
        }

        public static List<Data.Domain.Commodity> GetList(MasterSearchForm crit)
        {
            string name = crit.searchName != null ? crit.searchName.Trim().ToLower() : "";

            IOrderedEnumerable<Data.Domain.Commodity> list = from c in GetList()
                where (name == "" || (c.Name).Trim().ToLower().Contains(name))
                orderby c.Name
                select c;
            return list.ToList();
        }

        public static Data.Domain.Commodity GetId(string id)
        {
            Data.Domain.Commodity item = GetList().Where(w => w.CommodityNo == id).FirstOrDefault();
            return item;
        }

        public static int Update(Data.Domain.Commodity itm, string dml)
        {
            if (dml == "I")
            {
                itm.EntryBy = SiteConfiguration.UserName;
                itm.EntryDate = DateTime.Now;
            }

            itm.ModifiedBy = SiteConfiguration.UserName;
            itm.ModifiedDate = DateTime.Now;

            _cacheManager.Remove(cacheName);

            using (var db = new RepositoryFactory(new EfDbContext()))
            {
                return db.CreateRepository<Data.Domain.Commodity>().CRUD(dml, itm);
            }
        }
    }
}