using MyProj1.Test.MyDomains.Calculator;
using Tsinswreng.CsTreeTest;

namespace MyProj1.Test;


// Each csproj of test project should have a Test Manager
// and register all tester below
public class MyProj1TestMgr : DiEtTestMgr {
	public static MyProj1TestMgr Inst = new();
	public override ITestNode RegisterTestsInto(ITestNode? Node) {
		Node = this.TestNode;
		// 注册各个领域的测试器
		this.RegisterTester<TestCalculator>();
		return Node;
	}
}
