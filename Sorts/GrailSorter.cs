using System.Collections.Generic;

namespace Sorting_algorithm_benchmark_grapher.Sorts
{
    internal class GrailSorter : ISorter
    {
        public string Title => "Grail sort";

        public string Message => "";

        public string Category => "Merge sorts";

        public Complexity Time => Complexity.GOOD;

        public void RunSort<T>(T[] array, int sortLength, int parameter, IComparer<T> cmp)
        {
            GrailSort<T> gs = new(cmp);
            gs.GrailSortInPlace(array, 0, sortLength);
        }
    }
}
