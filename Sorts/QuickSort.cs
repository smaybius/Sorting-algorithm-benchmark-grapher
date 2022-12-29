using System.Collections.Generic;

namespace Sorting_algorithm_benchmark_grapher.Sorts
{
    internal class QuickSort : ISorter
    {
        public string Title => "Quicksort";

        public string Message => "Enter partition algorithm (0: Lomuto, 1: Hoare, 2: Mod. Lomuto, 3: Mod. Hoare) (default: 1)";

        public string Category => "Quick sorts";

        public Complexity Time => Complexity.GOOD;

        private int partchoice;

        private static int LomutoPartition<T>(T[] a, int p, int r, IComparer<T> cmp)
        {
            T x = a[r]; // x stores the pivot element
            int i = p - 1;
            int j = p; // j is loop control variable
            while (j <= (r - 1))
            {
                if (cmp.Compare(a[j], x) <= 0) // a[p] to a[r-1] elements will be compared with pivot
                {
                    i++;
                    Sort.Swap(a, i, j);
                }
                j++;
            }
            Sort.Swap(a, i + 1, r);
            return i + 1; // return the location of the pivot
        }

        private int HoarePartition<T>(T[] a, int p, int r, IComparer<T> cmp)
        {
            if (p >= r)
            {
                return -1; // trivial return for empty array
            }

            T x = a[p]; // x stores the pivot element
            int i = p;
            int j = r + 1;
            while (true)
            {
                do
                {
                    i++;
                } while ((i <= r) && (cmp.Compare(a[i], x) < 0)); // searches the element larger than
                                                                  // pivot from Left portion
                do
                {
                    j--;
                } while (cmp.Compare(a[j], x) > 0); // searches the element smaller than pivot
                                                    // from the Right portion
                if (i > j)
                {
                    break;
                }

                Sort.Swap(a, i, j);
            }
            Sort.Swap(a, p, j); // swaps the larger element from the left with the // smaller element from
                                // the right
            return j; // returns the location of the pivot
        }

        private int ModifiedLomuto<T>(T[] a, int p, int r, IComparer<T> cmp)
        {
            T x = a[p]; // x stores the pivot element
            int i = p; // location i is vacant
            int j = r;
            while (true)
            {
                while (cmp.Compare(a[j], x) > 0)
                {
                    j--;
                }
                if (j <= i)
                {
                    break; // terminates the outer loop
                }

                a[i] = a[j];
                a[j] = a[i + 1];
                i++;
            }
            a[i] = x;
            return i; // returns the location of the pivot
        }

        private int ModifiedHoare<T>(T[] a, int p, int r, IComparer<T> cmp)
        {
            if (cmp.Compare(a[p], a[r]) > 0)
            {
                Sort.Swap(a, p, r); // Sentinel at both ends
            }

            T x = a[p]; // x stores the pivot and location p is vacant now.
            while (true)
            {
                do
                {
                    r--;
                } while (cmp.Compare(a[r], x) > 0); // search the smallest element in right portion.
                a[p] = a[r]; // location r is vacant now.
                do
                {
                    p++;
                } while (cmp.Compare(a[p], x) < 0); // search the larger element in left portion.
                if (p < r)
                {
                    a[r] = a[p]; // location p is vacant now.
                }
                else
                {
                    if (cmp.Compare(a[r + 1], x) <= 0)
                    {
                        r++;
                    }

                    a[r] = x;
                    return r; // return the location of the pivot
                }
            }
        }

        private int partition<T>(T[] array, int lo, int hi, IComparer<T> cmp)
        {
            return partchoice switch
            {
                0 => LomutoPartition(array, lo, hi, cmp),
                1 => HoarePartition(array, lo, hi, cmp),
                2 => ModifiedLomuto(array, lo, hi, cmp),
                3 => ModifiedHoare(array, lo, hi, cmp),
                _ => HoarePartition(array, lo, hi, cmp),
            };
        }

        private void quickSort<T>(T[] array, int lo, int hi, IComparer<T> cmp)
        {
            if (lo < hi)
            {
                int p = partition(array, lo, hi, cmp);
                quickSort(array, lo, p - 1, cmp);
                quickSort(array, p + 1, hi, cmp);
            }
        }
        public void RunSort<T>(T[] array, int currentLength, int parameter, IComparer<T> cmp)
        {
            partchoice = parameter;
            quickSort(array, 0, currentLength - 1, cmp);
        }
    }
}
