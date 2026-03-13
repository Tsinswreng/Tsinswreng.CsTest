using Tsinswreng.CsCore;

namespace Tsinswreng.CsTest;

public interface ITestFnRegister{
	[Doc(@$" every time you call `{nameof(Register)}`, it should always use newest options.
	which means you can call `{nameof(Register)}` after you change other option fields
	")]
	public void Register(str UniqName, FnTest Fn);
	
	/// 以下字段皆可讀寫
	public Type TesterType{get;set;}
	public IList<Type> TesteeTypes{get;set;}
	public IList<str> TesteeFnNames{get;set;}
	public str UniqNamePrefix{get;set;}
	
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

	public void Register(str UniqName, FnTest Fn){
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
