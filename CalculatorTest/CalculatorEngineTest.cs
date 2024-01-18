using CalculatorLib;

namespace CalculatorTest
{
    [TestFixture]
    public class Tests
    {
        [TestCase("2 + 2", 4)]
        [TestCase("2 + 2 + 2", 6)]
        [TestCase("2 + 2 + 2 + 2", 8)]
        [TestCase("2 + ", 2)]
        [TestCase("+ 2", 2)]
        [TestCase("4 - 2", 2)]
        [TestCase("2 - 2 - 2", -2)]
        [TestCase("2 - 2 - 2 - 2", -4)]
        [TestCase("4 -", 4)]
        [TestCase("- 4", 4)]
        [TestCase("2 * 2", 4)]
        [TestCase("2 * 2 * 2", 8)]
        [TestCase("2 * 2 * 2 * 2", 16)]
        [TestCase("2 * ", 2)]
        [TestCase(" * 2", 2)]
        [TestCase("4 / 2", 2)]
        [TestCase("4 / 2 / 2", 1)]
        [TestCase("4 / 2 / 2 / 2", 0.5)]
        [TestCase("4 / ", 4)]
        [TestCase("/ 4", 4)]
        [TestCase("4/2", 2)]
        [TestCase("(4/2)", 2)]
        public void Calculate_Simple(string input, double expected)
        {
            var engine = new CalculatorEngine();
            Assert.That(engine.ProcessCalculation(input), Is.EqualTo(expected));
        }

        [TestCase("2 * (2 + 2)", 8)]
        [TestCase("2 * (2 + (4 / 2))", 8)]
        [TestCase("4 + 2 * 2", 8)]
        [TestCase("4 + 2 * (2 - 1)", 6)]
        public void Calculate_Complex(string input, double expected)
        {
            var engine = new CalculatorEngine();
            Assert.That(engine.ProcessCalculation(input), Is.EqualTo(expected));
        }
    }
}