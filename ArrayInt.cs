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

        public static implicit operator int(ArrayInt value)
        {
            return value.Value;
        }

        public static implicit operator ArrayInt(int value)
        {
            return new ArrayInt(value);
        }

        public override bool Equals(object obj)
        {
            throw new System.NotImplementedException();
        }

        public override int GetHashCode()
        {
            throw new System.NotImplementedException();
        }
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
    }
}
