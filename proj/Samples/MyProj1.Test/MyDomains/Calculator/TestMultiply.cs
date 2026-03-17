using Tsinswreng.CsTreeTest;

namespace MyProj1.Test.MyDomains.Calculator;

public partial class TestCalculator {
	
	public void RegisterMultiply(ITestNode Node) {
		var register = Node.MkTestFnRegister(
			typeof(TestCalculator),
			[typeof(Calculator)],
			[nameof(Calculator.Multiply)],
			"Multiply"
		);

		var R = register.Register;

		R("正数相乘", async (o) => {
			// Arrange
			int a = 4;
			int b = 5;

			// Act
			int result = _calculator.Multiply(a, b);

			// Assert
			if (result != 20) {
				throw new Exception($"Expected 20 but got {result}");
			}
			return null;
		});

		R("正数和负数相乘", async (o) => {
			// Arrange
			int a = 4;
			int b = -5;

			// Act
			int result = _calculator.Multiply(a, b);

			// Assert
			if (result != -20) {
				throw new Exception($"Expected -20 but got {result}");
			}
			return null;
		});

		R("两个负数相乘", async (o) => {
			// Arrange
			int a = -4;
			int b = -5;

			// Act
			int result = _calculator.Multiply(a, b);

			// Assert
			if (result != 20) {
				throw new Exception($"Expected 20 but got {result}");
			}
			return null;
		});

		R("乘以零", async (o) => {
			// Arrange
			int a = 5;
			int b = 0;

			// Act
			int result = _calculator.Multiply(a, b);

			// Assert
			if (result != 0) {
				throw new Exception($"Expected 0 but got {result}");
			}
			return null;
		});
	}
}
