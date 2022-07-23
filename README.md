# AlgoBenchmark
 WPF/XAML program for benchmarking the performance, comparisons, and data type accesses of sorting algorithms on all input sizes of a certain range

The benchmarks are shown in graphs using ScottPlot's signal plot, with the X axis being the array sizes run, and the Y axis being the number of the respective metric occuring per array size. Lower scores mean better results, because it's the number of comparisons, type accesses, and elapsed time in ticks.

Note that all the algorithms used are generic, meaning not modified to facilitate variables, methods, properties, etc, of the program just to run or chart/visualize them. All that's special is the data type (ArrayInt instead of int) and the IComparer used.

![](example.png)
