using Tsinswreng.CsTreeTest;

namespace MyProj1.Test.MyDomains.Calculator;

public partial class TestCalculator {
	
	public void RegisterSubtract(ITestNode Node) {
		var register = Node.MkTestFnRegister(
			typeof(TestCalculator),
			[typeof(Calculator)],
			[nameof(Calculator.Subtract)],
			"Subtract"
		);

		var R = register.Register;

		R("正数相减", async (o) => {
			// Arrange
			int a = 10;
			int b = 3;

			// Act
			int result = _calculator.Subtract(a, b);

			// Assert
			if (result != 7) {
				throw new Exception($"Expected 7 but got {result}");
			}
			return null;
		});

		R("从较小数中减去较大数", async (o) => {
			// Arrange
			int a = 3;
			int b = 10;

			// Act
			int result = _calculator.Subtract(a, b);

			// Assert
			if (result != -7) {
				throw new Exception($"Expected -7 but got {result}");
			}
			return null;
		});

		R("相同的数相减", async (o) => {
			// Arrange
			int a = 5;
			int b = 5;

			// Act
			int result = _calculator.Subtract(a, b);

			// Assert
			if (result != 0) {
				throw new Exception($"Expected 0 but got {result}");
			}
			return null;
		});
	}
}
