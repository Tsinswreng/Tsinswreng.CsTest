using Tsinswreng.CsTreeTest;

namespace MyProj1.Test.MyDomains.StringTool;

/// <summary>
/// 测试字符串工具服务
/// 使用分部类形式组织各个函数的测试
/// </summary>
public partial class TestStringTool : ITester {
	
	private StringToolService _service;

	public TestStringTool() {
		_service = new StringToolService();
	}

	public ITestNode RegisterTestsInto(ITestNode? Test) {
		Test ??= new TestNode();
		Test.Ordered = true; // 按顺序执行子测试

		RegisterReverseString(Test);
		RegisterIsPalindrome(Test);
		RegisterCountCharacter(Test);
		RegisterToUpperCase(Test);

		return Test;
	}
}
