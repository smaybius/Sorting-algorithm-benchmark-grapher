using System.Collections.Generic;
// Refactored from C++ code written by Manish Bhojasia, found here:
// https://www.sanfoundry.com/cpp-program-implement-weak-heap/
namespace Sorting_algorithm_benchmark_grapher.Sorts
{
    internal class WeakHeapSort : ISorter
    {
        public string Title => "Weak heap sort";

        public string Message => "";

        public string Category => "Selection sorts";

        public Complexity Time => Complexity.GOOD;
        private static int GetBitwiseFlag(ArrayInt[] bits, int x)
        {
            return (bits[(x) >> 3] >> ((x) & 7)) & 1;
        }

        private static void ToggleBitwiseFlag(ArrayInt[] bits, int x)
        {
            int flag = bits[(x) >> 3];
            flag ^= 1 << ((x) & 7);

            bits[(x) >> 3] = flag;
        }

        /*
         * Merge Weak Heap
         */
        private static void WeakHeapMerge<T>(T[] array, ArrayInt[] bits, int i, int j, IComparer<T> cmp)
        {
            if (cmp.Compare(array[i], array[j]) == -1)
            {
                ToggleBitwiseFlag(bits, j);
                Sort.Swap(array, i, j);
            }
        }
        public void RunSort<T>(T[] array, int n, int parameter, IComparer<T> cmp)
        {
            int i, j, x, y, Gparent;

            int bitsLength = (n + 7) / 8;
            ArrayInt[] bits = new ArrayInt[bitsLength];

            for (i = 0; i < n / 8; ++i)
            {
                bits[i] = 0;
            }

            for (i = n - 1; i > 0; --i)
            {
                j = i;

                while ((j & 1) == GetBitwiseFlag(bits, j >> 1))
                {
                    j >>= 1;
                }

                Gparent = j >> 1;

                WeakHeapMerge(array, bits, Gparent, i, cmp);
            }

            for (i = n - 1; i >= 2; --i)
            {
                Sort.Swap(array, 0, i);

                x = 1;

                while ((y = (2 * x) + GetBitwiseFlag(bits, x)) < i)
                {
                    x = y;
                }

                while (x > 0)
                {
                    WeakHeapMerge(array, bits, 0, x, cmp);
                    x >>= 1;
                }
            }
            Sort.Swap(array, 0, 1);
        }
    }
}
