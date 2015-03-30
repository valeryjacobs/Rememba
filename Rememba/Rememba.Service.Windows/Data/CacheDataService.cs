using Rememba.Contracts.Services;
using Rememba.Repositories.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rememba.Service.Windows.Data
{
    public class CacheDataService : ICacheDataService
    {
    

        void ICacheDataService.ClearCache()
        {
            new CacheRepository().ClearCache();
        }
    }
}
