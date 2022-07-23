using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;

namespace Sorting_algorithm_benchmark_grapher
{
    // library class for sort features and functions, particularly if they're ported over from ArrayV or something

    public enum Complexity
    {
        GOOD,
        QUADRATIC,
        CUBIC,
        EXPONENTIAL,
        STUPID
    }
    public static class Sort
    {
        public static int CompareIndices<T>(T[] array, int a, int b, IComparer<T> cmp)
        {
            return cmp.Compare(array[a], array[b]);
        }

        public static void Swap<T>(T[] array, int a, int b)
        {

            (array[b], array[a]) = (array[a], array[b]);

        }

        public static void MultiSwap<T>(T[] array, int pos, int to)
        {
            if (to - pos > 0)
            {
                for (int i = pos; i < to; i++)
                {
                    Swap(array, i, i + 1);
                }
            }
            else
            {
                for (int i = pos; i > to; i--)
                {
                    Swap(array, i, i - 1);
                }
            }
        }

        public static void Write<T>(T[] array, int at, T equals)
        {
            array[at] = equals;
        }

        public static void Reversal<T>(T[] array, int start, int length)
        {

            for (int i = start; i < start + ((length - start + 1) / 2); i++)
            {
                Swap(array, i, start + length - i);
            }
        }

        public static void IndexedRotate<T>(T[] array, int start, int mid, int end)
        {
            Rotate(array, start, mid - start, end - mid);
        }

        public static void BlockSwapBackwards<T>(T[] array, int a, int b, int len)
        {
            for (int i = 0; i < len; i++)
            {
                Swap(array, a + len - i - 1, b + len - i - 1);
            }
        }

        public static void BlockSwap<T>(T[] array, int a, int b, int len)
        {
            for (int i = 0; i < len; i++)
            {
                Swap(array, a + i, b + i);
            }
        }

        // by Scandum and Control
        public static void Rotate<T>(T[] array, int pos, int lenA, int lenB)
        {
            if (lenA < 1 || lenB < 1)
                return;

            int a = pos;
            int b = pos + lenA - 1;
            int c = pos + lenA;
            int d = pos + lenA + lenB - 1;
            T swap;

            while (a < b && c < d)
            {
                swap = array[b];
                Write(array, b--, array[a]);
                Write(array, a++, array[c]);
                Write(array, c++, array[d]);
                Write(array, d--, swap);
            }
            while (a < b)
            {
                swap = array[b];
                Write(array, b--, array[a]);
                Write(array, a++, array[d]);
                Write(array, d--, swap);
            }
            while (c < d)
            {
                swap = array[c];
                Write(array, c++, array[d]);
                Write(array, d--, array[a]);
                Write(array, a++, swap);
            }
            if (a < d)
            { // dont count reversals that dont do anything
                Reversal(array, a, d);
            }
        }

        public static T AnalyzeMax<T>(T[] array, int length, IComparer<T> cmp)
        {

            T max = array[0];

            for (int i = 0; i < length; i++)
            {

                T val = array[i];


                if (cmp.Compare(val, max) > 0) max = val;
            }

            return max;
        }

        public static T AnalyzeMin<T>(T[] array, int length, IComparer<T> cmp)
        {

            T min = array[0];

            for (int i = 0; i < length; i++)
            {

                T val = array[i];

                if (cmp.Compare(val, min) < 0) min = val;
            }

            return min;
        }
    }
}
