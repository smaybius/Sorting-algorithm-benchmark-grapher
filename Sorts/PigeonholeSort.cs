using System.Collections.Generic;

namespace Sorting_algorithm_benchmark_grapher.Sorts
{
    internal class PigeonholeSort : IIntegerSorter
    {
        public string Title => "Pigeonhole sort";

        public string Message => "";

        public Complexity Time => Complexity.GOOD;

        public void RunSort(ArrayInt[] array, int sortLength, int parameter, IComparer<ArrayInt> cmp)
        {
            int min = int.MaxValue;
            int max = int.MinValue;

            for (int i = 0; i < sortLength; i++)
            {
                if (array[i] < min)
                {
                    min = array[i];
                }
                if (array[i] > max)
                {
                    max = array[i];
                }
            }

            int mi = min;
            int size = max - mi + 1;
            ArrayInt[] holes = new ArrayInt[size];

            for (int x = 0; x < sortLength; x++)
            {
                holes[array[x] - mi] = holes[array[x] - mi] + 1;
            }

            int j = 0;

            for (int count = 0; count < size; count++)
            {
                for (int i = 0; i < holes[count]; i++)
                {
                    array[j] = count + mi;
                    j++;
                }
            }
        }
    }
}
