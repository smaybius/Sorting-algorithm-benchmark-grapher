using System;
using System.Collections.Generic;

namespace Sorting_algorithm_benchmark_grapher.Sorts
{
    internal class TournamentSorter<T>
    {
        private readonly IComparer<T> cmp;
        private readonly int[] matches;
        private int tourney;
        public TournamentSorter(T[] array, int currentLength, IComparer<T> compr)
        {
            cmp = compr;
            matches = new int[6 * currentLength];
            tourney = Knockout(array, 0, currentLength - 1, 3);
            Sort(array, currentLength);
        }
        private int TourneyCompare(T a, T b)
        {

            return cmp.Compare(a, b);
        }

        private static bool IsPlayer(int i)
        {
            return i <= 0;
        }

        private void SetWinner(int root, int winner)
        {
            matches[root] = winner;
        }
        private void SetWinners(int root, int winners)
        {
            matches[root + 1] = winners;
        }
        private void SetLosers(int root, int losers)
        {
            matches[root + 2] = losers;
        }
        private int GetWinner(int root)
        {
            return matches[root];
        }
        private int GetWinners(int root)
        {
            return matches[root + 1];
        }
        private int GetLosers(int root)
        {
            return matches[root + 2];
        }
        private void SetMatch(int root, int winner, int winners, int losers)
        {
            SetWinner(root, winner);
            SetWinners(root, winners);
            SetLosers(root, losers);
        }

        private int GetPlayer(int i)
        {
            return i <= 0 ? Math.Abs(i) : GetWinner(i);
        }

        private T Pop(T[] arr)
        {
            T result = arr[GetPlayer(tourney)];
            tourney = IsPlayer(tourney) ? 0 : Rebuild(arr, tourney);
            return result;
        }

        private static int MakePlayer(int i)
        {
            return -i;
        }

        private int MakeMatch(T[] arr, int top, int bot, int root)
        {
            int top_w = GetPlayer(top);
            int bot_w = GetPlayer(bot);

            if (TourneyCompare(arr[top_w], arr[bot_w]) <= 0)
            {
                SetMatch(root, top_w, top, bot);
            }
            else
            {
                SetMatch(root, bot_w, bot, top);
            }

            return root;
        }

        private int Knockout(T[] arr, int i, int k, int root)
        {
            if (i == k)
            {
                return MakePlayer(i);
            }

            int j = (i + k) / 2;
            return MakeMatch(arr, Knockout(arr, i, j, 2 * root), Knockout(arr, j + 1, k, (2 * root) + 3), root);
        }

        private int Rebuild(T[] arr, int root)
        {
            if (IsPlayer(GetWinners(root)))
            {
                return GetLosers(root);
            }

            SetWinners(root, Rebuild(arr, GetWinners(root)));

            if (TourneyCompare(arr[GetPlayer(GetLosers(root))], arr[GetPlayer(GetWinners(root))]) < 0)
            {
                SetWinner(root, GetPlayer(GetLosers(root)));

                int temp = GetLosers(root);

                SetLosers(root, GetWinners(root));
                SetWinners(root, temp);
            }
            else
            {
                SetWinner(root, GetPlayer(GetWinners(root)));
            }

            return root;
        }

        private void Sort(T[] arr, int currentLen)
        {
            T[] copy = new T[currentLen];

            for (int i = 0; i < currentLen; i++)
            {
                T result = Pop(arr);
                copy[i] = result;
            }

            Array.Copy(copy, 0, arr, 0, currentLen);

        }
    }
}
