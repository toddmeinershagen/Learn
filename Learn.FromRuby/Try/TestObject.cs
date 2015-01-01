using System;

namespace Learn.FromRuby.Try
{
    public class TestObject
    {
        private readonly TestObject _child = null;
        public int Value { get; set; }

        public TestObject(int value)
            : this(value, null)
        {
        }

        public TestObject(int value, TestObject child)
        {
            Value = value;
            _child = child;
        }

        public TestObject GetChild(bool throwException = false)
        {
            if (throwException)
                throw new ArgumentException();

            return _child;
        }

        public int GetValue(bool throwException = false)
        {
            if (throwException)
                throw new ArgumentException();

            return Value;
        }
    }
}