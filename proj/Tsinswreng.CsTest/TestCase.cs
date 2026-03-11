using Tsinswreng.CsCore;

namespace Tsinswreng.CsTest;


public interface ITestCase{
	public FnTest FnTest{get;set;}
	
	[Doc(@$"Clr type that contains the {nameof(FnTest)},
	that is the tester class
	
	this may be used to filter in the future
	")]
	public Type? TesterType{get;set;}
	[Doc(@$"Clr type of testee
	
	e.g if you test `ISvcUser.Login()` then this should be `typeof(ISvcUser)`
	
	this may be used to filter in the future
	")]
	public Type? TesteeType{get;set;}
	
}


/// 表示单个测试用例
public class TestCaseOld {
	public str Name { get; set; }

	public Func<obj?, Task<obj?>> TestFunc { get; set; }

	public TestCaseOld(str Name, Func<obj?, Task<obj?>> TestFunc) {
		this.Name = Name ?? throw new ArgumentNullException(nameof(Name));
		this.TestFunc = TestFunc ?? throw new ArgumentNullException(nameof(TestFunc));
	}
}
