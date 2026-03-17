using Tsinswreng.CsCore;

namespace Tsinswreng.CsTreeTest;

[Doc(@$"classes impl this interface means
the class can register tests
")]
public interface ITester{
	[Doc(@$"Init `{nameof(Test)}` By Register other `{nameof(ITestNode)}`
	with `{nameof(ITestCase)}` into it
	")]
	public ITestNode RegisterTestsInto(ITestNode? Test);
}
