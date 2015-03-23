using Rememba.Contracts.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rememba.Contracts.Services
{
    public interface IContentDataService
    {
        Task<ObservableCollection<IContent>> GetContentItems();

        Task<IContent> GetContent(string id);

        Task AddContent(IContent content);

        Task UpdateContent(IContent content);

        Task DeleteContent(string id);
        Task ClearCache();
    }
}
