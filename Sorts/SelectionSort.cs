using System.Collections.Generic;

namespace Sorting_algorithm_benchmark_grapher.Sorts
{
    internal class SelectionSort : ISorter
    {
        public string Title => "Selection sort";

        public string Message => "";

        public string Category => "Selection sorts";

        public Complexity Time => Complexity.QUADRATIC;

        public void RunSort<T>(T[] arr, int n, int parameter, IComparer<T> cmp)
        {

            // One by one move boundary of unsorted subarray
            for (int i = 0; i < n - 1; i++)
            {
                // Find the minimum element in unsorted array
                int min_idx = i;
                for (int j = i + 1; j < n; j++)
                {
                    if (cmp.Compare(arr[j], arr[min_idx]) < 0)
                    {
                        min_idx = j;
                    }
                }

                // Swap the found minimum element with the first
                // element
                (arr[i], arr[min_idx]) = (arr[min_idx], arr[i]);
            }
        }
    }
}
