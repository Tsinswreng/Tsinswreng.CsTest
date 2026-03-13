using Tsinswreng.CsCore;

namespace Tsinswreng.CsTest;

public interface ITestFnRegister{
	public void RegisterOld(str UniqName, FnTest Fn);
	
}

public class TestFnRegister : ITestFnRegister{
	private readonly ITestNode Node;
	private readonly Type TesterType;
	private readonly Type TesteeType;
	private readonly str UniqNamePrefix;

	public TestFnRegister(ITestNode node, Type testerType, Type testeeType, str uniqNamePrefix){
		Node = node;
		TesterType = testerType;
		TesteeType = testeeType;
		UniqNamePrefix = uniqNamePrefix;
	}

	public void RegisterOld(str UniqName, FnTest Fn){
		var Case = new TestCase{
			UniqName = UniqNamePrefix + UniqName,
			TesterType = TesterType,
			TesteeType = TesteeType,
			FnTest = Fn,
		};
		Node.Children.Add(new TestNode{
			Data = Case,
		});
	}
}
