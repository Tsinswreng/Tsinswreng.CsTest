using Tsinswreng.CsCore;

namespace Tsinswreng.CsTest;

[Doc(@$"Node of test tree.

Running a node of test tree  is to run all its children test cases.
")]
public interface ITestNode{
	public IList<ITestNode> Children {get;set;}
	
	[Doc(@$"Should only have one parent.
	
	but I design it as IList for be compatible with graph
	")]
	public IList<ITestNode> Parents {get;set;}
	
}
