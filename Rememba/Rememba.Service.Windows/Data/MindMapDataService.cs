using Newtonsoft.Json.Linq;
using Rememba.Contracts.Models;
using Rememba.Contracts.Services;
using Rememba.Model;
using Rememba.Repositories.Windows;
using Rememba.Shared;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rememba.Service.Windows.Data
{
    public class MindMapDataService : IMindMapDataService
    {
        public async Task<IMindMap> GetMindMap(string mindMapName)
        {
            MindMapRepository rep = new MindMapRepository();

            var mindmap = await rep.GetMindMap(mindMapName);

            return mindmap;
        }

        public async Task<INode> GetRootNode(IMindMap mindMap)
        {
            dynamic json = await Task.Run(() => JValue.Parse(mindMap.Content));

            return await Task.Run(() => TreeHelper.BuildTree(new Node(), json));
        }
    }
}
