using Tsinswreng.CsTreeTest;

namespace MyProj1.Test.MyDomains.StringTool;

public partial class TestStringTool {
	
	public void RegisterReverseString(ITestNode Node) {
		var register = Node.MkTestFnRegister(
			typeof(TestStringTool),
			[typeof(StringToolService)],
			[nameof(StringToolService.ReverseString)],
			"ReverseString"
		);

		var R = register.Register;

		R("反转普通字符串", async (o) => {
			// Arrange
			string input = "Hello";

			// Act
			string result = _service.ReverseString(input);

			// Assert
			if (result != "olleH") {
				throw new Exception($"Expected 'olleH' but got '{result}'");
			}
			return null;
		});

		R("反转数字字符串", async (o) => {
			// Arrange
			string input = "12345";

			// Act
			string result = _service.ReverseString(input);

			// Assert
			if (result != "54321") {
				throw new Exception($"Expected '54321' but got '{result}'");
			}
			return null;
		});

		R("反转单字符字符串", async (o) => {
			// Arrange
			string input = "A";

			// Act
			string result = _service.ReverseString(input);

			// Assert
			if (result != "A") {
				throw new Exception($"Expected 'A' but got '{result}'");
			}
			return null;
		});

		R("反转空字符串", async (o) => {
			// Arrange
			string input = "";

			// Act
			string result = _service.ReverseString(input);

			// Assert
			if (result != "") {
				throw new Exception($"Expected empty string but got '{result}'");
			}
			return null;
		});

		R("反转包含特殊字符的字符串", async (o) => {
			// Arrange
			string input = "A-1@B";

			// Act
			string result = _service.ReverseString(input);

			// Assert
			if (result != "B@1-A") {
				throw new Exception($"Expected 'B@1-A' but got '{result}'");
			}
			return null;
		});
	}
}
