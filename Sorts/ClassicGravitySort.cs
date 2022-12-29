using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/*
 *
MIT License
Copyright (c) 2020 aphitorite
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
    internal sealed class ClassicGravitySort : IIntegerSorter
    {
        public string Title => "Gravity sort (classic)";

        public string Message => "";

        public Complexity Time => Complexity.QUADRATIC;

        public void RunSort(ArrayInt[] array, int length, int parameter, IComparer<ArrayInt> cmp)
        {
            int max = Sort.AnalyzeMax(array, length, cmp);
            ArrayInt[] transpose = new ArrayInt[max];

            for (int i = 0; i < length; i++)
            {
                int num = array[i];
                for (int j = 0; j < num; j++)
                {
                    transpose[ j] =  transpose[j] + 1;
                }
            }

            for (int i = 0; i < length; i++)
            {
                int sum = 0;
                for (int j = 0; j < max; j++)
                {
                    if (transpose[j] > 0)
                    {
                        sum++;
                    }
                }
                array[ length - i - 1] =  sum;
                for (int j = 0; j < max; j++)
                {
                    transpose[ j] =  transpose[j] - 1;
                }
            }
        }
    }
}
