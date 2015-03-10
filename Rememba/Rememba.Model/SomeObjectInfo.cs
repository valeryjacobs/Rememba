using Rememba.Contracts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rememba.Model
{
    public class SomeObjectInfo : ISomeObjectInfo
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

        public List<ISomeOtherObjectInfo> SetOfOtherObject
        {
            get;
            set;
        }
    }
}
