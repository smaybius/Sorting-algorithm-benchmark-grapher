using System.Collections.Generic;

namespace Sorting_algorithm_benchmark_grapher.Sorts
{
    internal class InsertionSort : ISorter
    {
        public string Title => "Insertion sort";

        public string Message => "";

        public string Category => "Insertion sorts";

        public Complexity Time => Complexity.QUADRATIC;

        public static void InsertSort<T>(T[] array, int start, int end, IComparer<T> cmp)
        {
            int pos;
            T current;

            for (int i = start; i < end; i++)
            {
                current = array[i];
                pos = i - 1;

                while (pos >= start && cmp.Compare(array[pos], current) > 0)
                {
                    array[pos + 1] = array[pos];
                    pos--;
                }
                array[pos + 1] = current;
            }
        }

        public void RunSort<T>(T[] array, int sortLength, int parameter, IComparer<T> cmp)
        {
            InsertSort(array, 0, sortLength, cmp);
        }
    }
}
