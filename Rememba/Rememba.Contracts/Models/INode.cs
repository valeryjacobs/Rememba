using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rememba.Contracts.Models
{
    public interface INode
    {
        System.Collections.ObjectModel.ObservableCollection<INode> Children { get; set; }
        string ContentId { get; set; }
        string Description { get; set; }
        string Id { get; set; }
        bool MarkedForDeletion { get; set; }
        INode Parent { get; set; }
        string Title { get; set; }
    }
}
