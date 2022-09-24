using System;
using System.Collections.Generic;

namespace Sorting_algorithm_benchmark_grapher.Sorts
{

    /*
     *
    The MIT License (MIT)
    Copyright (c) 2021 aphitorite
    Permission is hereby granted, free of charge, to any person obtaining a copy of
    this software and associated documentation files (the "Software"), to deal in
    the Software without restriction, including without limitation the rights to
    use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of
    the Software, and to permit persons to whom the Software is furnished to do so,
    subject to the following conditions:
    The above copyright notice and this permission notice shall be included in all
    copies or substantial portions of the Software.
    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
    FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
    COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
    IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
    CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
     *
     */
    internal class StacklessRotateMerge //: ISorter // TODO: Index out of range in rotation
    {
        public string Title => "Rotate merge (stackless)";

        public string Message => "";

        public string Category => "Merge sorts";

        public Complexity Time => Complexity.GOOD;

        //@param c - select c smallest elements
        private static void PartitionMerge<T>(T[] array, int a, int m, int b, int c, IComparer<T> cmp)
        {
            int lenA = m - a, lenB = b - m;

            if (lenA < 1 || lenB < 1)
            {
                return;
            }

            if (lenB < lenA)
            {
                c = lenA + lenB - c;
                int r1 = 0, r2 = Math.Min(c, lenB);

                while (r1 < r2)
                {
                    int ml = (r1 + r2) / 2;

                    if (cmp.Compare(array[m - (c - ml)], array[b - ml - 1]) > 0)
                    {
                        r2 = ml;
                    }
                    else
                    {
                        r1 = ml + 1;
                    }
                }
                //[lenA-(c-r1)][c-r1][lenB-r1][r1]
                //[lenA-(c-r1)][lenB-r1][c-r1][r1]
                Sort.Rotate(array, m - (c - r1), m, b - r1);
            }
            else
            {
                int r1 = 0, r2 = Math.Min(c, lenA);

                while (r1 < r2)
                {
                    int ml = (r1 + r2) / 2;

                    if (cmp.Compare(array[a + ml], array[m + (c - ml) - 1]) > 0)
                    {
                        r2 = ml;
                    }
                    else
                    {
                        r1 = ml + 1;
                    }
                }
                //[r1][lenA-r1][c-r1][lenB-(c-r1)]
                //[r1][c-r1][lenA-r1][lenB-(c-r1)]
                Sort.Rotate(array, a + r1, m, m + (c - r1));
            }
        }

        private static void RotateMerge<T>(T[] array, int a, int b, int c, IComparer<T> cmp)
        {
            int i;
            for (i = a + 1; i < b && cmp.Compare(array[i - 1], array[i]) <= 0; i++)
            {
                ;
            }

            if (i < b)
            {
                PartitionMerge(array, a, i, b, c, cmp);
            }
        }

        private static void RotatePartitionMergeSort<T>(T[] array, int a, int b, IComparer<T> cmp)
        {
            int len = b - a;

            for (int i = a + 1; i < b; i += 2)
            {
                if (cmp.Compare(array[i - 1], array[i]) > 0)
                {
                    Sort.Swap(array, i - 1, i);
                }
            }

            for (int j = 2; j < len; j *= 2)
            {
                int b1 = 0;

                for (int i = a; i + j < b; i += 2 * j)
                {
                    b1 = Math.Min(i + (2 * j), b);
                    PartitionMerge(array, i, i + j, b1, j, cmp);
                }

                for (int k = j / 2; k > 1; k /= 2)
                {
                    for (int i = a; i + k < b1; i += 2 * k)
                    {
                        RotateMerge(array, i, Math.Min(i + (2 * k), b), k, cmp);
                    }
                }

                for (int i = a + 1; i < b1; i += 2)
                {
                    if (cmp.Compare(array[i - 1], array[i]) > 0)
                    {
                        Sort.Swap(array, i - 1, i);
                    }
                }
            }
        }
        public void RunSort<T>(T[] array, int length, int parameter, IComparer<T> cmp)
        {
            RotatePartitionMergeSort(array, 0, length, cmp);
        }
    }
}
