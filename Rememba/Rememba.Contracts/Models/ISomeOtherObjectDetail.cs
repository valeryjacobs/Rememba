﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rememba.Contracts.Models
{
    public interface ISomeOtherObjectDetail
    {
         int SomeIntProp { get; set; }

         string SomeStringProp { get; set; }

         string SomeDetailProp { get; set; }
    }
}
