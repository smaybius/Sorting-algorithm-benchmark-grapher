using System.Collections.Generic;

namespace Sorting_algorithm_benchmark_grapher.Sorts
{
    internal class StableSort<T>
    {
        private static IComparer<T>? cmp;

        public StableSort(IComparer<T> comp)
        {
            cmp = comp;
        }


        /**
 * Entry point for merge sort
 */
        public void MergeSort(T[] a, int n)
        {
            // Sort a[0:n-1] using merge sort.
            int s = 1;   // segment size
            T[] b = new T[n];

            while (s < n)
            {
                MergePass(a, b, s, n); // merge from a to b
                s += s;                 // double the segment size
                MergePass(b, a, s, n); // merge from b to a
                s += s;                 // again, double the segment size
            } // end while
              // in C/C++, return the scratch array b by free/delete  --- tjr
        }  // end mergeSort

        /**
         * Perform one pass through the two arrays, invoking Merge() above
         */
        private static void MergePass(T[] x, T[] y, int s, int n)
        {
            // Merge adjacent segments of size s.
            int i = 0;

            while (i <= n - (2 * s))
            {//Merge two adjacent segments of size s
                Merge(x, y, i, i + s - 1, i + (2 * s) - 1);
                i += 2 * s;
            }
            // fewer than 2s elements remain
            if (i + s < n)
            {
                Merge(x, y, i, i + s - 1, n - 1);
            }
            else
            {
                for (int j = i; j <= n - 1; j++)
                {
                    y[j] = x[j];   // copy last segment to y
                }
            }
        } // end mergePass()

        /**
         * Merge from one array into another
         */
        private static void Merge(T[] c, T[] d, int lt, int md, int rt)
        {
            // Merge c[lt:md] and c[md+1:rt] to d[lt:rt]
            int i = lt,       // cursor for first segment
                j = md + 1,     // cursor for second
                k = lt;       // cursor for result

            // merge until i or j exits its segment
            while (i <= md && j <= rt)
            {
                d[k++] = cmp.Compare(c[i], c[j]) <= 0 ? c[i++] : c[j++];
            }

            // take care of left overs --- tjr code:  only one while loop actually runs
            while (i <= md)
            {
                d[k++] = c[i++];
            }

            while (j <= rt)
            {
                d[k++] = c[j++];
            }
        } // end merge()
    }
    internal class StdStableSort : ISorter
    {
        public string Title => "std::stable_sort on C++";

        public string Message => "";

        public string Category => "Merge sorts";

        public Complexity Time => Complexity.GOOD;

        public void RunSort<T>(T[] array, int end, int parameter, IComparer<T> cmp)
        {
            StableSort<T> stablesort = new(cmp);

            stablesort.MergeSort(array, end);
        }
    }
}
