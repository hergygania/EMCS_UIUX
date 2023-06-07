using App.Data.Caching;
using System;
using System.Collections.Generic;
using System.Linq;
using App.Domain;


namespace App.Service.POST
{
    public static class StParameter
    {
        public const string CacheName = "App.POST.StParameter";

        public readonly static ICacheManager CacheManager = new MemoryCacheManager();


        public static List<Data.Domain.POST.StParameter> GetData()
        {
            using (var db = new Data.POSTContext())
            {
                var tb = db.StParameter.Where(a=>a.IsActive && !a.IsDelete).OrderBy(a => a.Sort).ToList();
                return tb;
            }
        }

        public static List<Data.Domain.POST.StParameter> GetParamByGroup(string groupName)
        {
            using (var db = new Data.POSTContext())
            {
                var tb = db.StParameter.Where(a => a.Group == groupName).AsQueryable().OrderBy(a => a.Sort);
                return tb.ToList();
            }
        }

        public static dynamic GetSelectOption(MasterSearchForm crit)
        {
            string search = crit.searchName ?? "";
            string group = crit.searchCode ?? "";

            using (var db = new Data.POSTContext())
            {
                var data = db.StParameter
                    .Where(i => i.Group.Equals(@group) && i.Name.Contains(search))
                    .OrderBy(i => i.Sort)
                    .Skip(0).Take(100)
                    .Select(i => new
                    {
                        id = i.Value,
                        text = i.Name
                    }).ToList();

                return data.Select(i => new Data.Domain.POST.SelectItem2 { Id = i.text, Text = i.text }).ToList();
            }
        }

        public static dynamic GetParamOptions(MasterSearchForm crit)
        {
            string search = crit.searchName ?? "";
            string group = crit.searchCode ?? "";

            using (var db = new Data.POSTContext())
            {
                List<Data.Domain.POST.SelectItem2> data;
                if (@group.Equals("ContainerType"))
                {
                    data = db.StParameter
                        .Where(i => i.Group.Equals(@group) && i.Description.Contains(search))
                        .OrderBy(i => i.Sort)
                        .Skip(0).Take(100)
                        .Select(i => new Data.Domain.POST.SelectItem2
                        {
                            Id = i.Value,
                            Text = i.Description
                        }).ToList();
                }
                else
                {
                    data = db.StParameter
                        .Where(i => i.Group.Equals(@group) && i.Name.Contains(search))
                        .OrderBy(i => i.Sort)
                        .Skip(0).Take(100)
                        .Select(i => new Data.Domain.POST.SelectItem2
                        {
                            Id = i.Value,
                            Text = i.Name
                        }).ToList();
                }

                return data.ToList();
            }
        }

        public static List<Data.Domain.POST.SelectItem2> GetSelectList(string group)
        {
            using (var db = new Data.POSTContext())
            {
                var data = db.StParameter
                    .Where(i => i.Group.Equals(@group))
                    .OrderBy(i => i.Sort)
                    .Skip(0).Take(100)
                    .Select(i => new
                    {
                        id = i.Value,
                        text = i.Name
                    }).ToList();

                return data.Select(i => new Data.Domain.POST.SelectItem2 { Id = i.text, Text = i.text }).ToList();
            }
        }

        public static List<Data.Domain.POST.SelectItem2> GetSelectList2(string group)
        {
            using (var db = new Data.POSTContext())
            {
                var data = db.StParameter
                    .Where(i => i.Group.Equals(@group))
                    .OrderBy(i => i.Sort)
                    .Skip(0).Take(100)
                    .Select(i => new
                    {
                        id = i.Value,
                        text = i.Name,
                        sort = i.Sort
                    }).AsEnumerable().OrderBy(a => Convert.ToInt32(a.sort)).ToList();

                return data.Select(i => new Data.Domain.POST.SelectItem2 { Id = i.text, Text = i.text }).ToList();
            }
        }

        public static List<Data.Domain.POST.SelectItem3> GetSelectListDesc(string group)
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

                return data.Select(i => new Data.Domain.POST.SelectItem3 { Id = i.text, Text = i.text, Extra = i.desc }).ToList();
            }
        }

        public static List<Data.Domain.POST.StParameter> GetParamByValue(string groupName, string value)
        {
            using (var db = new Data.POSTContext())
            {
                var tb = db.StParameter.Where(a => a.Group == groupName && a.Value == value);
                return tb.ToList();
            }
        }

        public static List<Data.Domain.POST.StParameter> GetParamByName(string groupName, string name)
        {
            using (var db = new Data.POSTContext())
            {
                var tb = db.StParameter.Where(a => a.Group == groupName && a.Name == name);
                return tb.ToList();
            }
        }

        public static Data.Domain.POST.StParameter GetSingleParam(string name)
        {
            using (var db = new Data.POSTContext())
            {
                var tb = db.StParameter.Where(a => a.Name == name);
                return tb.FirstOrDefault();
            }
        }

        public static int Crud(Data.Domain.POST.StParameter itm, string dml)
        {
            using (var db = new Data.RepositoryFactory(new Data.POSTContext()))
            {
                if (dml == Global.dmlinsert)
                {
                    itm.CreatedBy = SiteConfiguration.UserName;
                    itm.CreatedOn = DateTime.Now;
                    itm.IsActive = true;
                    itm.IsDelete = false;

                }
                else
                {
                    itm.UpdatedBy = SiteConfiguration.UserName;
                    itm.UpdatedOn = DateTime.Now;
                }

                

                CacheManager.Remove(CacheName);

                return db.CreateRepository<Data.Domain.POST.StParameter > ().CRUD(dml, itm);
            }
        }

        public static Data.Domain.POST.StParameter GetParamById(long id)
        {
            using (var db = new Data.POSTContext())
            {
                var tb = db.StParameter.Where(a => a.Id == id);
                return tb.FirstOrDefault();
            }
        }
    }
}
