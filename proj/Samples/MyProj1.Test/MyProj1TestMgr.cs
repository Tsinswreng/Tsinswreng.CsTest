using MyProj1.Test.MyDomains.Calculator;
using MyProj1.Test.MyDomains.StringTool;
using Tsinswreng.CsTreeTest;

namespace MyProj1.Test;

/// <summary>
/// 根测试管理器，管理所有子测试器
/// 这是示例项目的主测试入口
/// </summary>
public class MyProj1TestMgr : DiEtTestMgr {
	public static MyProj1TestMgr Inst = new();

	public override ITestNode RegisterTestsInto(ITestNode? Test) {
		Test = this.TestNode;
		
		// 注册各个领域的测试器
		this.RegisterTester<TestCalculator>();
		this.RegisterTester<TestStringTool>();
		
		return Test;
	}
}
