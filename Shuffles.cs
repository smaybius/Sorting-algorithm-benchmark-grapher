using ScottPlot.Drawing.Colormaps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Sorting_algorithm_benchmark_grapher.PenultimateBitonic;

namespace Sorting_algorithm_benchmark_grapher
{
    public class RandomShuffle : IShuffle
    {
        public string Title => "Random shuffle";

        public void ShuffleArray<T>(T[] array, int start, int end, IComparer<T> cmp)
        {
            ShuffleUtils.Shuffle(array, start, end);
        }
    }

    public class Reversed : IShuffle
    {
        public string Title => "Reversed";

        public void ShuffleArray<T>(T[] array, int start, int end, IComparer<T> cmp)
        {
            Array.Reverse(array, start, end);
        }
    }

    public class Almost : IShuffle
    {
        public string Title => "Slight shuffle";

        public void ShuffleArray<T>(T[] array, int start, int end, IComparer<T> cmp)
        {
            for (int i = 0; i < Math.Max(end / 20, 1); i++)
            {
                Sort.Swap(array, ShuffleUtils.random.Next(end), ShuffleUtils.random.Next(end));
            }
        }
    }

    public class Sorted : IShuffle
    {
        public string Title => "Sorted";

        public void ShuffleArray<T>(T[] array, int start, int end, IComparer<T> cmp)
        {
            Array.Sort(array, start, end, cmp);
        }
    }

    public class Naive : IShuffle
    {
        public string Title => "Naive random shuffle";

        public void ShuffleArray<T>(T[] array, int start, int end, IComparer<T> cmp)
        {
            for (int i = 0; i < end; i++)
                Sort.Swap(array, i, ShuffleUtils.random.Next(end));
        }
    }

    public class ShuffledTail : IShuffle //TODO: fix
    {
        public string Title => "Shuffled tail";

        public void ShuffleArray<T>(T[] array, int start, int end, IComparer<T> cmp)
        {
            ShuffleUtils.ShuffledTail(array, start, end, 7);
        }
    }

    public class ShuffledHead : IShuffle //TODO: fix
    {
        public string Title => "Shuffled head";

        public void ShuffleArray<T>(T[] array, int start, int end, IComparer<T> cmp)
        {
            ShuffleUtils.ShuffledHead(array, start, end, 7);
        }
    }

    public class ShuffledBothSides : IShuffle //TODO: fix
    {
        public string Title => "Shuffled both sides";

        public void ShuffleArray<T>(T[] array, int start, int end, IComparer<T> cmp)
        {
            ShuffleUtils.ShuffledHead(array, start, end, 7);
            ShuffleUtils.ShuffledTail(array, start, end, 7);
        }
    }

    public class ShiftedElement : IShuffle
    {
        public string Title => "Shifted element";

        public void ShuffleArray<T>(T[] array, int start, int end, IComparer<T> cmp)
        {
            int begin = ShuffleUtils.random.Next(end);
            int dest = ShuffleUtils.random.Next(end);
            if (dest < start)
            {
                Sort.IndexedRotate(array, dest, start, start + 1);
            }
            else
            {
                Sort.IndexedRotate(array, start, start + 1, dest);
            }
        }
    }

    public class RandomSwap : IShuffle
    {
        public string Title => "Random swap";

        public void ShuffleArray<T>(T[] array, int start, int end, IComparer<T> cmp)
        {
            int begin = ShuffleUtils.random.Next(end);
            int dest = ShuffleUtils.random.Next(end);
            Sort.Swap(array, begin, dest);
        }
    }

    public class RandomReversal : IShuffle
    {
        public string Title => "Random reversal";

        public void ShuffleArray<T>(T[] array, int start, int end, IComparer<T> cmp)
        {
            int begin = ShuffleUtils.random.Next(end);
            int dest = ShuffleUtils.random.Next(end);
            Sort.Reversal(array, begin, dest);
        }
    }

    public class RandomPull : IShuffle
    {
        public string Title => "Random pull";

        public void ShuffleArray<T>(T[] array, int start, int end, IComparer<T> cmp)
        {
            int begin = ShuffleUtils.random.Next(end);
            int dest = ShuffleUtils.random.Next(end);
            Sort.MultiSwap(array, begin, dest);
        }
    }

    public class RandomBlockSwap : IShuffle
    {
        public string Title => "Random blockswap";

        public void ShuffleArray<T>(T[] array, int start, int end, IComparer<T> cmp)
        {
            int len = ShuffleUtils.random.Next(end);
            int begin = ShuffleUtils.random.Next(end - len);
            int dest = ShuffleUtils.random.Next(end - len);
            bool dir = ShuffleUtils.random.NextDouble() >= 0.5;

            if (begin + len >= end)
            { // add / 2 to end on the int declarers for start dest and len
                dir = false;
            }
            if (dest + len >= end)
            {
                dir = false;
            }

            if (begin + len <= 0)
            {
                dir = true;
            }
            if (dest + len <= 0)
            {
                dir = true;
            }

            if (dir)
                Sort.BlockSwap(array, begin, dest, len);
            else
                Sort.BlockSwapBackwards(array, begin, dest, len);
        }
    }

    

    public class RandomRotation : IShuffle
    {
        public string Title => "Random rotation";

        public void ShuffleArray<T>(T[] array, int start, int end, IComparer<T> cmp)
        {
            int a = ShuffleUtils.random.Next(end);
            int b = ShuffleUtils.random.Next(end);
            int begin = Math.Min(a, b);
            int last = Math.Max(a, b) + 1;
            int mid = ShuffleUtils.random.Next(begin, last);
            Sort.IndexedRotate(array, start, mid, end);
        }
    }

    public class SwappedEnds : IShuffle
    {
        public string Title => "Swapped ends";

        public void ShuffleArray<T>(T[] array, int start, int end, IComparer<T> cmp)
        {
            Sort.Swap(array, start + 1, end - 1);
        }
    }

    public class SwappedPairs : IShuffle
    {
        public string Title => "Swapped pairs";

        public void ShuffleArray<T>(T[] array, int start, int end, IComparer<T> cmp)
        {
            for (int i = start + 1; i < end; i += 2)
            {
                Sort.Swap(array, i - 1, i);
            }
        }
    }

    public class Noisy : IShuffle
    {
        public string Title => "Noisy";

        public void ShuffleArray<T>(T[] array, int start, int end, IComparer<T> cmp)
        {
            int i;
            int size = Math.Max(4, (int)(Math.Sqrt(end) / 2));
            for (i = 0; i + size <= end; i += ShuffleUtils.random.Next(size - 1) + 1)
                ShuffleUtils.Shuffle(array, i, i + size);
            ShuffleUtils.Shuffle(array, i, end);
        }
    }

    public class ScrambledOdds : IShuffle
    {
        public string Title => "Scrambled odds";

        public void ShuffleArray<T>(T[] array, int start, int end, IComparer<T> cmp)
        {
            for (int i = 1; i < end; i += 2)
            {
                int randomIndex = ((ShuffleUtils.random.Next(end - i) / 2) * 2) + i;
                Sort.Swap(array, i, randomIndex);
            }
        }
    }

    public class FinalMerge : IShuffle
    {
        public string Title => "Final merge pass";

        public void ShuffleArray<T>(T[] array, int start, int end, IComparer<T> cmp)
        {
            ShuffleUtils.FinalMerge(array, start, end, 2);
        }
    }

    public class RealFinalMerge : IShuffle //TODO: replace arraysort
    {
        public string Title => "Real final merge pass";

        public void ShuffleArray<T>(T[] array, int start, int end, IComparer<T> cmp)
        {
            if (MainWindow.ExtraShuffles)
                ShuffleUtils.Shuffle(array, start, end);
            Array.Sort(array, start, end / 2, cmp);
            Array.Sort(array, end / 2 + 1, end, cmp);
        }
    }

    public class ShuffledSecondHalf : IShuffle
    {
        public string Title => "Shuffled second half";

        public void ShuffleArray<T>(T[] array, int start, int end, IComparer<T> cmp)
        {
            ShuffleUtils.ShuffledTail(array, start, end, 2);
        }
    }

    public class ShuffledFirstHalf : IShuffle
    {
        public string Title => "Shuffled first half";

        public void ShuffleArray<T>(T[] array, int start, int end, IComparer<T> cmp)
        {
            ShuffleUtils.ShuffledHead(array, start, end, 2);
        }
    }

    public class Partitioned : IShuffle
    {
        public string Title => "Partitioned";

        public void ShuffleArray<T>(T[] array, int start, int end, IComparer<T> cmp)
        {
            if (MainWindow.ExtraShuffles)
                Array.Sort(array, start, end, cmp);
            ShuffleUtils.Shuffle(array, start, end / 2);
            ShuffleUtils.Shuffle(array, end / 2, end);
        }
    }

    public class QuickPartitioned : IShuffle
    {
        public string Title => "Quick partitioned";

        public void ShuffleArray<T>(T[] array, int start, int end, IComparer<T> cmp)
        {
            if (MainWindow.ExtraShuffles)
                Array.Sort(array, start, end, cmp);
            for (int i = 1; i < end - 1; i *= 2)
            {
                if (i * 2 < end)
                    ShuffleUtils.Shuffle(array, i, i * 2);
                else
                    ShuffleUtils.Shuffle(array, i, end - 1);
            }
        }
    }

    public class IncreasingReversals : IShuffle
    {
        public string Title => "Increasing reversals";

        public void ShuffleArray<T>(T[] array, int start, int end, IComparer<T> cmp)
        {
            if (MainWindow.ExtraShuffles)
                Array.Sort(array, start, end, cmp);
            for (int i = 1; i < end - 1; i *= 2)
            {
                if (i * 2 < end)
                    Sort.Reversal(array, i, i * 2);
                else
                    Sort.Reversal(array, i, end - 1);
            }
        }
    }

    public class IncreasingSwaps : IShuffle
    {
        public string Title => "Increasing swaps";

        public void ShuffleArray<T>(T[] array, int start, int end, IComparer<T> cmp)
        {
            if (MainWindow.ExtraShuffles)
                Array.Sort(array, start, end, cmp);
            for (int i = 1; i < end - 1; i *= 2)
            {
                if (i * 2 < end)
                    Sort.Swap(array, i, i * 2);
                else
                    Sort.Swap(array, i, end - 1);
            }
        }
    }

    public class FirstStrangePass : IShuffle
    {
        public string Title => "First strangesort pass";

        public void ShuffleArray<T>(T[] array, int start, int end, IComparer<T> cmp)
        {
            if (MainWindow.ExtraShuffles)
                ShuffleUtils.Shuffle(array, start, end);
            int offset = 1;
            double mult = start;
            int bound = start;
            while (offset != end)
            {
                mult = 1;
                bound = 1;
                while (offset + mult <= end)
                {
                    if (Sort.CompareIndices(array, (int)(offset + mult / 2) - 1, (int)(offset + mult) - 1, cmp) > 0)
                    {
                        Sort.Swap(array, (int)(offset + mult / 2) - 1, (int)(offset + mult) - 1);
                        if (mult == 1 / 2)
                        {
                            bound *= 2;
                            mult = bound;
                        }
                        else
                        {
                            mult /= 2;
                        }
                    }
                    else
                    {
                        bound *= 2;
                        mult = bound;
                    }
                }
                offset++;
            }
        }
    }

    public class Sawtooth : IShuffle
    {
        public string Title => "Sawtooth";

        public void ShuffleArray<T>(T[] array, int start, int end, IComparer<T> cmp)
        {
            ShuffleUtils.FinalMerge(array, start, end, 4);
        }
    }

    public class Organ : IShuffle
    {
        public string Title => "Pipe organ";

        public void ShuffleArray<T>(T[] array, int start, int end, IComparer<T> cmp)
        {
            ShuffleUtils.PipeOrgan(array, start, end);
        }
    }

    public class FinalBitonic : IShuffle
    {
        public string Title => "Inverted pipe organ";

        public void ShuffleArray<T>(T[] array, int start, int end, IComparer<T> cmp)
        {
            Array.Reverse(array, start, end);
            ShuffleUtils.PipeOrgan(array, start, end);
        }
    }

    public class PenultimateBitonic : IShuffle
    {
        public string Title => "Penultimate bitonic pass";

        public void ShuffleArray<T>(T[] array, int start, int end, IComparer<T> cmp)
        {
            ShuffleUtils.FinalMerge(array, start, end, 2);
            for (int i = 0, j = end - 1; i < end / 2; i++, j--)
            {
                if (Sort.CompareIndices(array, i, j, cmp) > 0)
                    Sort.Swap(array, i, j);
            }
        }
    }

    public class TriangleWave : IShuffle
    {
        public string Title => "Triangle wave";

        public void ShuffleArray<T>(T[] array, int start, int end, IComparer<T> cmp)
        {
            ShuffleUtils.PipeOrgan(array, start, end);
            ShuffleUtils.PartRotation(array, start, end, 4);
        }
    }

    public class ReverseFinalMerge : IShuffle
    {
        public string Title => "Reverse final merge";

        public void ShuffleArray<T>(T[] array, int start, int end, IComparer<T> cmp)
        {
            ShuffleUtils.FinalMerge(array, start, end, 2);
            Array.Reverse(array, start, end);
        }
    }

    public class ReverseSawtooth : IShuffle
    {
        public string Title => "Reverse sawtooth";

        public void ShuffleArray<T>(T[] array, int start, int end, IComparer<T> cmp)
        {
            ShuffleUtils.FinalMerge(array, start, end, 4);
            Array.Reverse(array, start, end);
        }
    }

    public class Interlaced : IShuffle
    {
        public string Title => "Interlaced";

        public void ShuffleArray<T>(T[] array, int start, int end, IComparer<T> cmp)
        {
            T[] referenceArray = new T[end];
            for (int i = 0; i < end; i++)
            {
                Sort.Write(referenceArray, i, array[i]);
            }

            int leftIndex = 1;
            int rightIndex = end - 1;

            for (int i = 1; i < end; i++)
            {
                if (i % 2 == 0)
                {
                    Sort.Write(array, i, referenceArray[leftIndex++]);
                }
                else
                {
                    Sort.Write(array, i, referenceArray[rightIndex--]);
                }
            }
        }
    }

    public class DoubleLayered : IShuffle
    {
        public string Title => "Double-layered";

        public void ShuffleArray<T>(T[] array, int start, int end, IComparer<T> cmp)
        {
            for (int i = 0; i < end / 2; i += 2)
            {
                Sort.Swap(array, i, end - i - 1);
            }
        }
    }

    public class DiamondInput : IShuffle
    {
        public string Title => "Diamond input";

        public void ShuffleArray<T>(T[] array, int start, int end, IComparer<T> cmp)
        {
            for (int i = 0; i < end / 2; i += 2)
            {
                Sort.Swap(array, i, end - i - 1);
            }
            ShuffleUtils.PartRotation(array, start, end, 2);
        }
    }

    public class FinalRadix : IShuffle
    {
        public string Title => "Final radix pass";

        public void ShuffleArray<T>(T[] array, int start, int end, IComparer<T> cmp)
        {
            end -= end % 2;
            int mid = end / 2;
            T[] temp = new T[mid];

            for (int i = 0; i < mid; i++)
                Sort.Write(temp, i, array[i]);

            for (int i = mid, j = 0; i < end; i++, j += 2)
            {
                Sort.Write(array, j, array[i]);
                Sort.Write(array, j + 1, temp[i - mid]);
            }
        }
    }

    public class RealFinalRadix : IShuffle
    {
        public string Title => "Real final radix pass";

        public void ShuffleArray<T>(T[] array, int start, int end, IComparer<T> cmp)
        {
            throw new NotImplementedException();
        }
    }

    public class RecFinalRadix : IShuffle
    {
        public string Title => "Recursive final radix";

        public void ShuffleArray<T>(T[] array, int start, int end, IComparer<T> cmp)
        {
            T[] temp = new T[end];
            WeaveRec(array, start, end, 1, temp);
        }

        public void WeaveRec<T>(T[] array, int pos, int length, int gap, T[] temp)
        {
            if (length < 2)
                return;
            int mod2 = length % 2;
            length -= mod2;
            int mid = length / 2;

            for (int i = pos, j = 0; i < pos + gap * mid; i += gap, j++)
            {
                Sort.Write(temp, j, array[i]);
            }
            for (int i = pos + gap * mid, j = pos, k = 0; i < pos + gap * length; i += gap, j += 2 * gap, k++)
            {
                Sort.Write(array, j, array[i]);
                Sort.Write(array, j + gap, temp[k]);
            }
            WeaveRec(array, pos, mid + mod2, 2 * gap, temp);
            WeaveRec(array, pos + gap, mid, 2 * gap, temp);
        }
    }

    public class HalfRotation : IShuffle
    {
        public string Title => "Half rotation";

        public void ShuffleArray<T>(T[] array, int start, int end, IComparer<T> cmp)
        {
            ShuffleUtils.PartRotation(array, start, end, 2);
        }
    }

    public class QuarterRotation : IShuffle
    {
        public string Title => "Quarter rotation";

        public void ShuffleArray<T>(T[] array, int start, int end, IComparer<T> cmp)
        {
            ShuffleUtils.PartRotation(array, start, end, 4);
        }
    }

    public class HalfReversed : IShuffle
    {
        public string Title => "Half-reversed";

        public void ShuffleArray<T>(T[] array, int start, int end, IComparer<T> cmp)
        {
            Sort.Reversal(array, 0, end - 1);
            Sort.Reversal(array, end / 4, (3 * end + 3) / 4 - 1);
        }
    }

    // credit to sam walko/anon

    public class Subarray
    {
        public int start;
        public int end;

        public Subarray(int start, int end)
        {
            this.start = start;
            this.end = end;
        }
    }

    public class BSTTraversal : IShuffle
    {
        public string Title => "Binary search tree";

        public void ShuffleArray<T>(T[] array, int start, int end, IComparer<T> cmp)
        {
            T[] temp = new T[end];
            Array.Copy(array, start, temp, 0, end);
            Queue<Subarray> q = new();
            q.Enqueue(new(0, end));
            int i = 0;

            while (q.Count != 0)
            {
                Subarray sub = q.Dequeue();
                if (sub.start != sub.end)
                {
                    int mid = (sub.start + sub.end) / 2;

                    Sort.Write(array, i, temp[mid]);
                    i++;
                    q.Enqueue(new(sub.start, mid));
                    q.Enqueue(new(mid + 1, sub.end));
                }
            }
        }
    }

    public class InvBST : IShuffle
    {
        public string Title => "Inverted binary search tree";

        public void ShuffleArray<T>(T[] array, int start, int end, IComparer<T> cmp)
        {
            int[] temp = new int[end];

            Queue<Subarray> q = new();
            q.Enqueue(new(0, end));
            int i = 0;

            while (q.Count != 0)
            {
                Subarray sub = q.Dequeue();
                if (sub.start != sub.end)
                {
                    int mid = (sub.start + sub.end) / 2;
                    Sort.Write(temp, i, mid);
                    i++;
                    q.Enqueue(new Subarray(sub.start, mid));
                    q.Enqueue(new Subarray(mid + 1, sub.end));
                }
            }
            T[] temp2 = new T[end];
            Array.Copy(array, start, temp2, 0, end);
            for (i = 0; i < end; i++)
                Sort.Write(array, temp[i], temp2[i]);
        }
    }

    public class LogPile : IShuffle
    {
        public string Title => "Logpile";

        public void ShuffleArray<T>(T[] array, int start, int end, IComparer<T> cmp)
        {
            T[] temp = new T[end];
            for (int i = 0; i < end; i++)
                Sort.Write(temp, i, array[i]);

            //writes.write(array, 0, 0);
            for (int i = 1; i < end; i++)
            {
                int log = (int)(Math.Log(i) / Math.Log(2));
                int power = (int)Math.Pow(2, log);
                T value = temp[2 * (i - power) + 1];
                Sort.Write(array, i, value);
            }
        }
    }

    public class PairwisePass : IShuffle
    {
        public string Title => "Final pairwise pass";

        public void ShuffleArray<T>(T[] array, int start, int end, IComparer<T> cmp)
        {
            throw new NotImplementedException();
        }
    }

    public class Pairwised : IShuffle
    {
        public string Title => "Pairwised";

        public void ShuffleArray<T>(T[] array, int start, int end, IComparer<T> cmp)
        {
            int a = start + 1;
            int b = start;
            int c = 0;
            if (MainWindow.ExtraShuffles)
                ShuffleUtils.Shuffle(array, start, end);
            while (a < end)
            {
                b = a;
                c = 0;
                while (b < end)
                {
                    if (Sort.CompareIndices(array, b - a, b, cmp) == 1)
                    {
                        Sort.Swap(array, b - a, b);
                    }
                    c = (c + 1) % a;
                    b++;
                    if (c == 0)
                    {
                        b += a;
                    }
                }
                a *= 2;
            }
        }
    }

    public static class ShuffleUtils
    {
        public readonly static Random random = new();

        public static void ShuffledHead<T>(T[] array, int start, int end, double frac)
        {
            T[] aux = new T[end];
            int i = end - 1;
            int j = end - 1;
            int k = start;
            while (i >= 0)
            {
                if (random.Next() < 1 / frac)
                    Sort.Write(aux, k++, array[i--]);
                else
                    Sort.Write(array, j--, array[i--]);
            }
            Array.Copy(aux, 0, array, start, k);
            if (MainWindow.ExtraShuffles)
                Shuffle(array, start, j);
        }

        public static void ShuffledTail<T>(T[] array, int start, int end, double frac)
        {
            T[] aux = new T[end];
            int i = start;
            int j = start;
            int k = start;
            while (i < end)
            {
                if (random.Next() < 1 / frac)
                    Sort.Write(aux, k++, array[i++]);
                else
                    Sort.Write(array, j++, array[i++]);
            }
            Array.Copy(aux, 0, array, j, k);
            if (MainWindow.ExtraShuffles)
                Shuffle(array, j, end);
        }

        public static void FinalMerge<T>(T[] array, int start, int end, int count)
        {

            int k = start;
            T[] temp = new T[end];

            for (int j = 0; j < count; j++)
                for (int i = j; i < end; i += count)
                {
                    Sort.Write(temp, k++, array[i]);
                }
            for (int i = start; i < end; i++)
                Sort.Write(array, i, temp[i]);
        }

        public static void PipeOrgan<T>(T[] array, int start, int end) //TODO: fix indexing
        {
            T[] temp = new T[end];
            end--;
            for (int i = start, j = start; i < end; i += 2, j++)
            {
                Sort.Write(temp, j, array[i]);
            }
            for (int i = start + 1, j = end; i < end; i += 2, --j)
            {
                Sort.Write(temp, j, array[i]);
            }
            for (int i = 0; i < end + 1; i++)
            {
                Sort.Write(array, i, temp[i]);
            }
        }
        public static void PartRotation<T>(T[] array, int start, int end, int div)
        {
            int a = start;
            int m = (end + 1) / div;

            if (end % 2 == 0)
                while (m < end)
                    Sort.Swap(array, a++, m++);
            else
            {
                T temp = array[a];
                while (m < end)
                {
                    Sort.Write(array, a++, array[m]);
                    Sort.Write(array, m++, array[a]);
                }
                Sort.Write(array, a, temp);
            }
        }
        public static void Shuffle<T>(T[] array, int start, int end)
        {
            for (int i = start; i < end; i++)
            {
                int randomIndex = random.Next(end - i) + i;
                Sort.Swap(array, i, randomIndex);
            }
        }
    }
}
