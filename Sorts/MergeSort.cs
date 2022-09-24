using System;
using System.Collections.Generic;

namespace Sorting_algorithm_benchmark_grapher.Sorts
{
    internal class MergeSort : ISorter
    {
        public string Title => "Merge sort";

        public string Message => "";

        public string Category => "Merge sorts";

        public Complexity Time => Complexity.GOOD;

        private int copyLength;
        private void merge<T>(T[] array, T[] scratchArray, int currentLength, int index, int mergeSize, IComparer<T> cmp)
        {
            int left = index;
            int mid = left + (mergeSize / 2);
            int right = mid;
            int end = Math.Min(currentLength, index + mergeSize);

            int scratchIndex = left;

            if (right < end)
            {
                while (left < mid && right < end)
                {

                    scratchArray[scratchIndex++] = cmp.Compare(array[left], array[right]) <= 0 ? array[left++] : array[right++];
                }
                if (left < mid)
                {
                    while (left < mid)
                    {
                        scratchArray[scratchIndex++] = array[left++];
                    }
                }
                if (right < end)
                {
                    while (right < end)
                    {
                        scratchArray[scratchIndex++] = array[right++];
                    }
                }
            }
            else
            {
                copyLength = left;
            }
        }

        public void RunSort<T>(T[] array, int currentLength, int parameter, IComparer<T> cmp)
        {
            T[] scratchArray = new T[currentLength];
            int mergeSize = 2;

            while (mergeSize <= currentLength)
            {
                copyLength = currentLength;

                for (int i = 0; i < currentLength; i += mergeSize)
                {
                    merge(array, scratchArray, currentLength, i, mergeSize, cmp);
                }

                for (int i = 0; i < copyLength; i++)
                {
                    array[i] = scratchArray[i];
                }

                mergeSize *= 2;
            }
            if ((mergeSize / 2) != currentLength)
            {
                merge(array, scratchArray, currentLength, 0, mergeSize, cmp);

                for (int i = 0; i < currentLength; i++)
                {
                    array[i] = scratchArray[i];
                }
            }
        }
    }
}
