using System.Collections.Generic;

namespace Sorting_algorithm_benchmark_grapher.Sorts
{

    /*
 *
The MIT License (MIT)
Copyright (c) 2020 aphitorite
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
    internal class RotateMergeSort : ISorter
    {
        public string Title => "Rotate merge sort";

        public string Message => "";

        public string Category => "Merge sorts";

        public Complexity Time => Complexity.GOOD;

        private static void MultiSwap<T>(T[] array, int a, int b, int len)
        {
            for (int i = 0; i < len; i++)
            {
                Sort.Swap(array, a + i, b + i);
            }
        }

        private static void Rotate<T>(T[] array, int a, int m, int b)
        {
            int l = m - a, r = b - m;

            while (l > 0 && r > 0)
            {
                if (r < l)
                {
                    MultiSwap(array, m - r, m, r);
                    b -= r;
                    m -= r;
                    l -= r;
                }
                else
                {
                    MultiSwap(array, a, m, l);
                    a += l;
                    m += l;
                    r -= l;
                }
            }
        }

        private static int BinarySearch<T>(T[] array, int a, int b, T value, bool left, IComparer<T> cmp)
        {
            while (a < b)
            {
                int m = a + ((b - a) / 2);

                bool comp = left ? cmp.Compare(value, array[m]) <= 0
                                    : cmp.Compare(value, array[m]) < 0;

                if (comp)
                {
                    b = m;
                }
                else
                {
                    a = m + 1;
                }
            }

            return a;
        }

        private void RotateMerge<T>(T[] array, int a, int m, int b, IComparer<T> cmp)
        {
            int m1, m2, m3;

            if (m - a >= b - m)
            {
                m1 = a + ((m - a) / 2);
                m2 = BinarySearch(array, m, b, array[m1], true, cmp);
                m3 = m1 + (m2 - m);
            }
            else
            {
                m2 = m + ((b - m) / 2);
                m1 = BinarySearch(array, a, m, array[m2], false, cmp);
                m3 = m2++ - (m - m1);
            }
            Rotate(array, m1, m, m2);

            if (m2 - (m3 + 1) > 0 && b - m2 > 0)
            {
                RotateMerge(array, m3 + 1, m2, b, cmp);
            }

            if (m1 - a > 0 && m3 - m1 > 0)
            {
                RotateMerge(array, a, m1, m3, cmp);
            }
        }

        private void RotateMergeSorter<T>(T[] array, int a, int b, IComparer<T> cmp)
        {
            int len = b - a, i;

            for (int j = 1; j < len; j *= 2)
            {
                for (i = a; i + (2 * j) <= b; i += 2 * j)
                {
                    RotateMerge(array, i, i + j, i + (2 * j), cmp);
                }

                if (i + j < b)
                {
                    RotateMerge(array, i, i + j, b, cmp);
                }
            }
        }

        public void RunSort<T>(T[] array, int length, int parameter, IComparer<T> cmp)
        {
            RotateMergeSorter(array, 0, length, cmp);
        }
    }
}
