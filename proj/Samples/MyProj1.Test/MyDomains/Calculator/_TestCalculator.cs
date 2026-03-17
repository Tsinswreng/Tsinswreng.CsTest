using Tsinswreng.CsTreeTest;

namespace MyProj1.Test.MyDomains.Calculator;

/// <summary>
/// 测试计算器服务
/// 使用分部类形式组织各个函数的测试
/// </summary>
public partial class TestCalculator : ITester {
	
	private Calculator _calculator;

	public TestCalculator() {
		_calculator = new Calculator();
	}

	public ITestNode RegisterTestsInto(ITestNode? Test) {
		Test ??= new TestNode();
		Test.Ordered = true; // 按顺序执行子测试

		RegisterAdd(Test);
		RegisterSubtract(Test);
		RegisterMultiply(Test);
		RegisterDivide(Test);

		return Test;
	}
}
