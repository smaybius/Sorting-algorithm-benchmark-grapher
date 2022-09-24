using System.Collections.Generic;

namespace Sorting_algorithm_benchmark_grapher.Sorts
{
    internal class BubbleSort : ISorter
    {
        public string Title => "Bubble sort";

        public string Message => "";

        public string Category => "Exchange sorts";

        public Complexity Time => Complexity.QUADRATIC;

        public void RunSort<T>(T[] array, int currentLength, int parameter, IComparer<T> cmp)
        {
            int s;
            int f = 1;
            int c;
            for (int j = currentLength - 1; j > 0; j -= c)
            {
                s = f - 1 < 0 ? 0 : f - 1;
                bool a = false;
                c = 1;
                for (int i = s; i < j; i++)
                {
                    if (cmp.Compare(array[i], array[i + 1]) > 0)
                    {
                        Sort.Swap(array, i, i + 1);
                        if (!a)
                        {
                            f = i;
                        }

                        a = true;
                        c = 1;
                    }
                    else
                    {
                        c++;
                    }
                }
            }
        }
    }
}
