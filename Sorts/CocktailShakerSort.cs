using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sorting_algorithm_benchmark_grapher.Sorts
{
    internal class CocktailShakerSort : ISorter
    {
        public string Title => "Cocktail shaker sort";

        public string Message => "";

        public string Category => "Exchange sorts";

        public Complexity Time => Complexity.QUADRATIC;

        public void RunSort<T>(T[] array, int length, int parameter, IComparer<T> cmp)
        {
            for (int start = 0, end = length - 1; start < end;)
            {
                int consecSorted = 1;
                for (int i = start; i < end; i++)
                {
                    if (cmp.Compare(array[ i], array[ i + 1]) > 0)
                    {
                        Sort.Swap(array,  i,  i + 1);
                        consecSorted = 1;
                    }
                    else consecSorted++;
                }
                end -= consecSorted;

                consecSorted = 1;
                for (int i = end; i > start; i--)
                {
                    if (cmp.Compare(array[ i - 1], array[ i]) > 0)
                    {
                        Sort.Swap(array,  i - 1,  i);
                        consecSorted = 1;
                    }
                    else consecSorted++;
                }
                start += consecSorted;
            }
        }
    }
}
