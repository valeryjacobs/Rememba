using Rememba.Contracts.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rememba.Contracts.Services
{
    public interface IMindMapDataService
    {
        Task<List<IMindMap>> ListMindMaps();
        Task<IMindMap> Create(string name);
        Task Save(IMindMap mindMap, INode rootNode);
        Task Delete(string id);

        Task<IMindMap> GetMindMap(string id);

        Task<INode> GetRootNode(IMindMap mindMap);

        Task<INode> CloneNode(INode node);
    }
}
