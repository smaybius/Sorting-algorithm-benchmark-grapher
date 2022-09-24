using System;
using System.Collections.Generic;

namespace Sorting_algorithm_benchmark_grapher.Sorts
{
    /*
 *
  Copyright (c) rosettacode.org.
  Permission is granted to copy, distribute and/or modify this document
  under the terms of the GNU Free Documentation License, Version 1.2
  or any later version published by the Free Software Foundation;
  with no Invariant Sections, no Front-Cover Texts, and no Back-Cover
  Texts.  A copy of the license is included in the section entitled "GNU
  Free Documentation License".
 *
 */
    internal sealed class Pile<T> : Stack<T>, IComparable<Pile<T>>
    {
        private readonly IComparer<T> cmp;
        public Pile(IComparer<T> cmp)
        {
            this.cmp = cmp;
        }
        public int Compare(Pile<T> y)
        {
            return cmp.Compare(Peek(), y.Peek());
        }
        public int CompareTo(Pile<T> y)
        {
            return cmp.Compare(Peek(), y.Peek());
        }
    }
    internal class PatienceSort : ISorter
    {
        public string Title => "Patience sort";

        public string Message => "";

        public string Category => "Insertion sorts";

        public Complexity Time => Complexity.GOOD;

        private static void BinarySearch<T>(List<Pile<T>> list, Pile<T> find)
        {
            int at = list.Count / 2;
            int change = list.Count / 4;

            while (list[at].Compare(find) != 0 && change > 0)
            {

                if (list[at].Compare(find) < 0)
                {
                    at += change;
                }
                else
                {
                    at -= change;
                }

                change /= 2;
            }
        }

        public void RunSort<T>(T[] array, int length, int parameter, IComparer<T> cmp)
        {
            List<Pile<T>> piles = new();

            // sort into piles
            for (int x = 0; x < length; x++)
            {
                Pile<T> newPile = new(cmp);

                newPile.Push(array[x]);

                int i = piles.BinarySearch(newPile);
                if (piles.Count != 0)
                {
                    BinarySearch(piles, newPile);
                }
                if (i < 0)
                {
                    i = ~i;
                }

                if (i != piles.Count)
                {
                    piles[i].Push(array[x]);
                }
                else
                {
                    piles.Add(newPile);
                }
            }


            // priority queue allows us to retrieve least pile efficiently
            Queue<Pile<T>> heap = new(piles);

            for (int c = 0; c < length; c++)
            {
                Pile<T> smallPile = heap.Dequeue();

                array[c] = smallPile.Pop();

                if (smallPile.Count != 0)
                {
                    heap.Enqueue(smallPile);
                }
            }
        }
    }
}
