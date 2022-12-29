﻿using System.Collections.Generic;

namespace Sorting_algorithm_benchmark_grapher.Sorts
{
    /**
 * Binary MSD Radix Sort / Binary Quicksort.
 *
 * Implemented as recursive decent, and via task queue, see:
 * * binaryQuickSortRecursive, and
 * * binaryQuickSort respectively.
 *
 * Both of which are in-place sorting algorithms, with the recursive utilizing
 * the stack for divide-and-conquer, while the non-recursive utilizes a queue.
 *
 * Can be extended to support unsigned integers, by sorting the first bit rin
 * reverse. Can be made stable at the cost of O(n) memory. Can be parallalized
 * to O(log2(n)) subtasks / threads.
 *
 * @author Skeen
 */
    internal class BinaryQuickSort : IIntegerSorter
    {
        public string Title => "Binary quicksort";

        public string Message => "";

        public Complexity Time => Complexity.GOOD;

        private static int Partition(ArrayInt[] array, int a, int b, int bit)
        {
            int i = a - 1, j = b;

            while (true)
            {
                do
                {
                    i++;
                }
                while (i < j && !Sort.GetBit(array[i], bit));

                do
                {
                    j--;
                }
                while (j > i && Sort.GetBit(array[j], bit));

                if (i < j)
                {
                    Sort.Swap(array, i, j);
                }
                else
                {
                    return i;
                }
            }
        }

        public void RunSort(ArrayInt[] array, int length, int parameter, IComparer<ArrayInt> cmp)
        {
            int q = Sort.AnalyzeBit(array, length), m = 0,
            i = 0, b = length;

            while (i < length)
            {
                int p = b - i < 1 ? i : Partition(array, i, b, q);

                if (q == 0)
                {
                    m += 2;
                    while (!Sort.GetBit(m, q + 1))
                    {
                        q++;
                    }

                    i = b;
                    while (b < length && (array[b] >> (q + 1)) == (m >> (q + 1)))
                    {
                        b++;
                    }
                }
                else
                {
                    b = p;
                    q--;
                }
            }
        }
    }
}
