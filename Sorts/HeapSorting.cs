using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sorting_algorithm_benchmark_grapher.Sorts
{
    internal static class HeapSorting
    {
        public static void SiftDown<T>(T[] array, int root, int dist, int start, bool isMax, IComparer<T> cmp)
        {
            int compareVal;

            if (isMax) compareVal = -1;
            else compareVal = 1;

            while (root <= dist / 2)
            {
                int leaf = 2 * root;
                if (leaf < dist && cmp.Compare(array[start + leaf - 1], array[start + leaf]) == compareVal)
                {
                    leaf++;
                }
                if (cmp.Compare(array[start + root - 1], array[start + leaf - 1]) == compareVal)
                {
                    Sort.Swap(array, start + root - 1, start + leaf - 1);
                    root = leaf;
                }
                else break;
            }
        }

        public static void Heapify<T>(T[] arr, int low, int high, bool isMax, IComparer<T> cmp)
        {
            int length = high - low;
            for (int i = length / 2; i >= 1; i--)
            {
                SiftDown(arr, i, length, low, isMax, cmp);
            }
        }

        // This version of heap sort works for max and min variants, alongside sorting
        // partial ranges of an array.
        public static void HeapSort<T>(T[] arr, int start, int length, bool isMax, IComparer<T> cmp)
        {
            Heapify(arr, start, length, isMax, cmp);

            for (int i = length - start; i > 1; i--)
            {
                Sort.Swap(arr, start, start + i - 1);
                SiftDown(arr, 1, i - 1, start, isMax, cmp);
            }

            if (!isMax)
            {
                Sort.Reversal(arr, start, start + length - 1);
            }
        }
    }
}
