using Rememba.Contracts.Models;
using Rememba.Contracts.Services;
using Rememba.Repositories.Windows;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rememba.Service.Windows.Data
{
    public class ContentDataService : IContentDataService
    {
        public async Task<ObservableCollection<IContent>> GetContentItems()
        {
            ContentRepository rep = new ContentRepository();

            var contentItems = await rep.GetContentItems();

            return contentItems;
        }

        public async Task<IContent> GetContent(string contentId)
        {
            ContentRepository rep = new ContentRepository();

            var contentItem = await rep.GetContent(contentId);

            return contentItem;
        }
    }
}
