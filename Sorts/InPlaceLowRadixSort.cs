using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/*
 *
MIT License
Copyright (c) 2019 w0rthy
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
    internal sealed class InPlaceLowRadixSort : IIntegerSorter
    {
        public string Title => "In-place low digit radix sort";

        public string Message => "Enter the base (default: 10)";

        public Complexity Time => Complexity.QUADRATIC;

        public void RunSort(ArrayInt[] array, int sortLength, int bucketCount, IComparer<ArrayInt> cmp)
        {
            int pos = 0;
            ArrayInt[] vregs = new ArrayInt[bucketCount - 1];

            int maxpower = Sort.AnalyzeMaxLog(array, sortLength, bucketCount);

            for (int p = 0; p <= maxpower; p++)
            {
                for (int i = 0; i < vregs.Length; i++)
                {
                    vregs[ i] =  sortLength - 1;
                }

                pos = 0;

                for (int i = 0; i < sortLength; i++)
                {
                    int digit = Sort.GetDigit(array[pos], p, bucketCount);

                    if (digit == 0)
                    {
                        pos++;
                    }
                    else
                    {

                        Sort.Pull(array, pos, vregs[digit - 1]);

                        for (int j = digit - 1; j > 0; j--)
                        {
                            vregs[ j - 1] =  vregs[j - 1] - 1;
                        }
                    }
                }
            }
        }
    }
}
