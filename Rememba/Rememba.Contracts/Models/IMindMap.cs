using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rememba.Contracts.Models
{
    public interface IMindMap
    {
        string Content { get; set; }
        string ContentUri { get; set; }
        string Id { get; set; }
        string Name { get; set; }
        string Touched { get; set; }
    }
}
