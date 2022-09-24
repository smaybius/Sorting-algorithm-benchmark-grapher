using System.Collections.Generic;

namespace Sorting_algorithm_benchmark_grapher.Sorts
{
    /*
 *
MIT License
Copyright (c) 2017 Rodney Shaghoulian
Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:
The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.
THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
 *
 */
    internal class StableQuickSort : ISorter
    {
        public string Title => "Stable quicksort";

        public string Message => "Select pivot position (0: start, 1: middle, 2: end) (default: 1)";

        public string Category => "Quick sorts";

        public Complexity Time => Complexity.GOOD;

        private static int pivpos;

        private static void Copy<T>(List<T> list, T[] array, int startIndex)
        {
            foreach (T num in list)
            {
                array[startIndex++] = num;
            }
        }

        private int length;

        /* Partition/Quicksort "Stable Sort" version using O(n) space */
        private int StablePartition<T>(T[] array, int start, int end, IComparer<T> cmp)
        {
            int pivot = pivpos switch
            {
                0 => start,
                2 => end,
                _ => start + ((end - start) / 2),
            };
            T pivotValue = array[pivot];
            List<T> leftList = new(length);
            List<T> rightList = new(length);

            for (int i = start + 1; i <= end; i++)
            {

                if (cmp.Compare(array[i], pivotValue) == -1)
                {
                    // Writes.mockWrite(end - start, leftList.size(), array[i], 0.25);
                    // Writes.arrayListAdd(leftList, array[i]);
                    leftList.Add(array[i]);
                }
                else
                {
                    // Writes.mockWrite(end - start, rightList.size(), array[i], 0.25);
                    // Writes.arrayListAdd(rightList, array[i]);
                    rightList.Add(array[i]);
                }
            }

            /* Recreate array */
            Copy(leftList, array, start);

            int newPivotIndex = start + leftList.Count;

            array[newPivotIndex] = pivotValue;

            Copy(rightList, array, newPivotIndex + 1);

            return newPivotIndex;
        }
        private void StableQuick<T>(T[] array, int start, int end, IComparer<T> cmp)
        {
            if (start < end)
            {
                int pivotIndex = StablePartition(array, start, end, cmp);
                StableQuick(array, start, pivotIndex - 1, cmp);
                StableQuick(array, pivotIndex + 1, end, cmp);
            }
        }
        public void RunSort<T>(T[] array, int sortLength, int parameter, IComparer<T> cmp)
        {
            length = sortLength;
            pivpos = parameter;
            StableQuick(array, 0, length - 1, cmp);
        }
    }
}
