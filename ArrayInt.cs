namespace Sorting_algorithm_benchmark_grapher
{
    public struct ArrayInt
    {

        private int _value;

        public int Value
        {
            get
            {
                MainWindow.AddGets();
                return _value;
            }
            private set => _value = value;
        }

        public ArrayInt(int val)
        {
            _value = val;
            Value = val;
            MainWindow.AddGets();
        }


        #region operators
        public static bool operator ==(ArrayInt left, ArrayInt right)
        {
            MainWindow.AddComparison();
            return left.Value == right.Value;
        }

        public static bool operator !=(ArrayInt left, ArrayInt right)
        {
            MainWindow.AddComparison();
            return left.Value != right.Value;
        }

        public static bool operator <(ArrayInt left, ArrayInt right)
        {
            MainWindow.AddComparison();
            return left.Value < right.Value;
        }

        public static bool operator >(ArrayInt left, ArrayInt right)
        {
            MainWindow.AddComparison();
            return left.Value > right.Value;
        }

        public static bool operator <=(ArrayInt left, ArrayInt right)
        {
            MainWindow.AddComparison();
            return left.Value <= right.Value;
        }

        public static bool operator >=(ArrayInt left, ArrayInt right)
        {
            MainWindow.AddComparison();
            return left.Value >= right.Value;
        }
        #endregion

        public static implicit operator int(ArrayInt value)
        {
            return value.Value;
        }

        public static implicit operator ArrayInt(int value)
        {
            return new ArrayInt(value);
        }
    }
}
