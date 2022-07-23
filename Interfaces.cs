using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sorting_algorithm_benchmark_grapher
{
    public interface IDistribution
    {
        public string Title { get; }

        public void InitializeArray(ArrayInt[] array);
    }

    public interface IShuffle
    {
        public string Title { get; }

        public void ShuffleArray<T>(T[] array, int start, int end, IComparer<T> cmp);
    }

    public interface ISorter
    {
        public string Title { get; }
        public string Message { get; }
        public string Category { get; }
        public Complexity Time { get; }
        public void RunSort<T>(T[] array, int sortLength, double parameter, IComparer<T> cmp);
    }
}
