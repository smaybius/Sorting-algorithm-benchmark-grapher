using System.Collections.Generic;

namespace Sorting_algorithm_benchmark_grapher.Sorts
{
    internal class TernaryHeapSort : ISorter
    {
        public string Title => "Ternary heapsort";

        public string Message => "";

        public string Category => "Selection sorts";

        public Complexity Time => Complexity.GOOD;

        // TERNARY HEAP SORT - written by qbit
        // https://codereview.stackexchange.com/questions/63384/binary-heapsort-and-ternary-heapsort-implementation

        private int heapSize;

        private static int LeftBranch(int i)
        {
            return (3 * i) + 1;
        }

        private static int MiddleBranch(int i)
        {
            return (3 * i) + 2;
        }

        private static int RightBranch(int i)
        {
            return (3 * i) + 3;
        }

        private void MaxHeapify<T>(T[] array, int i, IComparer<T> cmp)
        {

            int leftChild = LeftBranch(i);
            int rightChild = RightBranch(i);
            int middleChild = MiddleBranch(i);
            int largest;

            largest = leftChild <= heapSize && cmp.Compare(array[leftChild], array[i]) > 0 ? leftChild : i;

            if (rightChild <= heapSize && cmp.Compare(array[rightChild], array[largest]) > 0)
            {
                largest = rightChild;
            }

            if (middleChild <= heapSize && cmp.Compare(array[middleChild], array[largest]) > 0)
            {
                largest = middleChild;
            }


            if (largest != i)
            {
                Sort.Swap(array, i, largest);
                MaxHeapify(array, largest, cmp);
            }
        }

        private void BuildMaxTernaryHeap<T>(T[] array, int length, IComparer<T> cmp)
        {
            heapSize = length - 1;
            for (int i = length - (1 / 3); i >= 0; i--)
            {
                MaxHeapify(array, i, cmp);
            }
        }

        public void RunSort<T>(T[] array, int length, int parameter, IComparer<T> cmp)
        {
            BuildMaxTernaryHeap(array, length, cmp);

            for (int i = length - 1; i >= 0; i--)
            {
                Sort.Swap(array, 0, i); //add last element on array, i.e heap root

                heapSize--; //shrink heap by 1
                MaxHeapify(array, 0, cmp);
            }
        }
    }
}
