using Rememba.Contracts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rememba.Contracts.Plugins
{
    public interface INodePlugin
    {
        Task<INode> GetNode();
    }
}
