using Rememba.Contracts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rememba.Contracts.ViewModels
{
    public interface ISomeObjectDetailViewModel : IViewModel
    {
        ISomeObjectDetail SelectedObject { get; set; }
    }
}
