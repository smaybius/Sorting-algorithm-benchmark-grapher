using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sorting_algorithm_benchmark_grapher.Sorts
{
    internal class OrderBySort : ISorter
    {
        public string Title => "OrderBy sort";

        public string Message => "";

        public string Category => "Misc sorts";

        public Complexity Time => Complexity.GOOD;

        public void RunSort<T>(T[] array, int sortLength, double parameter, IComparer<T> cmp)
        {
            var sorted = array.OrderBy(x => x, cmp).ToArray();
        }
    }
}
