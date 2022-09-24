using System;
using System.Collections.Generic;

namespace Sorting_algorithm_benchmark_grapher.Sorts
{
    internal class ArraySort : ISorter
    {
        public string Title => "Array.Sort()";

        public string Message => "";

        public string Category => "Quick sorts";

        public Complexity Time => Complexity.GOOD;

        public void RunSort<T>(T[] array, int end, int parameter, IComparer<T> cmp)
        {
            Array.Sort(array, 0, end, comparer: cmp);
        }
    }
}
