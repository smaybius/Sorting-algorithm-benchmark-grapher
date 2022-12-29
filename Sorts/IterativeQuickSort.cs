using System.Collections.Generic;

namespace Sorting_algorithm_benchmark_grapher.Sorts
{
    internal class IterativeQuickSort : ISorter
    {
        public string Title => "Iterative quicksort";

        public string Message => "";

        public string Category => "Quick sorts";

        public Complexity Time => Complexity.GOOD;

        private static int Partition<T>(T[] a, int start, int stop, IComparer<T> cmp)
        {
            int up = start, down = stop - 1;
            T part = a[stop];
            if (stop <= start)
            {
                return start;
            }

            while (true)
            {
                while (cmp.Compare(a[up], part) < 0)
                {
                    up++;
                }
                while (cmp.Compare(part, a[down]) < 0 && up < down)
                {
                    down--;
                }
                if (up >= down)
                {
                    break;
                }

                Sort.Swap(a, up, down);
                up++;
                down--;
            }
            Sort.Swap(a, up, stop);
            return up;
        }

        private static void QSort<T>(T[] array, int startIndex, int endIndex, IComparer<T> cmp)
        {
            int[] stack = new int[2];
            int top = -1;

            stack[++top] = startIndex;
            stack[++top] = endIndex;

            while (top >= 0)
            {

                endIndex = stack[top--];
                startIndex = stack[top--];

                int p = Partition(array, startIndex, endIndex, cmp);

                if (p - 1 > startIndex)
                {
                    stack[++top] = startIndex;
                    if (top >= stack.Length - 1)
                    {
                        int[] newArr = Sort.CopyOf(stack, stack.Length + 1);
                        stack = newArr;
                    }
                    stack[++top] = p - 1;
                    if (top >= stack.Length - 1)
                    {
                        int[] newArr = Sort.CopyOf(stack, stack.Length + 1);
                        stack = newArr;
                    }
                }

                if (p + 1 < endIndex)
                {
                    stack[++top] = p + 1;
                    if (top >= stack.Length - 1)
                    {
                        int[] newArr = Sort.CopyOf(stack, stack.Length + 1);
                        stack = newArr;
                    }
                    stack[++top] = endIndex;
                    if (top >= stack.Length - 1)
                    {
                        int[] newArr = Sort.CopyOf(stack, stack.Length + 1);
                        stack = newArr;
                    }
                }

            }
        }
        public void RunSort<T>(T[] array, int sortLength, int parameter, IComparer<T> cmp)
        {
            QSort(array, 0, sortLength - 1, cmp);
        }
    }
}
