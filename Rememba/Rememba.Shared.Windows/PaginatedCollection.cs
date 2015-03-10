using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml.Data;

namespace Rememba.Shared.Windows
{
    public class PaginatedCollection<T> : ObservableCollection<T>,
        ISupportIncrementalLoading
    {
        private Func<uint, Task<IEnumerable<T>>> load;
        public bool HasMoreItems { get; protected set; }

        public PaginatedCollection(Func<uint, Task<IEnumerable<T>>> load)
        {
            HasMoreItems = true;
            this.load = load;
        }

        public IAsyncOperation<LoadMoreItemsResult> LoadMoreItemsAsync(uint count)
        {
            return AsyncInfo.Run(async c =>
            {
                var data = await load(count);

                foreach (var item in data)
                {
                    Add(item);
                }

                HasMoreItems = true;

                return new LoadMoreItemsResult()
                {
                    Count = (uint)data.Count<T>()
                };
            });
        }
    }
}
