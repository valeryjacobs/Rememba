﻿using Rememba.Contracts.Models;
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
        Task<INode> GetRootNode(IMindMap mindMap);

        Task<IMindMap> GetMindMap(string mindMapName);
      
    }
}
