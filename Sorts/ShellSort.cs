﻿using System.Collections.Generic;
// Shell sort variant retrieved from:
// https://www.cs.princeton.edu/~rs/talks/shellsort.ps
namespace Sorting_algorithm_benchmark_grapher.Sorts
{
    internal class ShellSort : ISorter
    {
        public string Title => "Shell sort";

        public string Message => "Select a gap configuration (0: original, 1: 2^x + 1, 2: 2^x - 1, 3: 3-smooth, 4: 3^x, 5: Sedg.-Incerpi, 6: Sedgewick, 7: Odd-even Sedg., 8: Gonnet-Baeza-Yates, 9: Tokuda, 10: Ciura, 11: Extd. Ciura) (default: 11)";

        public string Category => "Insertion sorts";

        public Complexity Time => Complexity.GOOD;

        private static readonly int[] OriginalGaps = { 2048, 1024, 512, 256, 128, 64, 32, 16, 8, 4, 2, 1 };
        private static readonly int[] PowTwoPlusOneGaps = { 2049, 1025, 513, 257, 129, 65, 33, 17, 9, 5, 3, 1 };
        private static readonly int[] PowTwoMinusOneGaps = { 4095, 2047, 1023, 511, 255, 127, 63, 31, 15, 7, 3, 1 };
        private static readonly int[] ThreeSmoothGaps = {3888, 3456, 3072, 2916, 2592, 2304, 2187, 2048, 1944, 1728,
                                                  1536, 1458, 1296, 1152, 1024, 972, 864, 768, 729, 648, 576,
                                                  512, 486, 432, 384, 324, 288, 256, 243, 216, 192, 162, 144,
                                                  128, 108, 96, 81, 72, 64, 54, 48, 36, 32, 27, 24, 18, 16, 12,
                                                  9, 8, 6, 4, 3, 2, 1};
        private static readonly int[] PowersOfThreeGaps = { 3280, 1093, 364, 121, 40, 13, 4, 1 };
        private static readonly int[] SedgewickIncerpiGaps = { 1968, 861, 336, 112, 48, 21, 7, 3, 1 };
        private static readonly int[] SedgewickGaps = { 1073, 281, 77, 23, 8, 1 };
        private static readonly int[] OddEvenSedgewickGaps = { 3905, 2161, 929, 505, 209, 109, 41, 19, 5, 1 };
        private static readonly int[] GonnetBaezaYatesGaps = { 1861, 846, 384, 174, 79, 36, 16, 7, 3, 1 };
        private static readonly int[] TokudaGaps = { 2660, 1182, 525, 233, 103, 46, 20, 9, 4, 1 };
        private static readonly int[] CiuraGaps = { 1750, 701, 301, 132, 57, 23, 10, 4, 1 };
        private static readonly int[] ExtendedCiuraGaps = { 8861, 3938, 1750, 701, 301, 132, 57, 23, 10, 4, 1 };

        public static void ShellSorter<T>(T[] array, int length, int[] incs, IComparer<T> cmp)
        {

            for (int k = 0; k < incs.Length; k++)
            {
                if (incs == PowersOfThreeGaps)
                {
                    if (incs[k] < length / 3)
                    {
                        for (int h = incs[k], i = h; i < length; i++)
                        {

                            T v = array[i];
                            int j = i;

                            while (j >= h && cmp.Compare(array[j - h], v) == 1)
                            {
                                array[j] = array[j - h];
                                j -= h;

                            }
                            array[j] = v;
                        }
                    }
                }
                else
                {
                    if (incs[k] < length)
                    {
                        for (int h = incs[k], i = h; i < length; i++)
                        {
                            //ArrayVisualizer.setCurrentGap(incs[k]);

                            T v = array[i];
                            int j = i;

                            while (j >= h && cmp.Compare(array[j - h], v) == 1)
                            {
                                array[j] = array[j - h];
                                j -= h;
                            }
                            array[j] = v;
                        }
                    }
                }
            }
        }
        public void RunSort<T>(T[] array, int sortLength, int parameter, IComparer<T> cmp)
        {
            int[] gaps = parameter switch
            {
                0 => OriginalGaps,
                1 => PowTwoPlusOneGaps,
                2 => PowTwoMinusOneGaps,
                3 => ThreeSmoothGaps,
                4 => PowersOfThreeGaps,
                5 => SedgewickIncerpiGaps,
                6 => SedgewickGaps,
                7 => OddEvenSedgewickGaps,
                8 => GonnetBaezaYatesGaps,
                9 => TokudaGaps,
                10 => CiuraGaps,
                _ => ExtendedCiuraGaps,
            };
            ShellSorter(array, sortLength, gaps, cmp);
        }
    }
}
