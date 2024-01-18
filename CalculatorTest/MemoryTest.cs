using CalculatorLib;

namespace CalculatorTest
{
    [TestFixture]
    public class MemoryTest
    {
        private CalculatorMemory _memory;

        [SetUp]
        public void Setup()
        {
            _memory = new CalculatorMemory();
        }
    }
}
