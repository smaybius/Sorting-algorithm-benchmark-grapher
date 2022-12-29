using System;
using System.Collections.Generic;

namespace Sorting_algorithm_benchmark_grapher.Sorts
{
    /*
 *
MIT License
Copyright (c) 2021 aphitorite
Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:
The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.
THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
 *
 */
    internal class ClassicTournamentSorter<T>
    {
        private readonly IComparer<T> cmp;
        private readonly T[] array;
        private readonly T[] tmp;
        private int[] tree;
        public ClassicTournamentSorter(T[] array, int length, IComparer<T> cmpr)
        {
            this.array = array;
            cmp = cmpr;
            tree = new int[2]; // nullability fix
            BuildTree(length);
            tmp = new T[length];

            tmp[0] = Peek();

            for (int i = 1; i < length; i++)
            {
                T val = FindNext();

                tmp[i] = val;
            }
            Array.Copy(tmp, 0, array, 0, length);
        }


        private int size;

        private static int CeilPow2(int n)
        {
            int r = 1;
            while (r < n)
            {
                r *= 2;
            }

            return r;
        }

        private bool TreeCompare(int a, int b)
        {
            return cmp.Compare(array[tree[a]], array[tree[b]]) <= 0;
        }

        private void BuildTree(int n)
        {
            size = CeilPow2(n) - 1;
            int mod = n & 1;
            int treeSize = n + size + mod;

            tree = new int[treeSize];

            for (int i = 0; i < treeSize; i++)
            {
                tree[i] = -1;
            }

            for (int i = size; i < treeSize - mod; i++)
            {
                tree[i] = i - size;
            }

            for (int i, j = size, k = treeSize - mod; j > 0; j /= 2, k /= 2)
            {
                for (i = j; i + 1 < k; i += 2)
                {
                    int val = TreeCompare(i, i + 1) ? tree[i] : tree[i + 1];

                    tree[i / 2] = val;
                }
                if (i < k)
                {
                    tree[i / 2] = tree[i];
                }
            }
        }

        private T Peek()
        {
            return array[tree[0]];
        }

        private T FindNext()
        {
            int root = tree[0] + size;

            for (int i = root; i > 0; i = (i - 1) / 2)
            {
                tree[i] = -1;
            }

            for (int i = root; i > 0;)
            {
                int j = i + ((i & 1) << 1) - 1;

                int c1 = tree[i] >> 31;
                int c2 = tree[j] >> 31;

                int nVal = (c1 & ((c2 & -1) + (~c2 & tree[j]))) + (~c1 & ((c2 & tree[i]) + (~c2 & -2)));

                if (nVal == -2)
                {
                    nVal = i < j ? TreeCompare(i, j) ? tree[i] : tree[j] : TreeCompare(j, i) ? tree[j] : tree[i];
                }

                i = (i - 1) / 2;
                if (nVal != -1)
                {
                    tree[i] = nVal;
                }
            }

            return Peek();
        }
    }
}
