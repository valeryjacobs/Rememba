using Rememba.Contracts.Models;
using Rememba.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rememba.Shared
{
    public static class TreeHelper
    {
        public static INode BuildTree(INode node, dynamic source)
        {
            try
            {
                if (source.id == null) source.id = Guid.NewGuid().ToString();
                //if (source.d == null) source.d = "";
                //if (source.n == null) source.n = "";
                var newNode = new Node
                          {
                              Id = source.id,
                              ContentId = source.cid,
                              Description = source.d,
                              Title = source.n,
                              Parent = node,
                              Edit = false
                          };

                if (source.c != null)
                {
                    foreach (dynamic d in source.c)
                    {
                        newNode.Children.Add(BuildTree(newNode, d));
                    }
                }

                return newNode;
            }
            catch(Exception ex)
            {
                return null;
            }
        }
    }
}
