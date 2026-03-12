using Tsinswreng.CsCore;

namespace Tsinswreng.CsTest;


public static class ExtnITestMgr{
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
		){
			return (str Name, FnTest Fn) => {
				var Case = new TestCase{
					UniqName = Name,
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
