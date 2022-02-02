using App.Data.Caching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Service.DeliveryTrackingStatus
{
    public class PrimeProduct
    {
        private const string cacheName = "App.DeliveryTrackingStatus.PrimeProduct";

        private readonly static ICacheManager _cacheManager = new MemoryCacheManager();

        public void ImportPrimeProduct()
        { 
        
        }

        public void ExportToExcel()
        { 
        
        }

        public void AdvanceSearch()
        { 
        
        }

        public void SimpleSearch()
        { 
        
        }

        public void Update()
        { 
        
        }

        
    }
}
