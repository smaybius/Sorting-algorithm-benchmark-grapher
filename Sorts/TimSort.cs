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
    internal class StacklessTimSort : ISorter
    {
        public string Title => "Timsort";

        public string Message => "";

        public string Category => "Merge sorts";

        public Complexity Time => Complexity.GOOD;

        public void RunSort<T>(T[] array, int length, int parameter, IComparer<T> cmp)
        {
            TimSorter<T> ts = new(cmp);
            T[] tmp = new T[length / 2];

            int mRun = length;
            for (; mRun >= 32; mRun = (mRun + 1) / 2)
            {
                ;
            }

            ts.BuildRuns(array, 0, length, mRun);

            for (int i, j = mRun; j < length; j *= 2)
            {
                for (i = 0; i + (2 * j) <= length; i += 2 * j)
                {
                    ts.SmartMerge(array, tmp, i, i + j, i + (2 * j));
                }

                if (i + j < length)
                {
                    ts.SmartMerge(array, tmp, i, i + j, length);
                }
            }
        }
    }
}
