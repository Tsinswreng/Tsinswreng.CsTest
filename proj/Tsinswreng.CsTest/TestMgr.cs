using Tsinswreng.CsCore;

namespace Tsinswreng.CsTest;


public static class ExtnITestMgr{
	extension(ITestNode z){
		public Func<
			str, FnTest, nil
		> MkFnRegisterTest(
			Type TesterType
			,Type TesteeType
		){
			return (str Name, FnTest Fn) => {
				var Case = new TestCase{
					Name = Name,
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
