﻿using Rememba.Contracts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rememba.Model
{
    public class SomeObjectDetail : ISomeObjectDetail
    {
        public int SomeIntProp
        {
            get;
            set;
        }

        public string SomeStringProp
        {
            get;
            set;
        }

        public string SomeDetailProp
        {
            get;
            set;
        }

        public List<ISomeOtherObjectInfo> SetOfOtherObject
        {
            get;
            set;
        }
    }
}
