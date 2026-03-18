using Tsinswreng.CsTreeTest;

namespace MyProj1.Test.MyDomains.Calculator;

public partial class TestCalculator {
	public void RegisterTryIntDivide(ITestNode Node) {
		var register = Node.MkTestFnRegister(
			typeof(TestCalculator),
			[typeof(ICalculator)],
			[nameof(MyDomains.Calculator.ICalculator.TryIntDivide)],
			"Divide"
		);

		var R = register.Register;

		R("positive", async (o) => {
			if(!Calculator.TryIntDivide(20, 4, out var R)){
				throw new Exception("Expected true but got false");
			}
			if(R != 5){
				throw new Exception($"Expected 5 but got {R}");
			}
			return null;
		});
		R("Divide by zero", async (o) => {
			if(Calculator.TryIntDivide(20, 0, out var R)){
				throw new Exception("Expected false but got true");
			}
			return null;
		});
	}
}
