using Tsinswreng.CsTreeTest;

namespace MyProj1.Test.MyDomains.Calculator;

public partial class TestCalculator {
	
	public void RegisterDivide(ITestNode Node) {
		var register = Node.MkTestFnRegister(
			typeof(TestCalculator),
			[typeof(Calculator)],
			[nameof(Calculator.Divide)],
			"Divide"
		);

		var R = register.Register;

		R("正数相除", async (o) => {
			// Arrange
			int a = 20;
			int b = 4;

			// Act
			int result = _calculator.Divide(a, b);

			// Assert
			if (result != 5) {
				throw new Exception($"Expected 5 but got {result}");
			}
			return null;
		});

		R("负数相除", async (o) => {
			// Arrange
			int a = -20;
			int b = -4;

			// Act
			int result = _calculator.Divide(a, b);

			// Assert
			if (result != 5) {
				throw new Exception($"Expected 5 but got {result}");
			}
			return null;
		});

		R("正数除以负数", async (o) => {
			// Arrange
			int a = 20;
			int b = -4;

			// Act
			int result = _calculator.Divide(a, b);

			// Assert
			if (result != -5) {
				throw new Exception($"Expected -5 but got {result}");
			}
			return null;
		});

		R("除以零应抛出异常", async (o) => {
			// Arrange
			int a = 20;
			int b = 0;

			// Act & Assert
			try {
				int result = _calculator.Divide(a, b);
				throw new Exception("Expected ArgumentException to be thrown");
			} catch (ArgumentException ex) {
				if (!ex.Message.Contains("除数不能为零")) {
					throw new Exception($"Expected exception message containing '除数不能为零' but got '{ex.Message}'");
				}
			}
			return null;
		});
	}
}
