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
    internal sealed class GravitySort : IIntegerSorter
    {
        public string Title => "Gravity sort";

        public string Message => "";

        public Complexity Time => Complexity.QUADRATIC;

        public void RunSort(ArrayInt[] array, int length, int parameter, IComparer<ArrayInt> cmp)
        {
            int min = array[0], max = array[0];

            for (int i = 1; i < length; i++)
            {
                if (array[i] < min) min = array[i];
                if (array[i] > max) max = array[i];
            }

            ArrayInt[] x = new ArrayInt[length];
            ArrayInt[] y = new ArrayInt[max - min + 1];

            //save a copy of array-min in x
            //increase count of the array-min value in y
            for (int i = 0; i < length; i++)
            {
                x[ i] =  array[i] - min;
                y[ array[i] - min] =  y[array[i] - min] + 1;
            }

            //do a partial sum backwards to determine how many elements are greater than a value
            for (int i = y.Length - 1; i > 0; i--)
                y[ i - 1] =  y[i - 1] += y[i];

            //iterate for every integer value in the array range
            for (int j = y.Length - 1; j >= 0; j--)
            {

                //iterate for every item in array and x
                for (int i = 0; i < length; i++)
                {

                    int inc = (i >= length - y[j] ? 1 : 0) - (x[i] >= j ? 1 : 0);

                    //update the main array
                    array[ i] =  array[i] + inc;
                }
            }
        }
    }
}
