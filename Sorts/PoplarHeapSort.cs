using System;
using System.Collections.Generic;
/*
 * The MIT License (MIT)
 *
 * Copyright (c) 2017-2019 Morwenn
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 */
namespace Sorting_algorithm_benchmark_grapher.Sorts
{
    internal class PoplarHeapSort //: ISorter // TODO: Out of bounds error on comparison
    {
        public string Title => "Poplar heapsort";

        public string Message => "";

        public string Category => "Selection sorts";

        public Complexity Time => Complexity.GOOD;

        ////////////////////////////////////////////////////////////
        // Generic helper functions
        ////////////////////////////////////////////////////////////

        // Returns 2^floor(log2(n)), assumes n > 0
        private static int Hyperfloor(int n)
        {
            return (int)Math.Pow(2, Math.Floor(Math.Log(n) / Math.Log(2)));
        }

        // Insertion sort which doesn't check for empty sequences
        private static void Unchecked_insertion_sort<T>(T[] array, int first, int last, IComparer<T> cmp)
        {
            for (int cur = first + 1; cur != last; ++cur)
            {
                int sift = cur;
                int sift_1 = cur - 1;

                // Compare first so we can avoid 2 moves for
                // an element already positioned correctly
                if (cmp.Compare(array[sift], array[sift_1]) == -1)
                {
                    T tmp = array[sift];
                    do
                    {
                        array[sift] = array[sift_1];
                    } while (--sift != first && cmp.Compare(tmp, array[--sift_1]) == -1);
                    array[sift] = tmp;
                }
            }
        }

        private static void Insertion_sort<T>(T[] array, int first, int last, IComparer<T> cmp)
        {
            if (first == last)
            {
                return;
            }

            Unchecked_insertion_sort(array, first, last, cmp);
        }

        ////////////////////////////////////////////////////////////
        // Poplar heap specific helper functions
        ////////////////////////////////////////////////////////////

        private static void Sift<T>(T[] array, int first, int size, IComparer<T> cmp)
        {
            if (size < 2)
            {
                return;
            }

            int root = first + (size - 1);
            int child_root1 = root - 1;
            int child_root2 = first + ((size / 2) - 1);

            while (true)
            {
                int max_root = root;
                if (cmp.Compare(array[max_root], array[child_root1]) == -1)
                {
                    max_root = child_root1;
                }
                if (cmp.Compare(array[max_root], array[child_root2]) == -1)
                {
                    max_root = child_root2;
                }
                if (max_root == root)
                {
                    return;
                }

                Sort.Swap(array, root, max_root);

                size /= 2;
                if (size < 2)
                {
                    return;
                }

                root = max_root;
                child_root1 = root - 1;
                child_root2 = max_root - (size - (size / 2));
            }
        }

        private static void Pop_heap_with_size<T>(T[] array, int first, int last, int size, IComparer<T> cmp)
        {
            int poplar_size = Hyperfloor(size + 1) - 1;
            int last_root = last - 1;
            int bigger = last_root;
            int bigger_size = poplar_size;

            // Look for the bigger poplar root
            int it = first;
            while (true)
            {
                int root = it + poplar_size - 1;
                if (root == last_root)
                {
                    break;
                }

                if (cmp.Compare(array[bigger], array[root]) == -1)
                {
                    bigger = root;
                    bigger_size = poplar_size;
                }
                it = root + 1;

                size -= poplar_size;
                poplar_size = Hyperfloor(size + 1) - 1;
            }

            // If a poplar root was bigger than the last one, exchange
            // them and sift
            if (bigger != last_root)
            {
                Sort.Swap(array, bigger, last_root);
                Sift(array, bigger - (bigger_size - 1), bigger_size, cmp);
            }
        }

        private static void Make_heap<T>(T[] array, int first, int last, IComparer<T> cmp)
        {
            int size = last - first;
            if (size < 2)
            {
                return;
            }

            // A sorted collection is a valid poplar heap; whenever the heap
            // is small, using insertion sort should be faster, which is why
            // we start by constructing 15-element poplars instead of 1-element
            // ones as the base case
            int small_poplar_size = 15;
            if (size <= small_poplar_size)
            {
                Unchecked_insertion_sort(array, first, last, cmp);
                return;
            }

            // Determines the "level" of the poplars seen so far; the log2 of this
            // variable will be used to make the binary carry sequence
            int poplar_level = 1;

            int it = first;
            int next = it + small_poplar_size;
            while (true)
            {
                // Make a 15 element poplar
                Unchecked_insertion_sort(array, it, next, cmp);

                int poplar_size = small_poplar_size;

                // Bit trick iterate without actually having to compute log2(poplar_level)
                for (int i = (poplar_level & (0 - poplar_level)) >> 1; i != 0; i >>= 1)
                {
                    it -= poplar_size;
                    poplar_size = (2 * poplar_size) + 1;
                    Sift(array, it, poplar_size, cmp);
                    ++next;
                }

                if ((last - next) <= small_poplar_size)
                {
                    Insertion_sort(array, next, last, cmp);
                    return;
                }

                it = next;
                next += small_poplar_size;
                ++poplar_level;
            }
        }

        private static void Sort_heap<T>(T[] array, int first, int last, IComparer<T> cmp)
        {
            int size = last - first;
            if (size < 2)
            {
                return;
            }

            do
            {
                Pop_heap_with_size(array, first, last, size, cmp);
                --last;
                --size;
            } while (size > 1);
        }

        public static void PoplarHeapify<T>(T[] array, int start, int length, IComparer<T> cmp)
        {
            Make_heap(array, start, length, cmp);
        }

        public static void HeapSort<T>(T[] array, int start, int end, IComparer<T> cmp)
        {
            Make_heap(array, start, end, cmp);
            Sort_heap(array, start, end, cmp);
        }
        public void RunSort<T>(T[] array, int length, int parameter, IComparer<T> cmp)
        {
            HeapSort(array, 0, length, cmp);
        }
    }
}
