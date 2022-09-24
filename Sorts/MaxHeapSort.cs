using System.Collections.Generic;

namespace Sorting_algorithm_benchmark_grapher.Sorts
{
    internal class MaxHeapSort : ISorter
    {
        public string Title => "Max heap sort";

        public string Message => "";

        public string Category => "Selection sorts";

        public Complexity Time => Complexity.GOOD;

        public void RunSort<T>(T[] array, int sortLength, int parameter, IComparer<T> cmp)
        {
            HeapSorting.HeapSort(array, 0, sortLength, true, cmp);
        }
    }
}
