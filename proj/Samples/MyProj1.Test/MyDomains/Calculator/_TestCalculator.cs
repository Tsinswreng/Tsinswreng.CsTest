//we suggest one tester class corresponds to one testee class.
// and put different tester classes in separates folders.
//now we are in Calculator/ , this is only for `TestCalculator`
using Tsinswreng.CsTreeTest;

namespace MyProj1.Test.MyDomains.Calculator;

//Use partial class. this is the main part. the file name of main part should start with _
// each part of the class should only test one method
// the main part should not do test but assemble the test cases in other parts.
public partial class TestCalculator : ITester {
	ICalculator Calculator;
	public TestCalculator(
		ICalculator Calculator
	) {
		this.Calculator = Calculator;
	}//we use DependencyInjection here
	// In some scenarios we use di, in other cases
	// we can directly new the testee class without di
	
	public ITestNode RegisterTestsInto(ITestNode? Test) {
		Test ??= new TestNode();
		//default is false, if set to true, the tests in this node will run in order.
		Test.Ordered = false;

		RegisterAdd(Test);
		RegisterTryIntDivide(Test);
		return Test;
	}
}
