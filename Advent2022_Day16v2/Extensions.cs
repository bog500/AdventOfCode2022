using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2022_Day16v2
{
    public static class Extensions
    {
        public static void Pop<T>(this List<T> list)
        {
            if (list.Any())
                list.RemoveAt(list.Count - 1);
        }
    }
}
