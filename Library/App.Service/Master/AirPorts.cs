#region License
// /****************************** Module Header ******************************\
// Module Name:  AirPorts.cs
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
using App.Data.Domain;
using App.Domain;

namespace App.Service.Master
{
    public class AirPorts
    {
        private const string cacheName = "App.master.AirPort";

        private static readonly ICacheManager _cacheManager = new MemoryCacheManager();

        public static List<AirPort> GetList()
        {
            string key = string.Format(cacheName);

            List<AirPort> list = _cacheManager.Get(key, () =>
            {
                using (var db = new RepositoryFactory(new EfDbContext()))
                {
                    IQueryable<AirPort> tb = db.CreateRepository<AirPort>().TableNoTracking.Select(e => e);
                    return tb.ToList();
                }
            });

            return list;
        }

        public static List<AirPort> GetList(MasterSearchForm crit)
        {
            string name = crit.searchName != null ? crit.searchName.Trim().ToLower() : "";

            IOrderedEnumerable<AirPort> list = from c in GetList()
                where (name == "" || (c.PortName).Trim().ToLower().Contains(name))
                orderby c.Description
                select c;
            return list.ToList();
        }

        public static AirPort GetId(int id)
        {
            AirPort item = GetList().Where(w => w.AirPortID == id).FirstOrDefault();
            return item;
        }

        public static int Update(AirPort itm, string dml)
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
                return db.CreateRepository<AirPort>().CRUD(dml, itm);
            }
        }
    }
}