using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Sorting_algorithm_benchmark_grapher
{
    public class ArrayInt
    {

        private int _value;

        public int Value
        {
            get
            {
                MainWindow.mw?.AddGets();
                return _value;
            }
            private set
            {
                _value = value;
            }
        }

        public ArrayInt(int val)
        {
            Value = val;
            MainWindow.mw?.AddGets();
        }

        public static implicit operator int(ArrayInt value)
        {
            return value != null ? value.Value : 0;
        }

        public static implicit operator ArrayInt(int value)
        {
            return new ArrayInt(value);
        }
    }
}
