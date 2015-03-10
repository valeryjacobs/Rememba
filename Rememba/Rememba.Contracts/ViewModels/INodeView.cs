using Rememba.Contracts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rememba.Contracts.ViewModels
{
    public interface INodeView
    {
        INode SelectedNode { get; set; }
        INode RootNode { get; set; }
    }
}
