using System;
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
    internal class TimSorter<T>
    {
        private readonly IComparer<T> cmp;
        public TimSorter(IComparer<T> cmp)
        {
            this.cmp = cmp;
        }
        private const int M = 7;

        private int LeftBinSearch(T[] array, int a, int b, T val)
        {
            while (a < b)
            {
                int m = a + ((b - a) / 2);

                if (cmp.Compare(val, array[m]) <= 0)
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

        private int RightBinSearch(T[] array, int a, int b, T val)
        {
            while (a < b)
            {
                int m = a + ((b - a) / 2);

                if (cmp.Compare(val, array[m]) < 0)
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

        private int LeftExpSearch(T[] array, int a, int b, T val)
        {
            int i = 1;
            while (a - 1 + i < b && cmp.Compare(val, array[a - 1 + i]) > 0)
            {
                i *= 2;
            }

            return LeftBinSearch(array, a + (i / 2), Math.Min(b, a - 1 + i), val);
        }

        private int RightExpSearch(T[] array, int a, int b, T val)
        {
            int i = 1;
            while (b - i >= a && cmp.Compare(val, array[b - i]) < 0)
            {
                i *= 2;
            }

            return RightBinSearch(array, Math.Max(a, b - i + 1), b - (i / 2), val);
        }

        private int LeftBoundSearch(T[] array, int a, int b, T val)
        {
            int i = 1;
            while (a - 1 + i < b && cmp.Compare(val, array[a - 1 + i]) >= 0)
            {
                i *= 2;
            }

            return RightBinSearch(array, a + (i / 2), Math.Min(b, a - 1 + i), val);
        }

        private int RightBoundSearch(T[] array, int a, int b, T val)
        {
            int i = 1;
            while (b - i >= a && cmp.Compare(val, array[b - i]) <= 0)
            {
                i *= 2;
            }

            return LeftBinSearch(array, Math.Max(a, b - i + 1), b - (i / 2), val);
        }

        private static void InsertTo(T[] array, int a, int b)
        {

            if (a > b)
            {
                T temp = array[a];

                do
                {
                    array[a] = array[--a];
                }
                while (a > b);

                array[b] = temp;
            }
        }

        public void BuildRuns(T[] array, int a, int b, int mRun)
        {
            int i = a + 1, j = a;

            while (i < b)
            {
                if (cmp.Compare(array[i - 1], array[i++]) == 1)
                {
                    while (i < b && cmp.Compare(array[i - 1], array[i]) == 1)
                    {
                        i++;
                    }

                    Sort.Reversal(array, j, i - 1);
                }
                else
                {
                    while (i < b && cmp.Compare(array[i - 1], array[i]) <= 0)
                    {
                        i++;
                    }
                }

                if (i < b)
                {
                    j = i - ((i - j - 1) % mRun) - 1;
                }

                while (i - j < mRun && i < b)
                {
                    TimSorter<T>.InsertTo(array, i, RightBinSearch(array, j, i, array[i]));
                    i++;
                }
                j = i++;
            }
        }

        //galloping mode code refactored from TimSorting.java
        private void MergeFW(T[] array, T[] tmp, int a, int m, int b)
        {
            int len1 = m - a;
            Array.Copy(array, a, tmp, 0, len1);

            int i = 0, mGallop = M, l = 0, r = 0;

            while (true)
            {
                do
                {
                    if (cmp.Compare(tmp[i], array[m]) <= 0)
                    {
                        array[a++] = tmp[i++];
                        l++;
                        r = 0;

                        if (i == len1)
                        {
                            return;
                        }
                    }
                    else
                    {
                        array[a++] = array[m++];
                        r++;
                        l = 0;

                        if (m == b)
                        {
                            while (i < len1)
                            {
                                array[a++] = tmp[i++];
                            }

                            return;
                        }
                    }
                }
                while ((l | r) < mGallop);

                do
                {
                    l = LeftExpSearch(array, m, b, tmp[i]) - m;

                    for (int j = 0; j < l; j++)
                    {
                        array[a++] = array[m++];
                    }

                    array[a++] = tmp[i++];

                    if (i == len1)
                    {
                        return;
                    }
                    else if (m == b)
                    {
                        while (i < len1)
                        {
                            array[a++] = tmp[i++];
                        }

                        return;
                    }

                    r = LeftBoundSearch(tmp, i, len1, array[m]) - i;

                    for (int j = 0; j < r; j++)
                    {
                        array[a++] = tmp[i++];
                    }

                    array[a++] = array[m++];

                    if (i == len1)
                    {
                        return;
                    }
                    else if (m == b)
                    {
                        while (i < len1)
                        {
                            array[a++] = tmp[i++];
                        }

                        return;
                    }

                    mGallop--;
                }
                while ((l | r) >= M);

                if (mGallop < 0)
                {
                    mGallop = 0;
                }

                mGallop += 2;
            }
        }
        private void MergeBW(T[] array, T[] tmp, int a, int m, int b)
        {
            int len2 = b - m;
            Array.Copy(array, m, tmp, 0, len2);

            int i = len2 - 1, mGallop = M, l = 0, r = 0;
            m--;

            while (true)
            {
                do
                {
                    if (cmp.Compare(tmp[i], array[m]) >= 0)
                    {
                        array[--b] = tmp[i--];
                        l++;
                        r = 0;

                        if (i < 0)
                        {
                            return;
                        }
                    }
                    else
                    {
                        array[--b] = array[m--];
                        r++;
                        l = 0;

                        if (m < a)
                        {
                            while (i >= 0)
                            {
                                array[--b] = tmp[i--];
                            }

                            return;
                        }
                    }
                }
                while ((l | r) < mGallop);

                do
                {
                    l = m + 1 - RightExpSearch(array, a, m + 1, tmp[i]);

                    for (int j = 0; j < l; j++)
                    {
                        array[--b] = array[m--];
                    }

                    array[--b] = tmp[i--];

                    if (i < 0)
                    {
                        return;
                    }
                    else if (m < a)
                    {
                        while (i >= 0)
                        {
                            array[--b] = tmp[i--];
                        }

                        return;
                    }

                    r = i + 1 - RightBoundSearch(tmp, 0, i + 1, array[m]);

                    for (int j = 0; j < r; j++)
                    {
                        array[--b] = tmp[i--];
                    }

                    array[--b] = array[m--];

                    if (i < 0)
                    {
                        return;
                    }
                    else if (m < a)
                    {
                        while (i >= 0)
                        {
                            array[--b] = tmp[i--];
                        }

                        return;
                    }
                }
                while ((l | r) >= M);

                if (mGallop < 0)
                {
                    mGallop = 0;
                }

                mGallop += 2;
            }
        }

        public void SmartMerge(T[] array, T[] tmp, int a, int m, int b)
        {
            if (cmp.Compare(array[m - 1], array[m]) <= 0)
            {
                return;
            }

            a = LeftBoundSearch(array, a, m, array[m]);
            b = RightBoundSearch(array, m, b, array[m - 1]);

            if (b - m < m - a)
            {
                MergeBW(array, tmp, a, m, b);
            }
            else
            {
                MergeFW(array, tmp, a, m, b);
            }
        }
    }
}
