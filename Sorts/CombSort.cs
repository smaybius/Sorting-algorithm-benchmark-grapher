using System;
using System.Collections.Generic;
/*
 *
The MIT License (MIT)
Copyright (c) 2012 Daniel Imms, http://www.growingwiththeweb.com
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
namespace Sorting_algorithm_benchmark_grapher.Sorts
{
    internal class CombSort : ISorter
    {
        public string Title => "Comb sort";

        public string Message => "Enter shrink factor (input divided by 100) (default: 130)";

        public string Category => "Exchange sorts";

        public Complexity Time => Complexity.GOOD;

        public void RunSort<T>(T[] array, int length, int parameter, IComparer<T> cmp)
        {
            if (parameter < 110)
            {
                parameter = 130;
            }

            double shrink = parameter / 100d;
            bool swapped = false;
            int gap = length;

            while ((gap > 1) || swapped)
            {

                if (gap > 1)
                {
                    gap = (int)(gap / shrink);
                    //ArrayVisualizer.setCurrentGap(gap);
                }

                swapped = false;

                for (int i = 0; (gap + i) < length; ++i)
                {
                    if (gap <= Math.Min(8, length * 0.03125))
                    {
                        gap = 0;

                        InsertionSort.InsertSort(array, 0, length, cmp);
                        break;
                    }
                    if (cmp.Compare(array[i], array[i + gap]) == 1)
                    {
                        Sort.Swap(array, i, i + gap);
                        swapped = true;
                    }
                }
            }
        }
    }
}
