using Tsinswreng.CsTreeTest;

namespace MyProj1.Test.MyDomains.Calculator;

public partial class TestCalculator {
	
	public void RegisterAdd(ITestNode Node) {
		var register = Node.MkTestFnRegister(
			typeof(TestCalculator),
			[typeof(Calculator)],
			[nameof(Calculator.Add)],
			"Add"
		);

		var R = register.Register;

		R("正数相加", async (o) => {
			// Arrange
			int a = 5;
			int b = 3;

			// Act
			int result = _calculator.Add(a, b);

			// Assert
			if (result != 8) {
				throw new Exception($"Expected 8 but got {result}");
			}
			return null;
		});

		R("负数相加", async (o) => {
			// Arrange
			int a = -5;
			int b = -3;

			// Act
			int result = _calculator.Add(a, b);

			// Assert
			if (result != -8) {
				throw new Exception($"Expected -8 but got {result}");
			}
			return null;
		});

		R("正数和负数相加", async (o) => {
			// Arrange
			int a = 10;
			int b = -3;

			// Act
			int result = _calculator.Add(a, b);

			// Assert
			if (result != 7) {
				throw new Exception($"Expected 7 but got {result}");
			}
			return null;
		});

		R("零值相加", async (o) => {
			// Arrange
			int a = 0;
			int b = 5;

			// Act
			int result = _calculator.Add(a, b);

			// Assert
			if (result != 5) {
				throw new Exception($"Expected 5 but got {result}");
			}
			return null;
		});
	}
}
