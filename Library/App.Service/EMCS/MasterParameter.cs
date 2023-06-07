using App.Data.Caching;
using System;
using System.Collections.Generic;
using System.Linq;
using App.Domain;

namespace App.Service.EMCS
{

    /// <summary>
    /// Services Proses Shipment inbound.</summary>                
    public class MasterParameter
    {
        public const string CacheName = "App.EMCS.MasterParameter";

        public readonly static ICacheManager CacheManager = new MemoryCacheManager();

        /// <summary>
        /// Get List from Shipment inbound data
        /// </summary>
        /// <returns></returns>
        public static List<Data.Domain.EMCS.MasterParameter> GetParamByGroup(string groupName)
        {
            using (var db = new Data.EmcsContext())
            {
                var tb = db.MasterParameter.Where(a => a.Group == groupName).AsQueryable().OrderBy(a => a.Sort);
                return tb.ToList();
            }
        }

        public static dynamic GetSelectOption(MasterSearchForm crit)
        {
            string search = crit.searchName ?? "";
            string group = crit.searchCode ?? "";

            using (var db = new Data.EmcsContext())
            {
                var data = db.MasterParameter
                    .Where(i => i.Group.Equals(@group) && i.Name.Contains(search))
                    .OrderBy(i => i.Sort)
                    .Skip(0).Take(100)
                    .Select(i => new
                    {
                        id = i.Value,
                        text = i.Name
                    }).ToList();

                return data.Select(i => new Data.Domain.EMCS.SelectItem2 { Id = i.text, Text = i.text }).ToList();
            }
        }

        public static dynamic GetParamOptions(MasterSearchForm crit)
        {
            string search = crit.searchName ?? "";
            string group = crit.searchCode ?? "";

            using (var db = new Data.EmcsContext())
            {
                List<Data.Domain.EMCS.SelectItem2> data;
                if (@group.Equals("ContainerType"))
                {
                    data = db.MasterParameter
                        .Where(i => i.Group.Equals(@group) && i.Description.Contains(search))
                        .OrderBy(i => i.Sort)
                        .Skip(0).Take(100)
                        .Select(i => new Data.Domain.EMCS.SelectItem2
                        {
                            Id = i.Value,
                            Text = i.Description
                        }).ToList();
                }
                else
                {
                    data = db.MasterParameter
                        .Where(i => i.Group.Equals(@group) && i.Name.Contains(search))
                        .OrderBy(i => i.Sort)
                        .Skip(0).Take(100)
                        .Select(i => new Data.Domain.EMCS.SelectItem2
                        {
                            Id = i.Value,
                            Text = i.Name
                        }).ToList();
                }
                
                return data.ToList();
            }
        }

        public static List<Data.Domain.EMCS.SelectItem2> GetSelectList(string group)
        {
            using (var db = new Data.EmcsContext())
            {
                var data = db.MasterParameter
                    .Where(i => i.Group.Equals(@group))
                    .OrderBy(i => i.Sort)
                    .Skip(0).Take(100)
                    .Select(i => new
                    {
                        id = i.Value,
                        text = i.Name
                    }).ToList();

                return data.Select(i => new Data.Domain.EMCS.SelectItem2 { Id = i.text, Text = i.text }).ToList();
            }
        }

        public static List<Data.Domain.EMCS.SelectItem2> GetSelectList2(string group)
        {
            using (var db = new Data.EmcsContext())
            {
                var data = db.MasterParameter
                    .Where(i => i.Group.Equals(@group))
                    .OrderBy(i => i.Sort)
                    .Skip(0).Take(100)
                    .Select(i => new
                    {
                        id = i.Value,
                        text = i.Name,
                        sort = i.Sort
                    }).ToList().AsQueryable().OrderBy(a => Convert.ToInt32(a.sort)).ToList();

                return data.Select(i => new Data.Domain.EMCS.SelectItem2 { Id = i.text, Text = i.text }).ToList();
            }
        }

        public static List<Data.Domain.EMCS.SelectItem3> GetSelectListDesc(string group)
        {
            using (var db = new Data.EmcsContext())
            {
                var data = db.MasterParameter
                    .Where(i => i.Group.Equals(@group))
                    .OrderBy(i => i.Sort)
                    .Skip(0).Take(100)
                    .Select(i => new
                    {
                        id = i.Value,
                        text = i.Name,
                        desc = i.Description
                    }).ToList();

                return data.Select(i => new Data.Domain.EMCS.SelectItem3 { Id = i.text, Text = i.text, Extra = i.desc }).ToList();
            }
        }

        public static List<Data.Domain.EMCS.MasterParameter> GetParamByValue(string groupName, string value)
        {
            using (var db = new Data.EmcsContext())
            {
                var tb = db.MasterParameter.Where(a => a.Group == groupName && a.Value == value);
                return tb.ToList();
            }
        }

        public static List<Data.Domain.EMCS.MasterParameter> GetParamByName(string groupName, string name)
        {
            using (var db = new Data.EmcsContext())
            {
                var tb = db.MasterParameter.Where(a => a.Group == groupName && a.Name == name);
                return tb.ToList();
            }
        }

        public static Data.Domain.EMCS.MasterParameter GetSingleParam(string name)
        {
            using (var db = new Data.EmcsContext())
            {
                var tb = db.MasterParameter.Where(a => a.Name == name);
                return tb.FirstOrDefault();
            }
        }

        public static int Crud(Data.Domain.EMCS.MasterParameter itm, string dml)
        {
            using (var db = new Data.RepositoryFactory(new Data.EmcsContext()))
            {
                if (dml == "I")
                {
                    itm.CreateBy = SiteConfiguration.UserName;
                    itm.CreateDate = DateTime.Now;
                }

                itm.UpdateBy = SiteConfiguration.UserName;
                itm.UpdateDate = DateTime.Now;

                CacheManager.Remove(CacheName);

                return db.CreateRepository<Data.Domain.EMCS.MasterParameter>().CRUD(dml, itm);
            }
        }

        public static Data.Domain.EMCS.MasterParameter GetParamById(long id)
        {
            using (var db = new Data.EmcsContext())
            {
                var tb = db.MasterParameter.Where(a => a.Id == id);
                return tb.FirstOrDefault();
            }
        }
        public static List<Data.Domain.EMCS.MasterParameter> GetParamByIdList(long id)
        {
            using (var db = new Data.EmcsContext())
            {
                var tb = db.MasterParameter.Where(a => a.Id == id);
                return tb.ToList();
            }
        }
    }
}
