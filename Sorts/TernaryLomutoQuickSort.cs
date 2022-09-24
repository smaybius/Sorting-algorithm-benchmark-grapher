using System.Collections.Generic;

namespace Sorting_algorithm_benchmark_grapher.Sorts
{
    internal class PivotPair
    {
        public int first, second;

        public PivotPair(int first, int second)
        {
            this.first = first;
            this.second = second;
        }
    }
    internal class TernaryLomutoQuickSort : ISorter
    {
        public string Title => "Ternary Lomuto quick sort";

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

        private static PivotPair PartitionTernaryLL<T>(T[] A, int lo, int hi, IComparer<T> cmpr)
        {
            int p = SelectPivot(A, lo, hi, cmpr);

            T pivot = A[p];
            Sort.Swap(A, p, hi - 1);

            int i = lo, k = hi - 1;

            for (int j1 = lo; j1 < k; ++j1)
            {
                int cmp = cmpr.Compare(A[j1], pivot);
                if (cmp == 0)
                {
                    Sort.Swap(A, --k, j1);
                    --j1;
                }
                else if (cmp < 0)
                {
                    Sort.Swap(A, i++, j1);
                }
            }

            int j = i + (hi - k);

            for (int s = 0; s < hi - k; ++s)
            {
                Sort.Swap(A, i + s, hi - 1 - s);
            }

            return new(i, j);
        }

        private static void QuickSortTernaryLL<T>(T[] A, int lo, int hi, IComparer<T> cmp)
        {
            if (lo + 1 < hi)
            {
                PivotPair mid = PartitionTernaryLL(A, lo, hi, cmp);

                QuickSortTernaryLL(A, lo, mid.first, cmp);
                QuickSortTernaryLL(A, mid.second, hi, cmp);
            }
        }

        public void RunSort<T>(T[] array, int currentLength, int parameter, IComparer<T> cmp)
        {
            QuickSortTernaryLL(array, 0, currentLength, cmp);
        }
    }
}
