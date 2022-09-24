using System.Collections.Generic;

namespace Sorting_algorithm_benchmark_grapher.Sorts
{
    internal sealed class CountingSort : IIntegerSorter
    {
        public string Title => "Counting sort";

        public string Message => "";

        public Complexity Time => Complexity.GOOD;

        public void RunSort(ArrayInt[] array, int sortLength, int parameter, IComparer<ArrayInt> cmp)
        {
            int max = Sort.AnalyzeMax(array, sortLength, cmp);

            ArrayInt[] output = (ArrayInt[])array.Clone();
            int[] counts = new int[max + 1];

            for (int i = 0; i < sortLength; i++)
            {
                counts[array[i]] = counts[array[i]] + 1;
            }

            for (int i = 1; i < counts.Length; i++)
            {
                counts[i] = counts[i] + counts[i - 1];
            }

            for (int i = sortLength - 1; i >= 0; i--)
            {
                output[counts[array[i]] - 1] = array[i];
                counts[array[i]] = counts[array[i]] - 1;
            }

            // Extra loop to simulate the results from the "output" array being written
            // to the visual array.
            for (int i = sortLength - 1; i >= 0; i--)
            {
                array[i] = output[i];
            }
        }
    }
}
