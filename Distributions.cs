using System;

namespace Sorting_algorithm_benchmark_grapher
{

    public sealed class Linear : IDistribution
    {
        public string Title => "Linear";

        public void InitializeArray(ArrayInt[] array)
        {
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = i;
            }
        }
    }

    public sealed class RandomNumbers : IDistribution
    {
        public string Title => "Random";

        public void InitializeArray(ArrayInt[] array)
        {
            Random random = new();
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = random.Next(array.Length);
            }
        }
    }
}
