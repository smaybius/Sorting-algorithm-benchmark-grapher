using System.Collections.Generic;

namespace Sorting_algorithm_benchmark_grapher.Sorts
{
    internal class OddEvenSort : ISorter
    {
        public string Title => "Odd-even sort";

        public string Message => "";

        public string Category => "Exchange sorts";

        public Complexity Time => Complexity.QUADRATIC;

        public void RunSort<T>(T[] arr, int n, int parameter, IComparer<T> cmp)
        {
            // Initially array is unsorted
            bool isSorted;

            do
            {
                isSorted = true;

                // Perform Bubble sort on
                // odd indexed element
                for (int i = 1; i <= n - 2; i += 2)
                {
                    if (cmp.Compare(arr[i], arr[i + 1]) > 0)
                    {
                        (arr[i + 1], arr[i]) = (arr[i], arr[i + 1]);
                        isSorted = false;
                    }
                }

                // Perform Bubble sort on
                // even indexed element
                for (int i = 0; i <= n - 2; i += 2)
                {
                    if (cmp.Compare(arr[i], arr[i + 1]) > 0)
                    {
                        (arr[i + 1], arr[i]) = (arr[i], arr[i + 1]);
                        isSorted = false;
                    }
                }
            } while (!isSorted);
        }
    }
}
