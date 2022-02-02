using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Transactions;

namespace App.Service.Master
{
	public class Emails
	{

		public async static Task<int> Update(
			Data.Domain.Email item,
			List<Data.Domain.EmailAttachment> documents,
			string dml)
		{

			string userName = Domain.SiteConfiguration.UserName;
			item.ModifiedBy = userName;
			item.ModifiedDate = DateTime.Now;
			var ret = 0;

			using(TransactionScope ts = new System.Transactions.TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
			{
				using(var db = new Data.RepositoryFactory(new Data.EfDbContext()))
				{

					if(dml == "I")
					{
						item.EntryBy = userName;
						item.EntryDate = DateTime.Now;
					}

					ret = await db.CreateRepository<Data.Domain.Email>().CrudAsync(dml, item);

					#region document
					var _dml = dml;
					documents = (documents == null ? new List<Data.Domain.EmailAttachment>() : documents);
					foreach(var doc in documents)
					{
						_dml = dml;
						doc.EmailID = item.EmailID;
						doc.ModifiedBy = userName;
						doc.ModifiedDate = DateTime.Now;
						doc.EntryDate = DateTime.Now;
						doc.EntryBy = userName;
						_dml = "I";


						ret = await db.CreateRepository<Data.Domain.EmailAttachment>().CrudAsync(_dml, doc);

					}

					#endregion

					ts.Complete();
					return ret;
				}
			}
		}

	}
}
