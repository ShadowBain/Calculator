namespace CalculatorLib
{
    public class CalculatorMemory
    {
        private double _currentValue = 0d;

        public void StoreValue(double value)
        {
            _currentValue = value;
        }

        public void AddValue(double value)
        {
            _currentValue += value;
        }

        public void SubtractValue(double value)
        {
            _currentValue -= value;
        }

        public double Recall()
        {
            return _currentValue;
        }

        public void Clear()
        {
            _currentValue = 0d;
        }
    }
}
