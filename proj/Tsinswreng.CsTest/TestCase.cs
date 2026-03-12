using Tsinswreng.CsCore;
using Tsinswreng.CsU128Id;

namespace Tsinswreng.CsTest;

[Doc(@$"Test Only one Function per case

you can also combine many functions into one function in one case
")]
public interface ITestCase{
	[Doc(@$"Function to test")]
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
	
	public str UniqName{get;set;}
	
}

public class TestCase : ITestCase{
	public FnTest FnTest{get;set;}
	public Type? TesterType{get;set;}
	public Type? TesteeType{get;set;}
	public str UniqName{get;set;} = U128Id.NewUlid().ToLow64Base();
}

