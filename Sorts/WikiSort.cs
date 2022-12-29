using System.Collections.Generic;

namespace Sorting_algorithm_benchmark_grapher.Sorts
{
    internal class WikiSort : ISorter
    {
        public string Title => "Wiki sort";

        public string Message => "Enter external buffer size (0 for in-place)";

        public string Category => "Merge sorts";

        public Complexity Time => Complexity.GOOD;

        public void RunSort<T>(T[] array, int sortLength, int parameter, IComparer<T> cmp)
        {
            int cache = parameter;
            WikiSorter<T> ws = new(cache, cmp);
            ws.WSort(array, sortLength);
        }
    }
}
