using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Data.Domain
{
	public partial class LicenseManagementExtendComment
    {
				[NotMapped]
				public string StringId {
					get
					{
						return CommentID.ToString("000000000");
					}
				}

				[NotMapped]
				public string DayDesc
				{
					get
					{
						var ret = "today";
						if (EntryDate.HasValue)
						{
							var dy = (DateTime.Today - EntryDate.Value).TotalDays;
							if (dy > 0.50)
								ret = (DateTime.Today - EntryDate.Value).TotalDays.ToString("##") + " day(s) ago";
							else if ((DateTime.Now - EntryDate.Value).TotalMinutes > 59)
								ret = (DateTime.Now - EntryDate.Value).TotalHours.ToString("##") + " hours ago";
							else { 
								if((DateTime.Now - EntryDate.Value).TotalMinutes >1)
									ret = (DateTime.Now - EntryDate.Value).TotalMinutes.ToString("##") + " minutes ago";
								else
								{
									if((DateTime.Now - EntryDate.Value).TotalSeconds < 10)
										ret = " just now";
									else
										ret = (DateTime.Now - EntryDate.Value).TotalSeconds.ToString("#0") + " seconds ago";
								}
									
							}
						}
						return ret;
					}
				}

		}
}
