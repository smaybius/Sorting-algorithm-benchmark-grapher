using System.Collections.Generic;
/*
 *
MIT License
Copyright (c) 2021 aphitorite
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
namespace Sorting_algorithm_benchmark_grapher.Sorts
{
    internal class StacklessQuickSort : ISorter
    {
        public string Title => "Stackless quicksort (aphitorite)";

        public string Message => "";

        public string Category => "Quick sorts";

        public Complexity Time => Complexity.GOOD;

        private static void MedianOfThree<T>(T[] array, int a, int b, IComparer<T> cmp)
        {
            int m = a + ((b - 1 - a) / 2);

            if (cmp.Compare(array[a], array[m]) == 1)
            {
                Sort.Swap(array, a, m);
            }

            if (cmp.Compare(array[m], array[b - 1]) == 1)
            {
                Sort.Swap(array, m, b - 1);

                if (cmp.Compare(array[a], array[m]) == 1)
                {
                    return;
                }
            }

            Sort.Swap(array, a, m);
        }

        private static int Partition<T>(T[] array, int a, int b, IComparer<T> cmp)
        {
            int i = a, j = b;

            MedianOfThree(array, a, b, cmp);
            do
            {
                do
                {
                    i++;
                }
                while (i < j && cmp.Compare(array[i], array[a]) < 0);

                do
                {
                    j--;
                }
                while (j >= i && cmp.Compare(array[j], array[a]) >= 0);

                if (i < j)
                {
                    Sort.Swap(array, i, j);
                }
                else
                {
                    Sort.Swap(array, a, j);
                    return j;
                }
            }
            while (true);
        }

        private static int LeftBinSearch<T>(T[] array, int a, int b, int p, IComparer<T> cmp)
        {
            while (a < b)
            {
                int m = a + ((b - a) / 2);

                if (cmp.Compare(array[p], array[m]) <= 0)
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

        private static void QuickSort<T>(T[] array, int a, int b, IComparer<T> cmp)
        {
            T max = array[a];

            for (int i = a + 1; i < b; i++)
            {

                if (cmp.Compare(array[i], max) > 0)
                {
                    max = array[i];
                }
            }
            for (int i = b - 1; i >= 0; i--)
            {
                if (cmp.Compare(array[i], max) == 0)
                {
                    Sort.Swap(array, i, --b);
                }
            }

            int b1 = b;

            do
            {
                while (b1 - a > 2)
                {
                    int p = Partition(array, a, b1, cmp);
                    Sort.Swap(array, p, b);

                    b1 = p;
                }

                if (b1 - a == 2 && cmp.Compare(array[a], array[a + 1]) == 1)
                {
                    Sort.Swap(array, a, a + 1);
                }

                a = b1 + 1;
                if (a >= b)
                {
                    if (a - 1 < b)
                    {
                        Sort.Swap(array, a - 1, b);
                    }

                    return;
                }

                b1 = LeftBinSearch(array, a, b, a - 1, cmp);
                Sort.Swap(array, a - 1, b);

                while (a < b1 && cmp.Compare(array[a - 1], array[a]) == 0)
                {
                    a++;
                }
            }
            while (true);
        }

        public void RunSort<T>(T[] array, int sortLength, int parameter, IComparer<T> cmp)
        {
            QuickSort(array, 0, sortLength, cmp);
        }
    }
}
