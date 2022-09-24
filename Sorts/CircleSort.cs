using System.Collections.Generic;

namespace Sorting_algorithm_benchmark_grapher.Sorts
{
    internal class CircleSort : ISorter
    {
        public string Title => "Circle sort";

        public string Message => "";

        public string Category => "Exchange sorts";

        public Complexity Time => Complexity.GOOD;

        private static int end;

        public static int CircleSortRoutine<T>(T[] array, int length, IComparer<T> cmp)
        {
            int swapCount = 0;
            for (int gap = length / 2; gap > 0; gap /= 2)
            {
                for (int start = 0; start + gap < end; start += 2 * gap)
                {
                    int high = start + (2 * gap) - 1;
                    int low = start;

                    while (low < high)
                    {
                        if (high < end && cmp.Compare(array[low], array[high]) > 0)
                        {
                            Sort.Swap(array, low, high);
                            swapCount++;
                        }

                        low++;
                        high--;
                    }
                }
            }
            return swapCount;
        }

        public void RunSort<T>(T[] array, int sortLength, int parameter, IComparer<T> cmp)
        {
            end = sortLength;
            int n = 1;
            for (; n < sortLength; n *= 2)
            {
                ;
            }

            int numberOfSwaps;
            do
            {
                numberOfSwaps = CircleSortRoutine(array, n, cmp);
            } while (numberOfSwaps != 0);
        }
    }
}
