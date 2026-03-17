using Tsinswreng.CsTreeTest;

namespace MyProj1.Test.MyDomains.Calculator;
// each part should only mainly test one function. in this part we test Add
public partial class TestCalculator {
	public void RegisterAdd(ITestNode Node) {
		var register = Node.MkTestFnRegister(
			typeof(TestCalculator), // tester type
			[typeof(Calculator)], // testee types
			[nameof(MyDomains.Calculator.Calculator.Add)], // testee fn names, must use nameof()
			"YourTestNamePrefix" // optional
		);
		var R = register.Register;
		R("Add positive numbers", async (o) => {
			var r = Calculator.Add(5, 3);
			if (r != 8) {
				throw new Exception($"Expected 8 but got {r}");
			}
			return null;
		});
		//you can change the props of `register` here and then you can still use `R` directly
		//e.g. register.UniqNamePrefix = "NewPrefix";
		R("Add positive and negative numbers", async (o) => {
			var r = Calculator.Add(5, -3);
			if (r != 2) {
				throw new Exception($"Expected 2 but got {r}");
			}
			return null;
		});
	}
}
