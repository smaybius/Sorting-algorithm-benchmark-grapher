using System.Collections.Generic;

namespace Sorting_algorithm_benchmark_grapher.Sorts
{
    internal class TournamentSort : ISorter
    {
        public string Title => "Tournament sort";

        public string Message => "";

        public string Category => "Selection sorts";

        public Complexity Time => Complexity.GOOD;

        public void RunSort<T>(T[] array, int sortLength, int parameter, IComparer<T> cmp)
        {
            _ = new TournamentSorter<T>(array, sortLength, cmp);
        }
    }
}
