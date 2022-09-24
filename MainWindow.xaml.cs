using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;

namespace Sorting_algorithm_benchmark_grapher
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public class Comparer : IComparer<ArrayInt>
    {
        public int Compare(ArrayInt x, ArrayInt y)
        {
            return x < y ? -1 : x > y ? 1 : 0;
        }
    }

    public class WrongCategoryException : Exception
    {
        public WrongCategoryException()
        {
        }

        public WrongCategoryException(string message)
            : base(message)
        {
        }

        public WrongCategoryException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
    public partial class MainWindow : Window
    {


        public ArrayInt[] Arr;

        public static readonly Comparer cmp = new();
        private readonly List<IDistribution> distribs = new();
        private readonly List<string> distnames = new();
        private readonly List<IShuffle> shuffles = new();
        private readonly List<string> shufnames = new();
        private readonly List<ISorter> genrcsorts = new();
        private readonly List<string> genrcsortnames = new();
        private readonly List<IIntegerSorter> intsorts = new();
        private readonly List<string> intsortnames = new();
        private static readonly ArrayInt[][] SortArr = new ArrayInt[2048][];
        private double[] Comps = new double[SortArr.Length];
        private double[] Gets = new double[SortArr.Length];
        private double[] Time = new double[SortArr.Length];
        public static bool ExtraShuffles { get; private set; }

        public int BoxText { get; set; }

        public bool resetdist;

        private static volatile int Compares;
        private static volatile int Get;

        public static void ResetStatistics()
        {
            Compares = 0;
            Get = 0;
        }

        public static int GetCompares()
        {
            return Compares;
        }

        public static int GetGets()
        {
            return Get;
        }

        public static void AddComparison()
        {
            Compares++;
        }

        public static void AddGets()
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
            IEnumerable<Type>? disttypes = Assembly.GetExecutingAssembly().GetTypes().Where(t => t.IsClass && !t.IsAbstract && !t.IsInterface && t.IsAssignableTo(typeof(IDistribution)));
            foreach (Type? it in disttypes)
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

            IEnumerable<Type>? shuftypes = Assembly.GetExecutingAssembly().GetTypes().Where(t => t.IsClass && !t.IsAbstract && !t.IsInterface && t.IsAssignableTo(typeof(IShuffle)));
            foreach (Type? it in shuftypes)
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

            IEnumerable<Type>? sorttypes = Assembly.GetExecutingAssembly().GetTypes().Where(t => t.IsClass && !t.IsAbstract && !t.IsInterface && t.IsAssignableTo(typeof(ISorter)));
            foreach (Type? it in sorttypes)
            {
                ISorter? i = (ISorter?)Activator.CreateInstance(it);
                if (i != null)
                {
                    genrcsorts.Add(i);
                    genrcsortnames.Add(i.Title);
                    Debug.WriteLine(i.Title);
                }
            }
            GenericSorts.ItemsSource = genrcsortnames;

            IEnumerable<Type>? intsorttypes = Assembly.GetExecutingAssembly().GetTypes().Where(t => t.IsClass && !t.IsAbstract && !t.IsInterface && t.IsAssignableTo(typeof(IIntegerSorter)));
            foreach (Type? it in intsorttypes)
            {
                IIntegerSorter? i = (IIntegerSorter?)Activator.CreateInstance(it);
                if (i != null)
                {
                    intsorts.Add(i);
                    intsortnames.Add(i.Title);
                    Debug.WriteLine(i.Title);
                }
            }
            IntegerSorts.ItemsSource = intsortnames;

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
            GenericSorts.IsEnabled = false;
            IntegerSorts.IsEnabled = false;
            int distindex = DistribBox.SelectedIndex;
            int shufindex = ShuffleBox.SelectedIndex;
            int sortindex = Sorts.SelectedIndex == 0 ? GenericSorts.SelectedIndex : IntegerSorts.SelectedIndex;

            if (Parameter.IsEnabled == true && int.TryParse(Parameter.Text, out _) == false)
            {
                _ = MessageBox.Show("The input parameter is not a number.");
            }
            else
            {
                int arraysize = 5;
                switch (genrcsorts[sortindex].Time)
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
                    {
                        distribs[distindex].InitializeArray(SortArr[i]);
                    }

                    if (ShuffleBox != null)
                    {
                        shuffles[shufindex].ShuffleArray(SortArr[i], 0, SortArr[i].Length, cmp);
                    }

                    ResetStatistics();
                    Stopwatch sw = new();
                    sw.Start();
                    if (Sorts.SelectedIndex == 0)
                    {
                        genrcsorts[sortindex].RunSort(SortArr[i], SortArr[i].Length, BoxText, cmp);
                    }
                    else
                    {
                        intsorts[sortindex].RunSort(SortArr[i], SortArr[i].Length, BoxText, cmp);
                    }

                    sw.Stop();
                    Comps[i] = GetCompares();
                    Gets[i] = GetGets();
                    Time[i] = sw.ElapsedTicks;
                }

            }
            CompareGraph.Plot.Clear();
            GetsGraph.Plot.Clear();
            TimeGraph.Plot.Clear();

            _ = CompareGraph.Plot.AddSignal(Comps);
            _ = GetsGraph.Plot.AddSignal(Gets);
            _ = TimeGraph.Plot.AddSignal(Time);

            CompareGraph.Render();
            GetsGraph.Render();
            TimeGraph.Render();

            SizeSlider.IsEnabled = true;
            if (DistribBox != null)
            {
                DistribBox.IsEnabled = true;
            }

            SortButton.IsEnabled = true;
            GenericSorts.IsEnabled = true;
            IntegerSorts.IsEnabled = true;
        }

        public void UpdateArray()
        {
            Display.Plot.Clear();
            double[] graphArr = new double[Arr.Length];
            graphArr = Arr.Select(x => (double)x).ToArray();
            _ = Display.Plot.AddSignal(graphArr);
            Display.Render();
        }

        public void Initialize() // distribution changed
        {
            Arr = new ArrayInt[SizeSlider != null ? (int)SizeSlider.Value : 256];
            if (DistribBox != null)
            {
                distribs[DistribBox.SelectedIndex].InitializeArray(Arr);
            }

            UpdateArray();
        }

        public void Shuffle()
        {
            if (ShuffleBox != null)
            {
                shuffles[ShuffleBox.SelectedIndex].ShuffleArray(Arr, 0, Arr.Length, cmp);
            }

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
            {
                Initialize();
            }

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

        private void GenericSorts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Sorts.SelectedIndex == 0)
            {
                if (genrcsorts[GenericSorts.SelectedIndex].Message != "")
                {
                    Parameter.IsEnabled = true;
                    MessageText.Text = genrcsorts[GenericSorts.SelectedIndex].Message;
                }
                else
                {
                    Parameter.IsEnabled = false;
                    MessageText.Text = "";
                }
            }
        }

        private void IntegerSorts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Sorts.SelectedIndex == 1)
            {
                if (intsorts[IntegerSorts.SelectedIndex].Message != "")
                {
                    Parameter.IsEnabled = true;
                    MessageText.Text = intsorts[IntegerSorts.SelectedIndex].Message;
                }
                else
                {
                    Parameter.IsEnabled = false;
                    MessageText.Text = "";
                }
            }
        }
    }
}
