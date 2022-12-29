using System;
using System.Collections.Generic;
using System.Numerics;

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

        public static void Pull<T>(T[] array, int pos, int to)
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
            {
                return;
            }

            int a = pos;
            int b = pos + lenA - 1;
            int c = pos + lenA;
            int d = pos + lenA + lenB - 1;
            T swap;

            while (a < b && c < d)
            {
                swap = array[b];
                array[b--] = array[a];
                array[a++] = array[c];
                array[c++] = array[d];
                array[d--] = swap;
            }
            while (a < b)
            {
                swap = array[b];
                array[b--] = array[a];
                array[a++] = array[d];
                array[d--] = swap;
            }
            while (c < d)
            {
                swap = array[c];
                array[c++] = array[d];
                array[d--] = array[a];
                array[a++] = swap;
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


                if (cmp.Compare(val, max) > 0)
                {
                    max = val;
                }
            }

            return max;
        }

        public static T AnalyzeMin<T>(T[] array, int length, IComparer<T> cmp)
        {

            T min = array[0];

            for (int i = 0; i < length; i++)
            {

                T val = array[i];

                if (cmp.Compare(val, min) < 0)
                {
                    min = val;
                }
            }

            return min;
        }
        public static int GetDigit(int a, int power, int radix)
        {

            int digit;
            digit = (int)(a / Math.Pow(radix, power)) % radix;
            return digit;
        }
        public static void FancyTranscribe(ArrayInt[] array, int length, List<int>[] registers)
        {
            ArrayInt[] tempArray = new ArrayInt[length];
            bool[] tempWrite = new bool[length];
            int radix = registers.Length;

            Transcribe(tempArray, registers, 0);

            for (int i = 0; i < length; i++)
            {
                int register = i % radix;
                int pos = (register * (length / radix)) + (i / radix);

                if (!tempWrite[pos])
                {
                    array[pos] = tempArray[pos];
                    tempWrite[pos] = true;
                }
            }
            for (int i = 0; i < length; i++)
            {
                if (!tempWrite[i])
                {
                    array[i] = tempArray[i];
                }
            }
        }

        public static void Transcribe(ArrayInt[] array, List<int>[] registers, int start)
        {
            int total = start;

            for (int index = 0; index < registers.Length; index++)
            {
                for (int i = 0; i < registers[index].Count; i++)
                {
                    array[total++] = registers[index][i];
                }
                registers[index].Clear();
            }
        }
        public static int AnalyzeMaxLog(ArrayInt[] array, int length, int bse)
        {

            int max = 0;

            for (int i = 0; i < length; i++)
            {
                int val = array[i];

                if (val > max)
                {
                    max = val;
                }
            }

            return (int)(Math.Log(max) / Math.Log(bse));
        }
        public static T[] CopyOf<T>(T[] original, int newLength) //copied from https://github.com/openjdk/jdk/blob/master/src/java.base/share/classes/java/util/Arrays.java
        {
            T[] copy = new T[newLength];
            Array.Copy(original, 0, copy, 0,
                             Math.Min(original.Length, newLength));
            return copy;
        }

        public static T[] CopyOfRange<T>(T[] original, int from, int to) //copied from https://github.com/openjdk/jdk/blob/master/src/java.base/share/classes/java/util/Arrays.java
        {
            int newLength = to - from;
            T[] copy = new T[newLength];
            Array.Copy(original, from, copy, 0,
                             Math.Min(original.Length - from, newLength));
            return copy;
        }

        public static bool GetBit(int n, int k)
        {
            // Find boolean value of bit k in n
            return ((n >> k) & 1) == 1;
        }

        public static int AnalyzeBit(ArrayInt[] array, int length)
        {

            // Find highest bit of highest value
            int max = 0;

            for (int i = 0; i < length; i++)
            {

                int val = array[i];

                if (val > max) max = val;
            }

            int analysis;



            analysis = 31 - BitOperations.LeadingZeroCount((uint)max);
            return analysis;
        }
    }
}
