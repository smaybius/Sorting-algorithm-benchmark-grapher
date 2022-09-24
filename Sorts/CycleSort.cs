using System.Collections.Generic;

namespace Sorting_algorithm_benchmark_grapher.Sorts
{
    internal class CycleSort : ISorter
    {
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
        public string Title => "Cycle sort";

        public string Message => "";

        public string Category => "Selection sorts";

        public Complexity Time => Complexity.QUADRATIC;
        private static int CountLesser<T>(T[] array, int a, int b, T t, IComparer<T> cmp)
        {
            int r = a;

            for (int i = a + 1; i < b; i++)
            {

                r += cmp.Compare(array[i], t) < 0 ? 1 : 0;
            }
            return r;
        }
        public void RunSort<T>(T[] array, int length, int parameter, IComparer<T> cmp)
        {
            for (int i = 0; i < length - 1; i++)
            {

                T t = array[i];
                int r = CountLesser(array, i, length, t, cmp);

                if (r != i)
                {
                    do
                    {
                        while (cmp.Compare(array[r], t) == 0)
                        {
                            r++;
                        }

                        (t, array[r]) = (array[r], t);
                        r = CountLesser(array, i, length, t, cmp);
                    }
                    while (r != i);

                    array[i] = t;
                }
            }
        }
    }
}
