using Rememba.Contracts.Models;
using Rememba.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rememba.Repositories.Windows
{
    public class ContentRepository
    {
        public async Task<ObservableCollection<IContent>> GetContentItems()
        {
            var contentItems = new ObservableCollection<IContent>();

            contentItems.Add(new Content
            {
               Id = Guid.NewGuid().ToString(),
               Data = "This is content"
            });

            return contentItems;
        }

        public async Task<IContent> GetContent(string contentId)
        {
            return new Content
            {
                Id = Guid.NewGuid().ToString(),
                Data = "This is content"
            };
        }
    }
}
