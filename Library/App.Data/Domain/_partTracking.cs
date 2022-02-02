namespace App.Data
{
	using System;
	using System.Data.Entity;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Linq;
	using App.Data.Domain;

	public partial class EfDbContext
	{

        public virtual DbSet<V_GET_CUSTOMER> V_GET_CUSTOMER 
        { 
            get; 
            set; 
        }
        public virtual DbSet<V_GET_CUSTOMER_GROUP> V_GET_CUSTOMER_GROUP 
        { 
            get; 
            set; 
        }

        public virtual DbSet<V_MODEL_LIST> V_MODEL_LIST 
        { 
            get; 
            set; 
        }
        public virtual DbSet<V_CUSTODER_DETAIL> V_CUSTODER_DETAIL 
        { 
            get;
            set; 
        }
        public virtual DbSet<V_CUSTORDER_HEADER> V_CUSTORDER_HEADER 
        { 
            get;
            set; 
        }


	}
}
