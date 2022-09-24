using System;
using System.Collections.Generic;
/*
PORTED TO ARRAYV BY PCBOYGAMES
------------------------------
- "QUICKSORT SAYS"  SANMAYCE -
------------------------------
*/
namespace Sorting_algorithm_benchmark_grapher.Sorts
{
    internal class MagneticaQuickSort : ISorter
    {
        private readonly Random random = new();

        private static bool randompivot = false;
        private static bool medianpivot = false;
        private static bool insertion = false;

        public string Title => "Magnetica quicksort";

        public string Message => "Enter variant: 1: Mid Pivot, 2: Mid Pivot + Insrt, 3: Mo3/7 Pivot, 4: Mo3/7 Pivot + Insrt, 5: Random Pivot, 6: Random Pivot + Insrt (Default is 4)";

        public string Category => "Quick sorts";

        public Complexity Time => Complexity.GOOD;

        private void Magnetica<T>(T[] array, int left, int right, IComparer<T> cmpr)
        {
            int threshold = insertion ? 17 : 0;
            int i, j, pl, pr;
            int entries = right - left + 1;
            ArrayInt[] stack = new ArrayInt[entries];
            int stackptr = 2;
            T p;
            int lback = left;
            int rback = right;
            int midmid;
            int gear = 0;
            int cmp;
            stack[1] = left;
            stack[2] = right;
            do
            {
                right = stack[stackptr];
                left = stack[stackptr - 1];
                stackptr -= 2;
                for (; left + threshold < right;)
                {
                    j = right;
                    pl = left;
                    pr = left;
                    // Sorry, Sanmayce, I think I found a better way to do this.
                    // It should also save me from having to write too much code.
                    if (medianpivot || (medianpivot && insertion && right - left > 31))
                    {
                        midmid = left + ((right - left) >> 2);
                        if (gear == 0)
                        {
                            InsertionSort.InsertSort(array, midmid, midmid + 3, cmpr);
                            Sort.Swap(array, midmid + 1, pr);
                        }
                        else
                        {
                            InsertionSort.InsertSort(array, midmid, midmid + 7, cmpr);
                            Sort.Swap(array, midmid + 3, pr);
                        }
                    }
                    else if (randompivot)
                    {
                        Sort.Swap(array, random.Next(right - left) + left, pr);
                    }
                    else
                    {
                        Sort.Swap(array, (left + right) >> 1, pr);
                    }

                    p = array[pr];
                    for (; pr < j;)
                    {
                        pr++;
                        cmp = cmpr.Compare(p, array[pr]);
                        if (cmp > 0)
                        {
                            Sort.Swap(array, pl, pr);
                            pl++;
                        }
                        else if (cmp < 0)
                        {
                            for (; cmpr.Compare(p, array[j]) < 0;)
                            {
                                j--;
                            }

                            if (pr < j)
                            {
                                Sort.Swap(array, pr, j);
                            }

                            j--;
                            pr--;
                        }
                    }
                    j = pl - 1;
                    i = pr + 1;
                    if (insertion)
                    {
                        gear = Math.Max(right - i, j - left) > (Math.Min(right - i, j - left) << 6) ? 1 : 0;
                    }
                    if (i + threshold < right)
                    {
                        stackptr += 2;
                        stack[stackptr - 1] = i;
                        stack[stackptr] = right;
                        if (insertion)
                        {
                            stackptr *= stackptr + 2 <= entries - 1 ? 1 : 0;
                            _ = stackptr + 2 <= entries - 1 ? 1 : 0;
                        }
                    }
                    right = j;
                }
            } while (stackptr != 0);
            if (insertion)
            {
                InsertionSort.InsertSort(array, lback, rback + 1, cmpr);
            }
        }

        public void RunSort<T>(T[] array, int currentLength, int variant, IComparer<T> cmp)
        {
            if (variant is 3 or 4)
            {
                medianpivot = true;
            }

            if (variant is 5 or 6)
            {
                randompivot = true;
            }
            else
            {
                insertion = true;
            }

            Magnetica(array, 0, currentLength - 1, cmp);
        }
    }
}
