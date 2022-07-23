using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sorting_algorithm_benchmark_grapher
{
    internal class GrailSorter : ISorter
    {
        public string Title => "Grail sort";

        public string Message => "";

        public string Category => "Merge sorts";

        public Complexity Time => Complexity.GOOD;

        public void RunSort<T>(T[] array, int sortLength, double parameter, IComparer<T> cmp)
        {
            GrailsortTester.GrailSort<T> gs = new(cmp);
            gs.GrailSortInPlace(array, 0, sortLength);
        }
    }
}
