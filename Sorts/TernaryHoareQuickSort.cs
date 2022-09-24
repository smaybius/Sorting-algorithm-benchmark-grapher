using System;
using System.Collections.Generic;

namespace Sorting_algorithm_benchmark_grapher.Sorts
{
    internal class TernaryHoareQuickSort : ISorter
    {
        public string Title => "Ternary Hoare quicksort";

        public string Message => "";

        public string Category => "Quick sorts";

        public Complexity Time => Complexity.GOOD;

        private static int Compare<T>(T[] A, int lo, int hi, IComparer<T> cmp)
        {
            return cmp.Compare(A[lo], A[hi]);

        }
        // I'll just be using median-of-3 here
        private static int SelectPivot<T>(T[] A, int lo, int hi, IComparer<T> cmp)
        {
            int mid = (lo + hi) / 2;

            return Compare(A, lo, mid, cmp) == 0
                ? lo
                : Compare(A, lo, hi - 1, cmp) == 0 || Compare(A, mid, hi - 1, cmp) == 0
                ? hi - 1
                : Compare(A, lo, mid, cmp) < 0
                ? (Compare(A, mid, hi - 1, cmp) < 0 ? mid : (Compare(A, lo, hi - 1, cmp) < 0 ? hi - 1 : lo))
                : (Compare(A, mid, hi - 1, cmp) > 0 ? mid : (Compare(A, lo, hi - 1, cmp) < 0 ? lo : hi - 1));
        }

        private void QuickSortTernaryLR<T>(T[] A, int lo, int hi, IComparer<T> cmpr)
        {
            if (hi <= lo)
            {
                return;
            }

            int cmp;

            int piv = SelectPivot(A, lo, hi + 1, cmpr);
            Sort.Swap(A, piv, hi);

            T pivot = A[hi];

            int i = lo, j = hi - 1;
            int p = lo, q = hi - 1;

            for (; ; )
            {
                while (i <= j && (cmp = cmpr.Compare(A[i], pivot)) <= 0)
                {
                    if (cmp == 0)
                    {
                        Sort.Swap(A, i, p++);
                    }
                    ++i;
                }

                while (i <= j && (cmp = cmpr.Compare(A[j], pivot)) >= 0)
                {
                    if (cmp == 0)
                    {
                        Sort.Swap(A, j, q--);
                    }
                    --j;
                }

                if (i > j)
                {
                    break;
                }

                Sort.Swap(A, i++, j--);
            }

            Sort.Swap(A, i, hi);

            int num_less = i - p;
            int num_greater = q - j;

            j = i - 1; i++;

            int pe = lo + Math.Min(p - lo, num_less);
            for (int k = lo; k < pe; k++, j--)
            {
                Sort.Swap(A, k, j);
            }

            int qe = hi - 1 - Math.Min(hi - 1 - q, num_greater - 1);
            for (int k = hi - 1; k > qe; k--, i++)
            {
                Sort.Swap(A, i, k);
            }

            QuickSortTernaryLR(A, lo, lo + num_less - 1, cmpr);
            QuickSortTernaryLR(A, hi - num_greater + 1, hi, cmpr);
        }
        public void RunSort<T>(T[] array, int currentLength, int parameter, IComparer<T> cmp)
        {
            QuickSortTernaryLR(array, 0, currentLength - 1, cmp);
        }
    }
}
