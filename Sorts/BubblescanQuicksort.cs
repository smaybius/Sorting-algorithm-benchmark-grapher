using System;
using System.Collections.Generic;
/**
 *
 * @author aphitorite
 * @author thatsOven
 */
namespace Sorting_algorithm_benchmark_grapher.Sorts
{
    internal class BubblescanQuicksort : IIntegerSorter
    {
        public string Title => "Bubblescan quicksort";

        public string Message => "";

        public string Category => "Quick sorts";

        public Complexity Time => Complexity.GOOD;

        private static int Partition<T>(T[] array, int a, int b, T val, IComparer<T> cmp)
        {
            int i = a, j = b - 1;
            while (i <= j)
            {
                while (cmp.Compare(array[i], val) < 0)
                {
                    i++;
                }
                while (cmp.Compare(array[j], val) > 0)
                {
                    j--;
                }

                if (i <= j)
                {
                    Sort.Swap(array, i++, j--);
                }
            }
            return i;
        }

        private void BsqSort(ArrayInt[] array, int a, int b, int depthLimit, IComparer<ArrayInt> cmp)
        {
            int end = b, length = b - a;

            while (length > 16)
            {
                if (depthLimit == 0)
                {
                    HeapSorting.HeapSort(array, a, end, true, cmp);

                    return;
                }
                double sum = 0.0D;
                bool swapped = false;

                for (int i = a + 1; i < end; i++)
                {

                    if (cmp.Compare(array[i - 1], array[i]) == 1)
                    {
                        Sort.Swap(array, i - 1, i);
                        swapped = true;
                    }

                    sum += array[i - 1];
                }

                if (!swapped)
                {
                    return;
                }

                int p = Partition(array, a, end - 1, (int)(sum / (length - 1)), cmp);
                depthLimit--;
                BsqSort(array, p, end - 1, depthLimit, cmp);

                end = p;
                length = end - a;
            }
            InsertionSort.InsertSort(array, a, end, cmp);
        }

        public void RunSort(ArrayInt[] array, int sortLength, int parameter, IComparer<ArrayInt> cmp)
        {
            BsqSort(array, 0, sortLength, 2 * (int)(Math.Log(sortLength) / Math.Log(2.0D)), cmp);
        }
    }
}
