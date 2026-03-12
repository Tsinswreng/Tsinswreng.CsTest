using Tsinswreng.CsCore;
using Tsinswreng.CsU128Id;

namespace Tsinswreng.CsTest;

[Doc(@$"Node of test tree.

Running a node of test tree  is to run all its children test cases.
")]
public interface ITestNode{
	
	public str UniqName{get;set;}
	
	[Doc(@$"Leaf node should hava {nameof(Data)}
	
	for non-leaf node, we suggest it should be null
	")]
	public ITestCase? Data{get;set;}
	public IList<ITestNode> Children {get;set;}
	
	[Doc(@$"Should only have one parent.
	
	but I design it as IList for be compatible with graph
	")]
	public IList<ITestNode> Parents {get;set;}
	
}

public interface ITesterNode:ITestNode{
	public Type? TesterType{get;set;}
}

public class TestNode : ITestNode{
	public str UniqName{get;set;} = U128Id.NewUlid().ToLow64Base();
	public ITestCase? Data{get;set;}
	public IList<ITestNode> Children {get;set;} = [];
	public IList<ITestNode> Parents {get;set;} = [];
}

public class TesterNode : TestNode, ITesterNode{
	public Type? TesterType{get;set;}
}

public static class ExtnITestNode{
	extension(ITestNode z){
		[Doc("Make a new child of self")]
		public ITestNode NewChild(str? UniqName = null){
			var R = new TestNode(){UniqName = UniqName ?? U128Id.NewUlid().ToLow64Base()};
			z.Children.Add(R);
			R.Parents.Add(z);
			return R;
		}
		public ITestNode NewChild<T>(str? UniqName = null){
			var R = new TesterNode(){
				UniqName = UniqName ?? U128Id.NewUlid().ToLow64Base()
				,TesterType = typeof(T)
			};
			z.Children.Add(R);
			R.Parents.Add(z);
			return R;
		}
	}
	extension(ITestNode z){
	[Doc(@$"Make a function to register `{nameof(FnTest)}`.
	this can help you simplify your code
	,without duplicately passing the same `{nameof(Type)}`
	")]
		public Func<
			str, FnTest, nil
		> MkFnRegisterTest(
			Type TesterType
			,Type TesteeType
			,str UniqNamePrefix = ""
		){
			return (str UniqName, FnTest Fn) => {
				var Case = new TestCase{
					UniqName = UniqNamePrefix+UniqName,
					TesterType = TesterType,
					TesteeType = TesteeType,
					FnTest = Fn,
				};
				z.Children.Add(new TestNode{
					Data = Case,
				});
				return NIL;
			};
		}
	}
}
