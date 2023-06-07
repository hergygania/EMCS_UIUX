using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

namespace App.Service.Vetting
{
	public class SurveyDocument
	{


		public static List<Data.Domain.SurveyDocument> GetList(int surveyId)
		{
			using(var db = new Data.EfDbContext())
			{
				var tbl = db.SurveyDocuments.Where(w => w.SurveyID == surveyId).ToList();
				var list = from c in tbl
									 from doc in Service.Master.DocumentTypes.GetList().Where(w => w.DocumentTypeID == c.DocumentTypeID)
									 select new Data.Domain.SurveyDocument()
									 {
										 SurveyID = c.SurveyID,
										 SurveyDocumentID = c.SurveyDocumentID,
										 DocumentTypeID = c.DocumentTypeID,
										 FileName = c.FileName,
										 FilePath = c.FilePath,
										 EntryDate = c.EntryDate,
										 ModifiedDate = c.ModifiedDate,
										 EntryBy = c.EntryBy,
										 ModifiedBy = c.ModifiedBy,
										 EntryDate_Date = c.EntryDate_Date,
										 DocumentName = doc.DocumentName
									 };


				return list.ToList();
			}
		}

		public static Data.Domain.SurveyDocument GetId(int id)
		{
			using(var db = new Data.EfDbContext())
			{
				var item = db.SurveyDocuments.Where(w => w.SurveyDocumentID == id).FirstOrDefault();

				return item;
			}
		}

		public static int Update(Data.Domain.SurveyDocument itm, string dml)
		{
			string userName = Domain.SiteConfiguration.UserName;
			itm.ModifiedBy = userName;
			itm.ModifiedDate = DateTime.Now;

			using(var db = new Data.RepositoryFactory(new Data.EfDbContext()))
			{
				if(dml == "I")
				{
					itm.EntryBy = userName;
					itm.EntryDate = DateTime.Now;
				}

				return db.CreateRepository<Data.Domain.SurveyDocument>().CRUD(dml, itm);
			}
		}


	}
}
