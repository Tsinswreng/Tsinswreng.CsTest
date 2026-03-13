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
	public Type TesterType{get;set;}
	public IList<Type> TesteeTypes{get;set;}
	public IList<str> TesteeFnNames{get;set;}
	public str UniqNamePrefix{get;set;}

	public TestFnRegister(ITestNode node, Type testerType, Type testeeType, str uniqNamePrefix){
		Node = node;
		TesterType = testerType;
		TesteeTypes = [testeeType];
		TesteeFnNames = [];
		UniqNamePrefix = uniqNamePrefix;
	}
	
	public TestFnRegister(
		ITestNode node
		,Type testerType
		,IList<Type> testeeTypes
		,IList<str> testeeFnNames
		,str uniqNamePrefix
	){
		Node = node;
		TesterType = testerType;
		TesteeTypes = testeeTypes;
		TesteeFnNames = testeeFnNames;
		UniqNamePrefix = uniqNamePrefix;
	}

	public void Register(str UniqName, FnTest Fn){
		var Case = new TestCase{
			UniqName = UniqNamePrefix + UniqName,
			TesterType = TesterType,
			TesteeTypes = TesteeTypes,
			TesteeFnNames = TesteeFnNames,
			FnTest = Fn,
		};
		Node.Children.Add(new TestNode{
			Data = Case,
		});
	}
}
