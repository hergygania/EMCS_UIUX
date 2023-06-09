using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using App.Data.Caching;
using App.Data.Domain;
using System.Data.Entity;
using App.Data.Domain.Extensions;

namespace App.Service.Master
{
    public class UserAcces
    {
        private const string cacheName = "App.master.user";
        private readonly static ICacheManager _cacheManager = new MemoryCacheManager();

        //public static IList<Data.Domain.UserAccess> GetList()
        //{
        //    string key = string.Format(cacheName);
        //    var list = new List<Data.Domain.UserAccess>();
        //    try
        //    {
        //        list = _cacheManager.Get(key, () =>
        //        {
        //            using(var db = new Data.RepositoryFactory(new Data.EfDbContext()))
        //            {
        //                var tb = db.CreateRepository<Data.Domain.UserAccess>().TableNoTracking.Select(e => e);
        //                return tb.ToList();
        //            }
        //        });
        //    }
        //    catch(Exception ex)
        //    {
        //        var exx = ex.Message;
        //    }
        //    return list;
        //}

        public static List<Data.Domain.UserAccess> GetList()
        {
            string key = string.Format(cacheName);
            var list = _cacheManager.Get(key, () =>
            {
                using (var db = new Data.EfDbContext())
                {
                    var tb = from c in db.UserAccesses
                             join g in db.Groups on c.GroupID equals g.ID into UserGroup
                             from g1 in UserGroup.DefaultIfEmpty()
                             join l in db.Levels on c.LevelID equals l.ID into UserLevel
                             from g2 in UserLevel.DefaultIfEmpty()
                             join r in db.RoleAccesses on c.RoleID equals r.RoleID into UserRole
                             from g3 in UserRole.DefaultIfEmpty()

                             select new UserAccessTable()
                             {
                                 UserID = c.UserID,
                                 FullName = c.FullName,
                                 Phone = c.Phone,
                                 Email = c.Email,
                                 Password = c.Password,
                                 UserType = c.UserType,
                                 Cust_Group_No = c.Cust_Group_No,
                                 Status = c.Status,
                                 GroupID = c.GroupID,
                                 EntryDate = c.EntryDate,
                                 ModifiedDate = c.ModifiedDate,
                                 EntryBy = c.EntryBy,
                                 ModifiedBy = c.ModifiedBy,
                                 SelectedGroup = g1.Name,
                                 Position = c.Position,
                                 LevelID = c.LevelID,
                                 SelectedLevel = g2.Name,
                                 RoleID = c.RoleID,
                                 SelectedRole = g3.Description

                             };
                    return ConvertToStoreList(tb.ToList());
                }
            });

            return list;
        }

        public static List<UserAccess> ConvertToStoreList(List<UserAccessTable> dataList)
        {
            List<UserAccess> partList = new List<UserAccess>();
            foreach (var c in dataList)
            {
                partList.Add(new UserAccess()
                {
                    UserID = c.UserID,
                    FullName = c.FullName,
                    Phone = c.Phone,
                    Email = c.Email,
                    Password = c.Password,
                    UserType = c.UserType,
                    Cust_Group_No = c.Cust_Group_No,
                    Status = c.Status,
                    GroupID = c.GroupID,
                    EntryDate = c.EntryDate,
                    ModifiedDate = c.ModifiedDate,
                    EntryBy = c.EntryBy,
                    ModifiedBy = c.ModifiedBy,
                    SelectedGroup = c.SelectedGroup,
                    Position = c.Position,
                    LevelID = c.LevelID,
                    SelectedLevel = c.SelectedLevel,
                    RoleID = c.RoleID,
                    SelectedRole = c.SelectedRole
                });
            }

            return partList;
        }

        public static IList<Data.Domain.UserAccess> GetList(Domain.MasterSearchForm crit)
        {
            var name = crit.searchName != null ? crit.searchName.Trim().ToLower() : "";

            var list = from c in GetList()
                       where (name == "" || (c.FullName).Trim().ToLower().Contains(name) 
                       || (c.UserID).Trim().ToLower().Contains(name))
                       orderby c.FullName
                       select c;
            return list.ToList();
        }

        public static Data.Domain.UserAccess GetId(string id)
        {
            if (string.IsNullOrEmpty(id))
                return new Data.Domain.UserAccess();

            var item = GetList().Where(w => w.UserID == id).FirstOrDefault();
            if (!string.IsNullOrEmpty(item.Cust_Group_No))
                item.Cust_Group_NoCap = PartTracking.V_GET_CUSTOMER_GROUP.GetItem(item.Cust_Group_No).CUNM;
            return item;
        }


        public async static Task<Data.Domain.UserAccess> GetPassword(string userId, string password)
        {
            using (var db = new Data.EfDbContext())
            {
<<<<<<< HEAD
<<<<<<< HEAD
                //var item = await db.UserAccesses.Where(c => c.UserID == userId).SingleOrDefaultAsync();
=======
>>>>>>> 639d8d0 (Intial commit)
=======
                //var item = await db.UserAccesses.Where(c => c.UserID == userId).SingleOrDefaultAsync();
>>>>>>> b773f28 (intial commit for changes from himanshu and vijendra)
                var item = await db.UserAccesses.Where(c => c.UserID == userId).SingleOrDefaultAsync();
                if (item != null)
                {
                    string _role = "", _roleMode = "";
                    var _rl = GetListUserRoles(userId);
                    if (_rl != null)
                    {
                        foreach (var e in _rl)
                        {
                            _role = _role + (string.IsNullOrEmpty(_role) ? "" : ",") + e.RoleAccess;
                            _roleMode = _roleMode + (string.IsNullOrEmpty(_roleMode) ? "" : ",") + e.RoleAccess + "_" + e.RoleAccessMode;
                        }
                    }
                    item.RoleAccess = _role;
                    item.RoleAccessMode = _roleMode;

                    //delay login
                    if (item.Status.HasValue && item.Status == 4)
                    {
                        if (item.ModifiedDate.Value.AddMinutes(Domain.SiteConfiguration.LoginDelayTime) > DateTime.Now)
                        {
                            item.Status = 4;
                        }
                        else
                        {
                            item.Status = 1;
                        }
                    }
                }
                return item;
            }
        }

        public async static Task<int> ChangePassword(string userId, string oldPwd, string newPwd)
        {
            int ret = 0;
            var item = new Data.Domain.UserAccess();

            using (var db = new Data.EfDbContext())
            {
                item = await db.UserAccesses.Where(c => c.UserID == userId && c.Password == oldPwd).SingleOrDefaultAsync();
                if (item != null)
                {
                    item.Password = newPwd;
                    item.ModifiedBy = userId;
                    item.ModifiedDate = DateTime.Now;
                    ret = 1;
                }
                else
                {
                    return -1;
                }
            }

            using (var db = new Data.RepositoryFactory(new Data.EfDbContext()))
            {
                ret = await db.CreateRepository<Data.Domain.UserAccess>().CrudAsync("U", item);
                return 1;
            }
        }

        public async static Task<int> SetPasswordWrong(string userId)
        {
            int ret = 0;
            int loginCount = 0; //LoginFailtUpdateCache(userId, null);
            var item = new Data.Domain.UserAccess();
            using (var db = new Data.EfDbContext())
            {
                item = await db.UserAccesses.Where(c => c.UserID == userId).SingleOrDefaultAsync();
                if (item != null)
                {
                    loginCount = (item.Status.HasValue ? item.Status.Value : 0);
                }
            }


            if (loginCount > 0)
            {
                loginCount = loginCount + 1;
                if (loginCount >= Domain.SiteConfiguration.LoginLocked + 1) //locked
                {
                    loginCount = 0;
                }

                using (var db = new Data.RepositoryFactory(new Data.EfDbContext()))
                {
                    item.Status = byte.Parse(loginCount.ToString());
                    if (loginCount >= Domain.SiteConfiguration.LoginDelay + 1 && loginCount < Domain.SiteConfiguration.LoginDelay + 2)
                    {
                        item.ModifiedDate = DateTime.Now;
                    }

                    ret = await db.CreateRepository<Data.Domain.UserAccess>().CrudAsync("U", item);
                }
            }
            return loginCount;
        }

        public async static Task<int> SetPasswordWrongClear(string userId)
        {
            using (var db = new Data.RepositoryFactory(new Data.EfDbContext()))
            {
                var item = await db.CreateRepository<Data.Domain.UserAccess>().GetAll().Where(w => w.UserID == userId).SingleOrDefaultAsync();
                item.Status = 1;
                return await db.CreateRepository<Data.Domain.UserAccess>().CrudAsync("U", item);
            }
        }


        public static Data.Domain.UserAccess GetUserRoles(string userId)
        {
            using (var db = new Data.EfDbContext())
            {
                var item = db.UserAccesses.Where(c => c.UserID == userId).SingleOrDefault();
                if (item != null)
                {
                    string _role = "", _roleMode = "";
                    var _rl = GetListUserRoles(userId);
                    if (_rl != null)
                    {
                        foreach (var e in _rl)
                        {
                            _role = _role + (string.IsNullOrEmpty(_role) ? "" : ",") + e.RoleAccess;
                            _roleMode = _roleMode + (string.IsNullOrEmpty(_roleMode) ? "" : ",") + e.RoleAccess + "_" + e.RoleAccessMode;
                        }
                    }
                    item.RoleAccess = _role;
                    item.RoleAccessMode = _roleMode;
                }
                return item;
            }
        }
        public static Data.Domain.UserAccess GetPassword(string userId)
        {
            using (var db = new Data.EfDbContext())
            {
                var item = db.UserAccesses.Where(c => c.Email == userId).SingleOrDefault();
                return item;
            }
        }

        #region user access relation
        public static List<Data.Domain.UserAccess> GetListUserRoles(string userId)
        {
            using (var db = new Data.EfDbContext())
            {
                var tb = db.UserAccess_Role.Where(c => c.UserID == userId).Select(s => new { s.RoleID, s.RoleMode }).ToList();
                var list = from c in Master.Roles.GetList()
                           from u in tb.Where(w => w.RoleID == c.RoleID)
                           select new Data.Domain.UserAccess
                           {
                               RoleAccess = c.RoleName,
                               RoleAccessMode = u.RoleMode
                           };
                return list.ToList();
            }
        }

        public static List<Data.Domain.UserAccess_Role> GetListRoles(string id)
        {
            if (string.IsNullOrEmpty(id))
                return new List<Data.Domain.UserAccess_Role>();

            using (var db = new Data.RepositoryFactory(new Data.EfDbContext()))
            {
                var tb = db.CreateRepository<Data.Domain.UserAccess_Role>().TableNoTracking.Where(w => w.UserID == id).Select(e => e);
                return tb.ToList();
            }
        }
        public static List<Data.Domain.UserAccess_Area> GetListAreas(string id)
        {
            if (string.IsNullOrEmpty(id))
                return new List<Data.Domain.UserAccess_Area>();

            using (var db = new Data.RepositoryFactory(new Data.EfDbContext()))
            {
                var tb = db.CreateRepository<Data.Domain.UserAccess_Area>().TableNoTracking.Where(w => w.UserID == id).Select(e => e);
                return tb.ToList();
            }
        }
        public static List<Data.Domain.UserAccess_Hub> GetListHubs(string id)
        {
            if (string.IsNullOrEmpty(id))
                return new List<Data.Domain.UserAccess_Hub>();

            using (var db = new Data.RepositoryFactory(new Data.EfDbContext()))
            {
                var tb = db.CreateRepository<Data.Domain.UserAccess_Hub>().TableNoTracking.Where(w => w.UserID == id).Select(e => e);
                return tb.ToList();
            }
        }
        public static List<Data.Domain.UserAccess_Store> GetListStores(string id)
        {
            if (string.IsNullOrEmpty(id))
                return new List<Data.Domain.UserAccess_Store>();

            using (var db = new Data.RepositoryFactory(new Data.EfDbContext()))
            {
                var tb = db.CreateRepository<Data.Domain.UserAccess_Store>().TableNoTracking.Where(w => w.UserID == id).Select(e => e);
                return tb.ToList();
            }
        }

        #endregion

        public static int Update(
            Data.Domain.UserAccess item,
            int[] roles,
            //string[] rolesMode,
            int[] areas,
            int[] hubs,
            int[] stores,
            string dml)
        {

            item.ModifiedBy = Domain.SiteConfiguration.UserName;
            item.ModifiedDate = DateTime.Now;

            using (TransactionScope ts = new System.Transactions.TransactionScope())
            {
                using (var db = new Data.RepositoryFactory(new Data.EfDbContext()))
                {

                    if (dml == "I")
                    {
                        item.Status = item.Status.HasValue ? item.Status.Value : byte.Parse("1");
                        item.EntryBy = Domain.SiteConfiguration.UserName;
                        item.EntryDate = DateTime.Now;
                    }
                    else
                    {
                        db.DbContext.Database.ExecuteSqlCommand(@"
							DELETE FROM UserAccess_Role WHERE UserID={0};
							DELETE FROM UserAccess_Area WHERE UserID={0};
							DELETE FROM UserAccess_Hub WHERE UserID={0};
							DELETE FROM UserAccess_Store WHERE UserID={0}",
                        item.UserID);

//                        db.DbContext.Database.ExecuteSqlCommand(@"
//							DELETE FROM UserAccess_Area WHERE UserID={0};
//							DELETE FROM UserAccess_Hub WHERE UserID={0};
//							DELETE FROM UserAccess_Store WHERE UserID={0}",
//                        item.UserID);
                    }

                    if (item.UserType.ToLower() != "ext-part")
                        item.Cust_Group_No = null;

                    var ret = db.CreateRepository<Data.Domain.UserAccess>().CRUD(dml, item);

                    #region detail

                    //insert by role
                    var _detRole = new Data.Domain.UserAccess_Role { UserID = item.UserID, RoleID = Convert.ToInt32(item.RoleID), RoleMode = "Read" };                 
                    ret = db.CreateRepository<Data.Domain.UserAccess_Role>().CRUD("I", _detRole);
                    

                    //insert by role admin read
                    if (item.RoleID != 1)
                    {
                        var _detRoleAdmin = new Data.Domain.UserAccess_Role { UserID = item.UserID, RoleID = 1, RoleMode = "Read" };
                        ret = db.CreateRepository<Data.Domain.UserAccess_Role>().CRUD("I", _detRoleAdmin);
                    }

                    areas = (areas == null ? new int[0] : areas);
                    foreach (var d in areas)
                    {
                        var _det = new Data.Domain.UserAccess_Area { UserID = item.UserID, AreaID = d };
                        ret = db.CreateRepository<Data.Domain.UserAccess_Area>().CRUD("I", _det);
                    }

                    hubs = (hubs == null ? new int[0] : hubs);
                    foreach (var d in hubs)
                    {
                        var _det = new Data.Domain.UserAccess_Hub { UserID = item.UserID, HubID = d };
                        ret = db.CreateRepository<Data.Domain.UserAccess_Hub>().CRUD("I", _det);
                    }

                    stores = (stores == null ? new int[0] : stores);
                    foreach (var d in stores)
                    {
                        var _det = new Data.Domain.UserAccess_Store { UserID = item.UserID, StoreID = d };
                        ret = db.CreateRepository<Data.Domain.UserAccess_Store>().CRUD("I", _det);
                    }

                    #endregion

                    ts.Complete();
                    _cacheManager.Remove(cacheName);
                    return ret;
                }
            }
        }

        public static Data.Domain.UserAccess getFullNameByID(string userID)
        {
            var item = GetList().Where(w => w.UserID == userID).FirstOrDefault();
            return item;
        }

        public static int Update(Data.Domain.UserAccess itm, string dml)
        {
            if (dml == "I")
            {
                itm.EntryBy = Domain.SiteConfiguration.UserName;
                itm.EntryDate = DateTime.Now;
            }

            itm.ModifiedBy = Domain.SiteConfiguration.UserName;
            itm.ModifiedDate = DateTime.Now;

            _cacheManager.Remove(cacheName);

            using (var db = new Data.RepositoryFactory(new Data.EfDbContext()))
            {
                return db.CreateRepository<Data.Domain.UserAccess>().CRUD(dml, itm);
            }
        }

    }
}
