using System.Collections.Generic;

namespace Sorting_algorithm_benchmark_grapher.Sorts
{
    public class Task
    {
        public int p;
        public int r;
        public int bit;

        public Task(int p, int r, int bit)
        {
            this.p = p;
            this.r = r;
            this.bit = bit;
        }
    }
    internal class BinaryQuickSortQueue : IIntegerSorter
    {
        public string Title => "Binary quicksort (with a queue)";

        public string Message => "";

        public Complexity Time => Complexity.GOOD;

        private static int Partition(ArrayInt[] array, int p, int r, int bit)
        {
            int i = p - 1;
            int j = r + 1;

            while (true)
            {
                // Left is not set
                i++;
                while (i <= r && !Sort.GetBit(array[i], bit))
                {
                    i++;
                }
                // Right is set
                j--;
                while (j >= p && Sort.GetBit(array[j], bit))
                {
                    j--;
                }
                // If i is less than j, we swap, otherwise we are done
                if (i < j)
                {
                    Sort.Swap(array, i, j);
                }
                else
                {
                    return j;
                }
            }
        }

        public void RunSort(ArrayInt[] array, int r, int parameter, IComparer<ArrayInt> cmp)
        {
            Queue<Task> tasks = new();
            int bit = Sort.AnalyzeBit(array, r - 1);
            tasks.Enqueue(new Task(0, r - 1, bit));

            while (tasks.Count != 0)
            {
                Task task = tasks.Dequeue();
                if (task.p < task.r && task.bit >= 0)
                {
                    int q = Partition(array, task.p, task.r, task.bit);
                    tasks.Enqueue(new Task(task.p, q, task.bit - 1));
                    tasks.Enqueue(new Task(q + 1, task.r, task.bit - 1));
                }
            }
        }
    }
}
