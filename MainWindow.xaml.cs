﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Sorting_algorithm_benchmark_grapher
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public class Comparer : IComparer<ArrayInt>
    {
        public int Compare(ArrayInt? x, ArrayInt? y)
        {
            MainWindow.mw?.AddComparison();
            return x < y ? -1 : x > y ? 1 : 0;
        }
    }


    public partial class MainWindow : Window
    {


        public ArrayInt[] Arr;

        public static MainWindow mw;

        public readonly static Comparer cmp = new();
        readonly List<IDistribution> distribs = new();
        readonly List<string> distnames = new();

        readonly List<IShuffle> shuffles = new();
        readonly List<string> shufnames = new();

        readonly List<ISorter> sorts = new();
        readonly List<string> sortnames = new();
        static ArrayInt[][] SortArr = new ArrayInt[2048][];
        double[] Comps = new double[SortArr.Length];
        double[] Gets = new double[SortArr.Length];
        double[] Time = new double[SortArr.Length];
        public static bool ExtraShuffles { get; private set; }

        public double BoxText { get; set; }

        public bool resetdist;

        private volatile int Compares;
        private volatile int Get;

        public void ResetStatistics()
        {
            Compares = 0;
            Get = 0;
        }

        public int GetCompares()
        {
            return Compares;
        }

        public int GetGets()
        {
            return Get;
        }

        public void AddComparison()
        {
            Compares++;
        }

        public void AddGets()
        {
            Get++;
        }

        public MainWindow()
        {
            InitializeComponent();
            Display.Configuration.LeftClickDragPan = false;
            Display.Configuration.RightClickDragZoom = false;
            Display.Configuration.ScrollWheelZoom = false;
            Display.Configuration.MiddleClickDragZoom = false;
            Arr = new ArrayInt[SizeSlider != null ? (int)SizeSlider.Value : 256];
            var disttypes = Assembly.GetExecutingAssembly().GetTypes().Where(t => t.IsClass && !t.IsAbstract && !t.IsInterface && t.IsAssignableTo(typeof(IDistribution)));
            foreach (var it in disttypes)
            {
                IDistribution? i = (IDistribution?)Activator.CreateInstance(it);
                if (i != null)
                {
                    distribs.Add(i);
                    distnames.Add(i.Title);
                    Debug.WriteLine(i.Title);
                }
                
            }
            DistribBox.ItemsSource = distnames;

            var shuftypes = Assembly.GetExecutingAssembly().GetTypes().Where(t => t.IsClass && !t.IsAbstract && !t.IsInterface && t.IsAssignableTo(typeof(IShuffle)));
            foreach (var it in shuftypes)
            {
                IShuffle? i = (IShuffle?)Activator.CreateInstance(it);
                if (i != null)
                {
                    shuffles.Add(i);
                    shufnames.Add(i.Title);
                    Debug.WriteLine(i.Title);
                }
            }
            ShuffleBox.ItemsSource = shufnames;
            var sorttypes = Assembly.GetExecutingAssembly().GetTypes().Where(t => t.IsClass && !t.IsAbstract && !t.IsInterface && t.IsAssignableTo(typeof(ISorter)));
            foreach (var it in sorttypes)
            {
                ISorter? i = (ISorter?)Activator.CreateInstance(it);
                if (i != null)
                {
                    sorts.Add(i);
                    sortnames.Add(i.Title);
                    Debug.WriteLine(i.Title);
                }
            }
            AllSorts.ItemsSource = sortnames;
            mw = this;
            Initialize();
            Shuffle();
        }

        public void ComboBox_RequestBringIntoView(object sender, RequestBringIntoViewEventArgs e)
        {
            e.Handled = true;
        }

        public void Benchmark()
        {
            SizeSlider.IsEnabled = false;
            DistribBox.IsEnabled = false;
            SortButton.IsEnabled = false;
            AllSorts.IsEnabled = false;
            int distindex = DistribBox.SelectedIndex;
            int shufindex = ShuffleBox.SelectedIndex;
            int sortindex = AllSorts.SelectedIndex;
            if (Parameter.IsEnabled == true && double.TryParse(Parameter.Text, out _) == false)
            {
                MessageBox.Show("The input parameter is not a number.");
            }
            else
            {
                int arraysize = 5;
                switch (sorts[sortindex].Time)
                {
                    case Complexity.GOOD:
                        arraysize = SortArr.Length;
                        break;
                    case Complexity.QUADRATIC:
                        arraysize = 512;
                        break;
                    case Complexity.CUBIC:
                        arraysize = 64;
                        break;
                    case Complexity.EXPONENTIAL:
                        arraysize = 20;
                        break;
                    case Complexity.STUPID:
                        arraysize = 10;
                        break;

                }
                Comps = new double[arraysize];
                Gets = new double[arraysize];
                Time = new double[arraysize];
                for (int i = 3; i < arraysize; i++)
                {
                    
                    SortArr[i] = new ArrayInt[i];
                    if (DistribBox != null)
                        distribs[distindex].InitializeArray(SortArr[i]);
                    if (ShuffleBox != null)
                        shuffles[shufindex].ShuffleArray(SortArr[i], 0, SortArr[i].Length, cmp);

                    ResetStatistics();
                    Stopwatch sw = new();
                    sw.Start();
                    sorts[sortindex].RunSort(SortArr[i], SortArr[i].Length, BoxText, cmp);
                    sw.Stop();
                    Comps[i] = GetCompares();
                    Gets[i] = GetGets();
                    Time[i] = sw.ElapsedTicks;
                }
                
            }
            CompareGraph.Plot.Clear();
            GetsGraph.Plot.Clear();
            TimeGraph.Plot.Clear();

            CompareGraph.Plot.AddSignal(Comps);
            GetsGraph.Plot.AddSignal(Gets);
            TimeGraph.Plot.AddSignal(Time);

            CompareGraph.Render();
            GetsGraph.Render();
            TimeGraph.Render();

            SizeSlider.IsEnabled = true;
            if (DistribBox != null)
                DistribBox.IsEnabled = true;
            SortButton.IsEnabled = true;
            AllSorts.IsEnabled = true;
        }

        public void BenchmarkParallel(int i)
        {
            
        }

        public void UpdateArray()
        {
            Display.Plot.Clear();
            double[] graphArr = new double[Arr.Length];
            graphArr = Arr.Select(x => (double)x).ToArray();
            Display.Plot.AddSignal(graphArr);
            Display.Render();
        }

        public void Initialize() // distribution changed
        {
                Arr = new ArrayInt[SizeSlider != null ? (int)SizeSlider.Value : 256];
                if (DistribBox != null)
                distribs[DistribBox.SelectedIndex].InitializeArray(Arr);
                UpdateArray();
        }

        public void Shuffle()
        {
            if (ShuffleBox != null)
                shuffles[ShuffleBox.SelectedIndex].ShuffleArray(Arr, 0, Arr.Length, cmp);
            UpdateArray();
        }

        private void DistribBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Initialize();
        }

        private void SizeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Initialize();
            Shuffle();
        }

        private void SortButton_Click(object sender, RoutedEventArgs e)
        {
            Benchmark();
        }

        private void ShuffleCheck_Checked(object sender, RoutedEventArgs e)
        {
            ExtraShuffles = true;
        }

        private void ShuffleCheck_Unchecked(object sender, RoutedEventArgs e)
        {
            ExtraShuffles = false;
        }

        private void ShuffleBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (resetdist)
                Initialize();
            Shuffle();
        }

        private void ResetCheck_Checked(object sender, RoutedEventArgs e)
        {
            resetdist = true;
        }

        private void ResetCheck_Unchecked(object sender, RoutedEventArgs e)
        {
            resetdist = false;
        }

        private void ShuffleButton_Click(object sender, RoutedEventArgs e)
        {
            Shuffle();
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            Initialize();
        }

        private void AllSorts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sorts[AllSorts.SelectedIndex].Message != "")
            {
                Parameter.IsEnabled = true;
                Parameter.Text = sorts[AllSorts.SelectedIndex].Message;
            }
            else
            {
                Parameter.IsEnabled = false;
                Parameter.Text = "";
            }
        }
    }
}