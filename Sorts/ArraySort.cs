using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sorting_algorithm_benchmark_grapher
{
    internal class ArraySort : ISorter
    {
        public string Title => "Array.Sort()";

        public string Message => "";

        public string Category => "Quick sorts";

        public Complexity Time => Complexity.GOOD;

        public void RunSort<T>(T[] array, int end, double parameter, IComparer<T> cmp)
        {
            Array.Sort(array, 0, end, comparer: cmp);
        }
    }
}
