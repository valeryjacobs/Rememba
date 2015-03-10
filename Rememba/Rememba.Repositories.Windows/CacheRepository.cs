using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Windows;
using Windows.Storage;

namespace Rememba.Repositories.Windows
{
    public class CacheRepository
    {
        public async void ClearCache()
        {
            StorageFolder folder = ApplicationData.Current.LocalFolder;
            await StorageHelper.ClearCache(folder);
        }
    }
}
