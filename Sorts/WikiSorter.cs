using System;
using System.Collections.Generic;

namespace Sorting_algorithm_benchmark_grapher.Sorts
{
    /*
 *
This is free and unencumbered software released into the public domain.
Anyone is free to copy, modify, publish, use, compile, sell, or
distribute this software, either in source code form or as a compiled
binary, for any purpose, commercial or non-commercial, and by any
means.
In jurisdictions that recognize copyright laws, the author or authors
of this software dedicate any and all copyright interest in the
software to the public domain. We make this dedication for the benefit
of the public at large and to the detriment of our heirs and
successors. We intend this dedication to be an overt act of
relinquishment in perpetuity of all present and future rights to this
software under copyright law.
THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
IN NO EVENT SHALL THE AUTHORS BE LIABLE FOR ANY CLAIM, DAMAGES OR
OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
OTHER DEALINGS IN THE SOFTWARE.
For more information, please refer to <http://unlicense.org>
 *
 */

    // structure to represent ranges within the array
    internal sealed class Range
    {
        public int start;
        public int end;

        public Range(int start1, int end1)
        {
            start = start1;
            end = end1;
        }

        public Range()
        {
            start = 0;
            end = 0;
        }

        public void Set(int start1, int end1)
        {
            start = start1;
            end = end1;
        }

        public int Length()
        {
            return end - start;
        }
    }

    internal sealed class Pull
    {
        public int from, to, count;
        public Range range;

        public Pull()
        {
            range = new Range(0, 0);
        }

        public void Reset()
        {
            range.Set(0, 0);
            from = 0;
            to = 0;
            count = 0;
        }
    }

    // calculate how to scale the index value to the range within the array
    // the bottom-up merge sort only operates on values that are powers of two,
    // so scale down to that power of two, then use a fraction to scale back again
    internal sealed class Iterator
    {
        public int size, power_of_two;
        public int numerator, decm;
        public int denominator, decimal_step, numerator_step;

        // 63 -> 32, 64 -> 64, etc.
        // this comes from Hacker's Delight
        public static int FloorPowerOfTwo(int value)
        {
            int x = value;
            x |= x >> 1;
            x |= x >> 2;
            x |= x >> 4;
            x |= x >> 8;
            x |= x >> 16;
            return x - (x >> 1);
        }

        public Iterator(int size2, int min_level)
        {
            size = size2;
            power_of_two = FloorPowerOfTwo(size);
            denominator = power_of_two / min_level;
            numerator_step = size % denominator;
            decimal_step = size / denominator;
            Begin();
        }

        public void Begin()
        {
            numerator = decm = 0;
        }

        public Range NextRange()
        {
            int start = decm;

            decm += decimal_step;
            numerator += numerator_step;
            if (numerator >= denominator)
            {
                numerator -= denominator;
                decm++;
            }

            return new Range(start, decm);
        }

        public bool Finished()
        {
            return decm >= size;
        }

        public bool NextLevel()
        {
            decimal_step += decimal_step;
            numerator_step += numerator_step;
            if (numerator_step >= denominator)
            {
                numerator_step -= denominator;
                decimal_step++;
            }

            return decimal_step < size;
        }

        public int Length()
        {
            return decimal_step;
        }
    }
    internal class WikiSorter<T>
    {
        // use a small cache to speed up some of the operations
        // since the cache size is fixed, it's still O(1) memory!
        // just keep in mind that making it too small ruins the point (nothing will fit into it),
        // and making it too large also ruins the point (so much for "low memory"!)

        private static int cache_size = 0;
        private readonly T[]? cache;
        private readonly IComparer<T> cmp;

        // note that you can easily modify the above to allocate a dynamically sized cache
        // good choices for the cache size are:

        // (size + 1)/2 – turns into a full-speed standard merge sort since everything fits into the cache
        // sqrt((size + 1)/2) + 1 – this will be the size of the A blocks at the largest level of merges,
        // so a buffer of this size would allow it to skip using internal or in-place merges for anything

        // Original static buffer = 512 – chosen from careful testing as a good balance between fixed-size memory use and run time
        // ArrayVisualizer static buffer = 32, as the numbers of items we use for visual purposes is relatively small

        // 0 – if the system simply cannot allocate any extra memory whatsoever, no memory works just fine
        public WikiSorter(int cacheChoice, IComparer<T> cmp)
        {
            cache_size = cacheChoice;

            cache = cache_size != 0 ? (new T[cache_size]) : null;
            this.cmp = cmp;
        }

        // toolbox functions used by the sorter

        // find the index of the first value within the range that is equal to array[index]
        private int BinaryFirst(T[] array, T value, Range range)
        {
            int start = range.start, end = range.end - 1;
            while (start < end)
            {
                int mid = start + ((end - start) / 2);
                if (cmp.Compare(array[mid], value) < 0)
                {
                    start = mid + 1;
                }
                else
                {
                    end = mid;
                }
            }
            if (start == range.end - 1 && cmp.Compare(array[start], value) < 0)
            {
                start++;
            }

            return start;
        }

        // find the index of the last value within the range that is equal to array[index], plus 1
        private int BinaryLast(T[] array, T value, Range range)
        {
            int start = range.start, end = range.end - 1;
            while (start < end)
            {
                int mid = start + ((end - start) / 2);
                if (cmp.Compare(value, array[mid]) >= 0)
                {
                    start = mid + 1;
                }
                else
                {
                    end = mid;
                }
            }
            if (start == range.end - 1 && cmp.Compare(value, array[start]) >= 0)
            {
                start++;
            }

            return start;
        }

        // combine a linear search with a binary search to reduce the number of comparisons in situations
        // where have some idea as to how many unique values there are and where the next value might be
        private int FindFirstForward(T[] array, T value, Range range, int unique)
        {
            if (range.Length() == 0)
            {
                return range.start;
            }

            int index, skip = Math.Max(range.Length() / unique, 1);

            for (index = range.start + skip; cmp.Compare(array[index - 1], value) < 0; index += skip)
            {
                if (index >= range.end - skip)
                {
                    return BinaryFirst(array, value, new Range(index, range.end));
                }
            }

            return BinaryFirst(array, value, new Range(index - skip, index));
        }

        private int FindLastForward(T[] array, T value, Range range, int unique)
        {
            if (range.Length() == 0)
            {
                return range.start;
            }

            int index, skip = Math.Max(range.Length() / unique, 1);

            for (index = range.start + skip; cmp.Compare(value, array[index - 1]) >= 0; index += skip)
            {
                if (index >= range.end - skip)
                {
                    return BinaryLast(array, value, new Range(index, range.end));
                }
            }

            return BinaryLast(array, value, new Range(index - skip, index));
        }

        private int FindFirstBackward(T[] array, T value, Range range, int unique)
        {
            if (range.Length() == 0)
            {
                return range.start;
            }

            int index, skip = Math.Max(range.Length() / unique, 1);

            for (index = range.end - skip; index > range.start && cmp.Compare(array[index - 1], value) >= 0; index -= skip)
            {
                if (index < range.start + skip)
                {
                    return BinaryFirst(array, value, new Range(range.start, index));
                }
            }

            return BinaryFirst(array, value, new Range(index, index + skip));
        }

        private int FindLastBackward(T[] array, T value, Range range, int unique)
        {
            if (range.Length() == 0)
            {
                return range.start;
            }

            int index, skip = Math.Max(range.Length() / unique, 1);

            for (index = range.end - skip; index > range.start && cmp.Compare(value, array[index - 1]) < 0; index -= skip)
            {
                if (index < range.start + skip)
                {
                    return BinaryLast(array, value, new Range(range.start, index));
                }
            }

            return BinaryLast(array, value, new Range(index, index + skip));
        }

        // n^2 sorting algorithm used to sort tiny chunks of the full array
        private void InsertionSorter(T[] array, Range range)
        {
            InsertionSort.InsertSort(array, range.start, range.end, cmp);
        }

        // swap a series of values in the array
        private static void BlockSwap(T[] array, int start1, int start2, int block_size)
        {
            for (int index = 0; index < block_size; index++)
            {
                Sort.Swap(array, start1 + index, start2 + index);
            }
        }

        // rotate the values in an array ([0 1 2 3] becomes [1 2 3 0] if we rotate by 1)
        // this assumes that 0 <= amount <= range.length()
        private void Rotate(T[] array, int amount, Range range, bool use_cache)
        {
            if (range.Length() == 0)
            {
                return;
            }

            int split = amount >= 0 ? range.start + amount : range.end + amount;
            Range range1 = new(range.start, split);
            Range range2 = new(split, range.end);

            if (use_cache)
            {
                // if the smaller of the two ranges fits into the cache, it's *slightly* faster
                // copying it there and shifting the elements over
                if (range1.Length() <= range2.Length())
                {
                    if (range1.Length() <= cache_size)
                    {
                        if (cache != null)
                        {
                            Array.Copy(array, range1.start, cache, 0, range1.Length());
                            Array.Copy(array, range2.start, array, range1.start, range2.Length());
                            Array.Copy(cache, 0, array, range1.start + range2.Length(), range1.Length());
                        }
                        return;
                    }
                }
                else
                {
                    if (range2.Length() <= cache_size)
                    {
                        if (cache != null)
                        {
                            Array.Copy(array, range2.start, cache, 0, range2.Length());
                            Array.Copy(array, range1.start, array, range2.end - range1.Length(), range1.Length());
                            Array.Copy(cache, 0, array, range1.start, range2.Length());
                        }
                        return;
                    }
                }
            }

            /*
             * int lenA = range1.length();
             * int lenB = range2.length();
             * int pos = range.start;
             * 
             * while(lenA != 0 && lenB != 0) {
             * if(lenA <= lenB) {
             * this.BlockSwap(array, pos, pos + lenA, lenA);
             * pos += lenA;
             * lenB -= lenA;
             * }
             * else {
             * this.BlockSwap(array, pos + (lenA - lenB), pos + lenA, lenB);
             * lenA -= lenB;
             * }
             * }
             */

            Sort.IndexedRotate(array, range.start, split, range.end);
        }

        // merge two ranges from one array and save the results into a different array
        private void MergeInto(T[] from, Range A, Range B, T[] into, int at_index)
        {
            int A_index = A.start;
            int B_index = B.start;
            int insert_index = at_index;
            int A_last = A.end;
            int B_last = B.end;

            while (true)
            {
                if (cmp.Compare(from[B_index], from[A_index]) >= 0)
                {
                    into[insert_index] = from[A_index];

                    A_index++;
                    insert_index++;
                    if (A_index == A_last)
                    {
                        // copy the remainder of B into the final array
                        Array.Copy(from, B_index, into, insert_index, B_last - B_index);
                        break;
                    }
                }
                else
                {
                    into[insert_index] = from[B_index];


                    B_index++;
                    insert_index++;
                    if (B_index == B_last)
                    {
                        // copy the remainder of A into the final array
                        Array.Copy(from, A_index, into, insert_index, A_last - A_index);
                        break;
                    }
                }
            }
        }

        // merge operation using an external buffer,
        private void MergeExternal(T[] array, Range A, Range B)
        {
            // A fits into the cache, so use that instead of the internal buffer
            int A_index = 0;
            int B_index = B.start;
            int insert_index = A.start;
            int A_last = A.Length();
            int B_last = B.end;

            if (B.Length() > 0 && A.Length() > 0)
            {
                while (true)
                {
                    if (cmp.Compare(array[B_index], cache[A_index]) >= 0)
                    {
                        array[insert_index] = cache[A_index];
                        A_index++;
                        insert_index++;
                        if (A_index == A_last)
                        {
                            break;
                        }
                    }
                    else
                    {
                        array[insert_index] = array[B_index];
                        B_index++;
                        insert_index++;
                        if (B_index == B_last)
                        {
                            break;
                        }
                    }
                }
            }

            // copy the remainder of A into the final array
            if (cache != null)
            {
                Array.Copy(cache, A_index, array, insert_index, A_last - A_index);
            }
        }

        // merge operation using an internal buffer
        private void MergeInternal(T[] array, Range A, Range B, Range buffer)
        {
            // whenever we find a value to add to the final array, swap it with the value that's already in that spot
            // when this algorithm is finished, 'buffer' will contain its original contents, but in a different order
            int A_count = 0, B_count = 0, insert = 0;

            if (B.Length() > 0 && A.Length() > 0)
            {
                while (true)
                {
                    if (cmp.Compare(array[B.start + B_count], array[buffer.start + A_count]) >= 0)
                    {
                        Sort.Swap(array, A.start + insert, buffer.start + A_count);
                        A_count++;
                        insert++;
                        if (A_count >= A.Length())
                        {
                            break;
                        }
                    }
                    else
                    {
                        Sort.Swap(array, A.start + insert, B.start + B_count);
                        B_count++;
                        insert++;
                        if (B_count >= B.Length())
                        {
                            break;
                        }
                    }
                }
            }

            // swap the remainder of A into the final array
            WikiSorter<T>.BlockSwap(array, buffer.start + A_count, A.start + insert, A.Length() - A_count);
        }

        // merge operation without a buffer
        private void MergeInPlace(T[] array, Range A, Range B)
        {
            if (A.Length() == 0 || B.Length() == 0)
            {
                return;
            }

            /*
                this just repeatedly binary searches into B and rotates A into position.
                the paper suggests using the 'rotation-based Hwang and Lin algorithm' here,
                but I decided to stick with this because it had better situational performance
                (Hwang and Lin is designed for merging subarrays of very different sizes,
                but WikiSort almost always uses subarrays that are roughly the same size)
                normally this is incredibly suboptimal, but this function is only called
                when none of the A or B blocks in any subarray contained 2 sqrt(A) unique values,
                which places a hard limit on the number of times this will ACTUALLY need
                to binary search and rotate.
                according to my analysis the worst case is sqrt(A) rotations performed on sqrt(A) items
                once the constant factors are removed, which ends up being O(n)
                again, this is NOT a general-purpose solution – it only works well in this case!
                kind of like how the O(n^2) insertion sort is used in some places
             */

            A = new Range(A.start, A.end);
            B = new Range(B.start, B.end);

            while (true)
            {
                // find the first place in B where the first item in A needs to be inserted
                int mid = BinaryFirst(array, array[A.start], B);

                // rotate A into place
                int amount = mid - A.end;
                Rotate(array, -amount, new Range(A.start, mid), true);
                if (B.end == mid)
                {
                    break;
                }

                // calculate the new A and B ranges
                B.start = mid;
                A.Set(A.start + amount, B.start);
                A.start = BinaryLast(array, array[A.start], A);
                if (A.Length() == 0)
                {
                    break;
                }
            }
        }

        private void NetSwap(T[] array, ArrayInt[] order, Range range, int x, int y)
        {
            int compare = cmp.Compare(array[range.start + x], array[range.start + y]);
            if (compare > 0 || (order[x] > order[y] && compare == 0))
            {
                Sort.Swap(array, range.start + x, range.start + y);
                Sort.Swap(order, x, y);
            }
        }

        // bottom-up merge sort combined with an in-place merge algorithm for O(1) memory use
        public void WSort(T[] array, int len)
        {
            int size = len;

            // if the array is of size 0, 1, 2, or 3, just sort them like so:
            if (size < 4)
            {
                if (size == 3)
                {
                    // hard-coded insertion sort
                    if (cmp.Compare(array[1], array[0]) < 0)
                    {
                        Sort.Swap(array, 0, 1);
                    }
                    if (cmp.Compare(array[2], array[1]) < 0)
                    {
                        Sort.Swap(array, 1, 2);
                        if (cmp.Compare(array[1], array[0]) < 0)
                        {
                            Sort.Swap(array, 0, 1);
                        }
                    }
                }
                else if (size == 2)
                {
                    // swap the items if they're out of order
                    if (cmp.Compare(array[1], array[0]) < 0)
                    {
                        Sort.Swap(array, 0, 1);
                    }
                }
                return;
            }

            // sort groups of 4-8 items at a time using an unstable sorting network,
            // but keep track of the original item orders to force it to be stable
            // http://pages.ripco.net/~jgamble/nw.html
            Iterator iterator = new(size, 4);
            while (!iterator.Finished())
            {
                ArrayInt[] order = { 0, 1, 2, 3, 4, 5, 6, 7 };
                Range range = iterator.NextRange();

                if (range.Length() == 8)
                {
                    NetSwap(array, order, range, 0, 1); NetSwap(array, order, range, 2, 3);
                    NetSwap(array, order, range, 4, 5); NetSwap(array, order, range, 6, 7);
                    NetSwap(array, order, range, 0, 2); NetSwap(array, order, range, 1, 3);
                    NetSwap(array, order, range, 4, 6); NetSwap(array, order, range, 5, 7);
                    NetSwap(array, order, range, 1, 2); NetSwap(array, order, range, 5, 6);
                    NetSwap(array, order, range, 0, 4); NetSwap(array, order, range, 3, 7);
                    NetSwap(array, order, range, 1, 5); NetSwap(array, order, range, 2, 6);
                    NetSwap(array, order, range, 1, 4); NetSwap(array, order, range, 3, 6);
                    NetSwap(array, order, range, 2, 4); NetSwap(array, order, range, 3, 5);
                    NetSwap(array, order, range, 3, 4);

                }
                else if (range.Length() == 7)
                {
                    NetSwap(array, order, range, 1, 2); NetSwap(array, order, range, 3, 4); NetSwap(array, order, range, 5, 6);
                    NetSwap(array, order, range, 0, 2); NetSwap(array, order, range, 3, 5); NetSwap(array, order, range, 4, 6);
                    NetSwap(array, order, range, 0, 1); NetSwap(array, order, range, 4, 5); NetSwap(array, order, range, 2, 6);
                    NetSwap(array, order, range, 0, 4); NetSwap(array, order, range, 1, 5);
                    NetSwap(array, order, range, 0, 3); NetSwap(array, order, range, 2, 5);
                    NetSwap(array, order, range, 1, 3); NetSwap(array, order, range, 2, 4);
                    NetSwap(array, order, range, 2, 3);

                }
                else if (range.Length() == 6)
                {
                    NetSwap(array, order, range, 1, 2); NetSwap(array, order, range, 4, 5);
                    NetSwap(array, order, range, 0, 2); NetSwap(array, order, range, 3, 5);
                    NetSwap(array, order, range, 0, 1); NetSwap(array, order, range, 3, 4); NetSwap(array, order, range, 2, 5);
                    NetSwap(array, order, range, 0, 3); NetSwap(array, order, range, 1, 4);
                    NetSwap(array, order, range, 2, 4); NetSwap(array, order, range, 1, 3);
                    NetSwap(array, order, range, 2, 3);

                }
                else if (range.Length() == 5)
                {
                    NetSwap(array, order, range, 0, 1); NetSwap(array, order, range, 3, 4);
                    NetSwap(array, order, range, 2, 4);
                    NetSwap(array, order, range, 2, 3); NetSwap(array, order, range, 1, 4);
                    NetSwap(array, order, range, 0, 3);
                    NetSwap(array, order, range, 0, 2); NetSwap(array, order, range, 1, 3);
                    NetSwap(array, order, range, 1, 2);

                }
                else if (range.Length() == 4)
                {
                    NetSwap(array, order, range, 0, 1); NetSwap(array, order, range, 2, 3);
                    NetSwap(array, order, range, 0, 2); NetSwap(array, order, range, 1, 3);
                    NetSwap(array, order, range, 1, 2);
                }
            }
            if (size < 8)
            {
                return;
            }


            // we need to keep track of a lot of ranges during this sort!
            Range buffer1 = new(), buffer2 = new();
            Range blockA = new(), blockB = new();
            Range lastA = new(), lastB = new();
            Range firstA = new();

            Pull[] pull = new Pull[2];
            pull[0] = new Pull();
            pull[1] = new Pull();

            // then merge sort the higher levels, which can be 8-15, 16-31, 32-63, 64-127, etc.
            while (true)
            {
                Range A;
                Range B;
                // if every A and B block will fit into the cache, use a special branch specifically for merging with the cache
                // (we use < rather than <= since the block size might be one more than iterator.length())
                if (iterator.Length() < cache_size)
                {

                    // if four subarrays fit into the cache, it's faster to merge both pairs of subarrays into the cache,
                    // then merge the two merged subarrays from the cache back into the original array
                    if ((iterator.Length() + 1) * 4 <= cache_size && iterator.Length() * 4 <= size)
                    {
                        iterator.Begin();
                        while (!iterator.Finished())
                        {
                            // merge A1 and B1 into the cache
                            Range A1 = iterator.NextRange();
                            Range B1 = iterator.NextRange();
                            Range A2 = iterator.NextRange();
                            Range B2 = iterator.NextRange();

                            if (cmp.Compare(array[B1.end - 1], array[A1.start]) < 0)
                            {
                                // the two ranges are in reverse order, so copy them in reverse order into the cache
                                Array.Copy(array, A1.start, cache, B1.Length(), A1.Length());
                                Array.Copy(array, B1.start, cache, 0, B1.Length());
                            }
                            else if (cmp.Compare(array[B1.start], array[A1.end - 1]) < 0)
                            {
                                // these two ranges weren't already in order, so merge them into the cache
                                MergeInto(array, A1, B1, cache, 0);
                            }
                            else
                            {
                                // if A1, B1, A2, and B2 are all in order, skip doing anything else
                                if (cmp.Compare(array[B2.start], array[A2.end - 1]) >= 0 && cmp.Compare(array[A2.start], array[B1.end - 1]) >= 0)
                                {
                                    continue;
                                }

                                // copy A1 and B1 into the cache in the same order
                                Array.Copy(array, A1.start, cache, 0, A1.Length());
                                Array.Copy(array, B1.start, cache, A1.Length(), B1.Length());
                            }
                            A1.Set(A1.start, B1.end);

                            // merge A2 and B2 into the cache
                            if (cmp.Compare(array[B2.end - 1], array[A2.start]) < 0)
                            {
                                // the two ranges are in reverse order, so copy them in reverse order into the cache
                                Array.Copy(array, A2.start, cache, A1.Length() + B2.Length(), A2.Length());
                                Array.Copy(array, B2.start, cache, A1.Length(), B2.Length());
                            }
                            else if (cmp.Compare(array[B2.start], array[A2.end - 1]) < 0)
                            {
                                // these two ranges weren't already in order, so merge them into the cache
                                MergeInto(array, A2, B2, cache, A1.Length());
                            }
                            else
                            {
                                // copy A2 and B2 into the cache in the same order
                                Array.Copy(array, A2.start, cache, A1.Length(), A2.Length());
                                Array.Copy(array, B2.start, cache, A1.Length() + A2.Length(), B2.Length());
                            }
                            A2.Set(A2.start, B2.end);

                            // merge A1 and A2 from the cache into the array
                            Range A3 = new(0, A1.Length());
                            Range B3 = new(A1.Length(), A1.Length() + A2.Length());

                            if (cmp.Compare(cache[B3.end - 1], cache[A3.start]) < 0)
                            {
                                // the two ranges are in reverse order, so copy them in reverse order into the cache
                                Array.Copy(cache, A3.start, array, A1.start + A2.Length(), A3.Length());
                                Array.Copy(cache, B3.start, array, A1.start, B3.Length());
                            }
                            else if (cmp.Compare(cache[B3.start], cache[A3.end - 1]) < 0)
                            {
                                // these two ranges weren't already in order, so merge them back into the array
                                MergeInto(cache, A3, B3, array, A1.start);
                            }
                            else
                            {
                                // copy A3 and B3 into the array in the same order
                                Array.Copy(cache, A3.start, array, A1.start, A3.Length());
                                Array.Copy(cache, B3.start, array, A1.start + A1.Length(), B3.Length());
                            }
                        }

                        // we merged two levels at the same time, so we're done with this level already
                        // (iterator.nextLevel() is called again at the bottom of this outer merge loop)
                        _ = iterator.NextLevel();

                    }
                    else
                    {
                        iterator.Begin();
                        while (!iterator.Finished())
                        {
                            A = iterator.NextRange();
                            B = iterator.NextRange();

                            if (cmp.Compare(array[B.end - 1], array[A.start]) < 0)
                            {
                                // the two ranges are in reverse order, so a simple rotation should fix it
                                Rotate(array, A.Length(), new Range(A.start, B.end), true);
                            }
                            else if (cmp.Compare(array[B.start], array[A.end - 1]) < 0)
                            {
                                // these two ranges weren't already in order, so we'll need to merge them!
                                Array.Copy(array, A.start, cache, 0, A.Length());
                                MergeExternal(array, A, B);
                            }
                        }
                    }
                }
                else
                {
                    // this is where the in-place merge logic starts!
                    // 1. pull out two internal buffers each containing sqrt(A) unique values
                    //     1a. adjust block_size and buffer_size if we couldn't find enough unique values
                    // 2. loop over the A and B subarrays within this level of the merge sort
                    //     3. break A and B into blocks of size 'block_size'
                    //     4. "tag" each of the A blocks with values from the first internal buffer
                    //     5. roll the A blocks through the B blocks and drop/rotate them where they belong
                    //     6. merge each A block with any B values that follow, using the cache or the second internal buffer
                    // 7. sort the second internal buffer if it exists
                    // 8. redistribute the two internal buffers back into the array

                    int block_size = (int)Math.Sqrt(iterator.Length());
                    int buffer_size = (iterator.Length() / block_size) + 1;

                    // as an optimization, we really only need to pull out the internal buffers once for each level of merges
                    // after that we can reuse the same buffers over and over, then redistribute it when we're finished with this level
                    int index, last, count, pull_index = 0;
                    buffer1.Set(0, 0);
                    buffer2.Set(0, 0);

                    pull[0].Reset();
                    pull[1].Reset();

                    // find two internal buffers of size 'buffer_size' each
                    int find = buffer_size + buffer_size;
                    bool find_separately = false;

                    if (block_size <= cache_size)
                    {
                        // if every A block fits into the cache then we won't need the second internal buffer,
                        // so we really only need to find 'buffer_size' unique values
                        find = buffer_size;
                    }
                    else if (find > iterator.Length())
                    {
                        // we can't fit both buffers into the same A or B subarray, so find two buffers separately
                        find = buffer_size;
                        find_separately = true;
                    }

                    // we need to find either a single contiguous space containing 2 sqrt(A) unique values (which will be split up into two buffers of size sqrt(A) each),
                    // or we need to find one buffer of < 2 sqrt(A) unique values, and a second buffer of sqrt(A) unique values,
                    // OR if we couldn't find that many unique values, we need the largest possible buffer we can get

                    // in the case where it couldn't find a single buffer of at least sqrt(A) unique values,
                    // all of the Merge steps must be replaced by a different merge algorithm (MergeInPlace)

                    iterator.Begin();
                    while (!iterator.Finished())
                    {
                        A = iterator.NextRange();
                        B = iterator.NextRange();

                        // check A for the number of unique values we need to fill an internal buffer
                        // these values will be pulled out to the start of A
                        for (last = A.start, count = 1; count < find; last = index, count++)
                        {
                            index = FindLastForward(array, array[last], new Range(last + 1, A.end), find - count);
                            if (index == A.end)
                            {
                                break;
                            }
                        }
                        index = last;

                        if (count >= buffer_size)
                        {
                            // keep track of the range within the array where we'll need to "pull out" these values to create the internal buffer
                            pull[pull_index].range.Set(A.start, B.end);
                            pull[pull_index].count = count;
                            pull[pull_index].from = index;
                            pull[pull_index].to = A.start;
                            pull_index = 1;

                            if (count == buffer_size + buffer_size)
                            {
                                // we were able to find a single contiguous section containing 2 sqrt(A) unique values,
                                // so this section can be used to contain both of the internal buffers we'll need
                                buffer1.Set(A.start, A.start + buffer_size);
                                buffer2.Set(A.start + buffer_size, A.start + count);
                                break;
                            }
                            else if (find == buffer_size + buffer_size)
                            {
                                // we found a buffer that contains at least sqrt(A) unique values, but did not contain the full 2 sqrt(A) unique values,
                                // so we still need to find a second separate buffer of at least sqrt(A) unique values
                                buffer1.Set(A.start, A.start + count);
                                find = buffer_size;
                            }
                            else if (block_size <= cache_size)
                            {
                                // we found the first and only internal buffer that we need, so we're done!
                                buffer1.Set(A.start, A.start + count);
                                break;
                            }
                            else if (find_separately)
                            {
                                // found one buffer, but now find the other one
                                buffer1 = new Range(A.start, A.start + count);
                                find_separately = false;
                            }
                            else
                            {
                                // we found a second buffer in an 'A' subarray containing sqrt(A) unique values, so we're done!
                                buffer2.Set(A.start, A.start + count);
                                break;
                            }
                        }
                        else if (pull_index == 0 && count > buffer1.Length())
                        {
                            // keep track of the largest buffer we were able to find
                            buffer1.Set(A.start, A.start + count);

                            pull[pull_index].range.Set(A.start, B.end);
                            pull[pull_index].count = count;
                            pull[pull_index].from = index;
                            pull[pull_index].to = A.start;
                        }

                        // check B for the number of unique values we need to fill an internal buffer
                        // these values will be pulled out to the end of B
                        for (last = B.end - 1, count = 1; count < find; last = index - 1, count++)
                        {
                            index = FindFirstBackward(array, array[last], new Range(B.start, last), find - count);
                            if (index == B.start)
                            {
                                break;
                            }
                        }
                        index = last;

                        if (count >= buffer_size)
                        {
                            // keep track of the range within the array where we'll need to "pull out" these values to create the internal buffer
                            pull[pull_index].range.Set(A.start, B.end);
                            pull[pull_index].count = count;
                            pull[pull_index].from = index;
                            pull[pull_index].to = B.end;
                            pull_index = 1;

                            if (count == buffer_size + buffer_size)
                            {
                                // we were able to find a single contiguous section containing 2 sqrt(A) unique values,
                                // so this section can be used to contain both of the internal buffers we'll need
                                buffer1.Set(B.end - count, B.end - buffer_size);
                                buffer2.Set(B.end - buffer_size, B.end);
                                break;
                            }
                            else if (find == buffer_size + buffer_size)
                            {
                                // we found a buffer that contains at least sqrt(A) unique values, but did not contain the full 2sqrt(A) unique values,
                                // so we still need to find a second separate buffer of at least sqrt(A) unique values
                                buffer1.Set(B.end - count, B.end);
                                find = buffer_size;
                            }
                            else if (block_size <= cache_size)
                            {
                                // we found the first and only internal buffer that we need, so we're done!
                                buffer1.Set(B.end - count, B.end);
                                break;
                            }
                            else if (find_separately)
                            {
                                // found one buffer, but now find the other one
                                buffer1 = new Range(B.end - count, B.end);
                                find_separately = false;
                            }
                            else
                            {
                                // buffer2 will be pulled out from a 'B' subarray, so if the first buffer was pulled out from the corresponding 'A' subarray,
                                // we need to adjust the end point for that A subarray so it knows to stop redistributing its values before reaching buffer2
                                if (pull[0].range.start == A.start)
                                {
                                    pull[0].range.end -= pull[1].count;
                                }

                                // we found a second buffer in an 'B' subarray containing sqrt(A) unique values, so we're done!
                                buffer2.Set(B.end - count, B.end);
                                break;
                            }
                        }
                        else if (pull_index == 0 && count > buffer1.Length())
                        {
                            // keep track of the largest buffer we were able to find
                            buffer1.Set(B.end - count, B.end);

                            pull[pull_index].range.Set(A.start, B.end);
                            pull[pull_index].count = count;
                            pull[pull_index].from = index;
                            pull[pull_index].to = B.end;
                        }
                    }

                    // pull out the two ranges so we can use them as internal buffers
                    for (pull_index = 0; pull_index < 2; pull_index++)
                    {
                        int length = pull[pull_index].count;

                        if (pull[pull_index].to < pull[pull_index].from)
                        {
                            // we're pulling the values out to the left, which means the start of an A subarray
                            index = pull[pull_index].from;
                            for (count = 1; count < length; count++)
                            {
                                index = FindFirstBackward(array, array[index - 1], new Range(pull[pull_index].to, pull[pull_index].from - (count - 1)), length - count);
                                Range range = new(index + 1, pull[pull_index].from + 1);
                                Rotate(array, range.Length() - count, range, true);
                                pull[pull_index].from = index + count;
                            }
                        }
                        else if (pull[pull_index].to > pull[pull_index].from)
                        {
                            // we're pulling values out to the right, which means the end of a B subarray
                            index = pull[pull_index].from + 1;
                            for (count = 1; count < length; count++)
                            {
                                index = FindLastForward(array, array[index], new Range(index, pull[pull_index].to), length - count);
                                Range range = new(pull[pull_index].from, index - 1);
                                Rotate(array, count, range, true);
                                pull[pull_index].from = index - 1 - count;
                            }
                        }
                    }

                    // adjust block_size and buffer_size based on the values we were able to pull out
                    buffer_size = buffer1.Length();
                    block_size = (iterator.Length() / buffer_size) + 1;

                    // the first buffer NEEDS to be large enough to tag each of the evenly sized A blocks,
                    // so this was originally here to test the math for adjusting block_size above
                    //if ((iterator.length() + 1)/block_size > buffer_size) throw new RuntimeException();

                    // now that the two internal buffers have been created, it's time to merge each A+B combination at this level of the merge sort!
                    iterator.Begin();
                    while (!iterator.Finished())
                    {
                        A = iterator.NextRange();
                        B = iterator.NextRange();

                        // remove any parts of A or B that are being used by the internal buffers
                        int start = A.start;
                        if (start == pull[0].range.start)
                        {
                            if (pull[0].from > pull[0].to)
                            {
                                A.start += pull[0].count;

                                // if the internal buffer takes up the entire A or B subarray, then there's nothing to merge
                                // this only happens for very small subarrays, like sqrt(4) = 2, 2 * (2 internal buffers) = 4,
                                // which also only happens when cache_size is small or 0 since it'd otherwise use MergeExternal
                                if (A.Length() == 0)
                                {
                                    continue;
                                }
                            }
                            else if (pull[0].from < pull[0].to)
                            {
                                B.end -= pull[0].count;
                                if (B.Length() == 0)
                                {
                                    continue;
                                }
                            }
                        }
                        if (start == pull[1].range.start)
                        {
                            if (pull[1].from > pull[1].to)
                            {
                                A.start += pull[1].count;
                                if (A.Length() == 0)
                                {
                                    continue;
                                }
                            }
                            else if (pull[1].from < pull[1].to)
                            {
                                B.end -= pull[1].count;
                                if (B.Length() == 0)
                                {
                                    continue;
                                }
                            }
                        }

                        if (cmp.Compare(array[B.end - 1], array[A.start]) < 0)
                        {
                            // the two ranges are in reverse order, so a simple rotation should fix it
                            Rotate(array, A.Length(), new Range(A.start, B.end), true);
                        }
                        else if (cmp.Compare(array[A.end], array[A.end - 1]) < 0)
                        {
                            // these two ranges weren't already in order, so we'll need to merge them!

                            // break the remainder of A into blocks. firstA is the uneven-sized first A block
                            blockA.Set(A.start, A.end);
                            firstA.Set(A.start, A.start + (blockA.Length() % block_size));

                            // swap the first value of each A block with the value in buffer1
                            int indexA = buffer1.start;
                            for (index = firstA.end; index < blockA.end; index += block_size)
                            {
                                Sort.Swap(array, indexA, index);
                                indexA++;
                            }

                            // start rolling the A blocks through the B blocks!
                            // whenever we leave an A block behind, we'll need to merge the previous A block with any B blocks that follow it, so track that information as well
                            lastA.Set(firstA.start, firstA.end);
                            lastB.Set(0, 0);
                            blockB.Set(B.start, B.start + Math.Min(block_size, B.Length()));
                            blockA.start += firstA.Length();
                            indexA = buffer1.start;

                            // if the first unevenly sized A block fits into the cache, copy it there for when we go to Merge it
                            // otherwise, if the second buffer is available, block swap the contents into that
                            if (lastA.Length() <= cache_size && cache != null)
                            {
                                Array.Copy(array, lastA.start, cache, 0, lastA.Length());
                            }
                            else if (buffer2.Length() > 0)
                            {
                                WikiSorter<T>.BlockSwap(array, lastA.start, buffer2.start, lastA.Length());
                            }

                            if (blockA.Length() > 0)
                            {
                                while (true)
                                {
                                    // if there's a previous B block and the first value of the minimum A block is <= the last value of the previous B block,
                                    // then drop that minimum A block behind. or if there are no B blocks left then keep dropping the remaining A blocks.
                                    if ((lastB.Length() > 0 && cmp.Compare(array[lastB.end - 1], array[indexA]) >= 0) || blockB.Length() == 0)
                                    {
                                        // figure out where to split the previous B block, and rotate it at the split
                                        int B_split = BinaryFirst(array, array[indexA], lastB);
                                        int B_remaining = lastB.end - B_split;

                                        // swap the minimum A block to the beginning of the rolling A blocks
                                        int minA = blockA.start;
                                        for (int findA = minA + block_size; findA < blockA.end; findA += block_size)
                                        {
                                            if (cmp.Compare(array[findA], array[minA]) < 0)
                                            {
                                                minA = findA;
                                            }
                                        }

                                        WikiSorter<T>.BlockSwap(array, blockA.start, minA, block_size);

                                        // swap the first item of the previous A block back with its original value, which is stored in buffer1
                                        Sort.Swap(array, blockA.start, indexA);
                                        indexA++;

                                        // locally merge the previous A block with the B values that follow it
                                        // if lastA fits into the external cache we'll use that (with MergeExternal),
                                        // or if the second internal buffer exists we'll use that (with MergeInternal),
                                        // or failing that we'll use a strictly in-place merge algorithm (MergeInPlace)
                                        if (lastA.Length() <= cache_size)
                                        {
                                            MergeExternal(array, lastA, new Range(lastA.end, B_split));
                                        }
                                        else if (buffer2.Length() > 0)
                                        {
                                            MergeInternal(array, lastA, new Range(lastA.end, B_split), buffer2);
                                        }
                                        else
                                        {
                                            MergeInPlace(array, lastA, new Range(lastA.end, B_split));
                                        }

                                        if (buffer2.Length() > 0 || block_size <= cache_size)
                                        {
                                            // copy the previous A block into the cache or buffer2, since that's where we need it to be when we go to merge it anyway
                                            if (block_size <= cache_size)
                                            {
                                                Array.Copy(array, blockA.start, cache, 0, block_size);
                                            }
                                            else
                                            {
                                                WikiSorter<T>.BlockSwap(array, blockA.start, buffer2.start, block_size);
                                            }

                                            // this is equivalent to rotating, but faster
                                            // the area normally taken up by the A block is either the contents of buffer2, or data we don't need anymore since we memcopied it
                                            // either way, we don't need to retain the order of those items, so instead of rotating we can just block swap B to where it belongs
                                            WikiSorter<T>.BlockSwap(array, B_split, blockA.start + block_size - B_remaining, B_remaining);
                                        }
                                        else
                                        {
                                            // we are unable to use the 'buffer2' trick to speed up the rotation operation since buffer2 doesn't exist, so perform a normal rotation
                                            Rotate(array, blockA.start - B_split, new Range(B_split, blockA.start + block_size), true);
                                        }

                                        // update the range for the remaining A blocks, and the range remaining from the B block after it was split
                                        lastA.Set(blockA.start - B_remaining, blockA.start - B_remaining + block_size);
                                        lastB.Set(lastA.end, lastA.end + B_remaining);

                                        // if there are no more A blocks remaining, this step is finished!
                                        blockA.start += block_size;
                                        if (blockA.Length() == 0)
                                        {
                                            break;
                                        }
                                    }
                                    else if (blockB.Length() < block_size)
                                    {
                                        // move the last B block, which is unevenly sized, to before the remaining A blocks, by using a rotation
                                        // the cache is disabled here since it might contain the contents of the previous A block
                                        Rotate(array, -blockB.Length(), new Range(blockA.start, blockB.end), false);

                                        lastB.Set(blockA.start, blockA.start + blockB.Length());
                                        blockA.start += blockB.Length();
                                        blockA.end += blockB.Length();
                                        blockB.end = blockB.start;
                                    }
                                    else
                                    {
                                        // roll the leftmost A block to the end by swapping it with the next B block
                                        WikiSorter<T>.BlockSwap(array, blockA.start, blockB.start, block_size);
                                        lastB.Set(blockA.start, blockA.start + block_size);

                                        blockA.start += block_size;
                                        blockA.end += block_size;
                                        blockB.start += block_size;
                                        blockB.end += block_size;

                                        if (blockB.end > B.end)
                                        {
                                            blockB.end = B.end;
                                        }
                                    }
                                }
                            }

                            // merge the last A block with the remaining B values
                            if (lastA.Length() <= cache_size)
                            {
                                MergeExternal(array, lastA, new Range(lastA.end, B.end));
                            }
                            else if (buffer2.Length() > 0)
                            {
                                MergeInternal(array, lastA, new Range(lastA.end, B.end), buffer2);
                            }
                            else
                            {
                                MergeInPlace(array, lastA, new Range(lastA.end, B.end));
                            }
                        }
                    }

                    // when we're finished with this merge step we should have the one or two internal buffers left over, where the second buffer is all jumbled up
                    // insertion sort the second buffer, then redistribute the buffers back into the array using the opposite process used for creating the buffer

                    // while an unstable sort like quick sort could be applied here, in benchmarks it was consistently slightly slower than a simple insertion sort,
                    // even for tens of millions of items. this may be because insertion sort is quite fast when the data is already somewhat sorted, like it is here
                    InsertionSorter(array, buffer2);

                    for (pull_index = 0; pull_index < 2; pull_index++)
                    {
                        int unique = pull[pull_index].count * 2;
                        if (pull[pull_index].from > pull[pull_index].to)
                        {
                            // the values were pulled out to the left, so redistribute them back to the right
                            Range buffer = new(pull[pull_index].range.start, pull[pull_index].range.start + pull[pull_index].count);
                            while (buffer.Length() > 0)
                            {
                                index = FindFirstForward(array, array[buffer.start], new Range(buffer.end, pull[pull_index].range.end), unique);
                                int amount = index - buffer.end;
                                Rotate(array, buffer.Length(), new Range(buffer.start, index), true);
                                buffer.start += amount + 1;
                                buffer.end += amount;
                                unique -= 2;
                            }
                        }
                        else if (pull[pull_index].from < pull[pull_index].to)
                        {
                            // the values were pulled out to the right, so redistribute them back to the left
                            Range buffer = new(pull[pull_index].range.end - pull[pull_index].count, pull[pull_index].range.end);
                            while (buffer.Length() > 0)
                            {
                                index = FindLastBackward(array, array[buffer.end - 1], new Range(pull[pull_index].range.start, buffer.start), unique);
                                int amount = buffer.start - index;
                                Rotate(array, amount, new Range(index, buffer.end), true);
                                buffer.start -= amount;
                                buffer.end -= amount + 1;
                                unique -= 2;
                            }
                        }
                    }
                }

                // double the size of each A and B subarray that will be merged in the next level
                if (!iterator.NextLevel())
                {
                    break;
                }
            }
        }
    }
}
