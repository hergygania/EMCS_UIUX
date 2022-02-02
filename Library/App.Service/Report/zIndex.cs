using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Data.SqlClient;
using App.Data;

namespace App.Service.Report
{
	public class zIndex
	{

		public static DataTable TableIndex(string tblNm)
		{
			var where="";
			if(!string.IsNullOrEmpty(tblNm))
				where = " and o.name like '%" + tblNm+"%'";

			var sql = @"
					select 
						OBJECT_SCHEMA_NAME(o.[object_id],DB_ID()) AS [Schema],
						o.name as ObjectName, 
						i.name as IndexName, 
						i.is_primary_key as [PrimaryKey],
						SUBSTRING(i.[type_desc],0,6) as IndexType,
						i.is_unique as [Unique],
						Columns.[Normal] as IndexColumns,
						Columns.[Included] as IncludedColumns
					from sys.indexes i 
					join sys.objects o on i.object_id = o.object_id
					cross apply
					(
							select
									substring
									(
											(
													select ', ' + co.[name]
													from sys.index_columns ic
													join sys.columns co on co.object_id = i.object_id and co.column_id = ic.column_id
													where ic.object_id = i.object_id and ic.index_id = i.index_id and ic.is_included_column = 0
													order by ic.key_ordinal
													for xml path('')
											)
											, 3
											, 10000
									)    as [Normal]    
									, substring
									(
											(
													select ', ' + co.[name]
													from sys.index_columns ic
													join sys.columns co on co.object_id = i.object_id and co.column_id = ic.column_id
													where ic.object_id = i.object_id and ic.index_id = i.index_id and ic.is_included_column = 1
													order by ic.key_ordinal
													for xml path('')
											)
											, 3
											, 10000
									)    as [Included]    

					) Columns
					where o.[type] = 'U' " + where + @"
					order by [Schema],o.[name], i.[name], i.is_primary_key desc";

			DataTable tbl = QueryToTable(new EfDbContext(), sql, null);
			return tbl;
		}

		public static DataTable TableDef(string tblNm)
		{
			var sql = @"
					SELECT a.[name] as 'Table',
						b.[name] as 'Column',
						c.[name] as 'Datatype',
						b.[length] as 'Length',
						CASE WHEN b.[cdefault] > 0 THEN d.[text] ELSE NULL END as 'Default',
						CASE WHEN b.[isnullable] = 0 THEN 'No' ELSE 'Yes' END as 'Nullable'
					FROM sysobjects a
					INNER JOIN syscolumns b ON a.[id] = b.[id]
					INNER JOIN systypes c ON b.[xtype] = c.[xtype]
					LEFT JOIN syscomments d ON b.[cdefault] = d.[id]
					WHERE a.[xtype] = 'u' AND a.[name] <> 'dtproperties'
					and a.[name] like '" + tblNm +@"%'
					ORDER BY a.[name],b.[colorder]
					";

			DataTable tbl = QueryToTable(new EfDbContext(), sql, null);
			return tbl;
		}

		public static DataTable QueryToTable(DbContext db, string queryText, SqlParameter[] parametes)
		{
			using(DbDataAdapter adapter = new SqlDataAdapter())
			{
				adapter.SelectCommand = db.Database.Connection.CreateCommand();
				adapter.SelectCommand.CommandText = queryText;
				if(parametes != null)
					adapter.SelectCommand.Parameters.AddRange(parametes);
				DataTable table = new DataTable();
				adapter.Fill(table);
				return table;
			}
		}

	}
}
