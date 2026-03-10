namespace Tsinswreng.CsTest;

/// <summary>
/// 测试节点运行器，支持运行指定的测试节点
/// </summary>
public static class TestNodeRunner {
	/// <summary>
	/// 运行指定的测试节点
	/// </summary>
	public static async Task<obj?> RunNode(ITestNode Node, CT Ct) {
		var fixture = new TestFixture(Node.Name);
		await Node.RegisterTests(fixture, Ct);
		
		var runner = new TestRunner();
		var report = await runner.Run(fixture, Ct);
		Console.WriteLine(report.ToString());
		
		if (!report.AllPassed) {
			throw new Exception("Tests failed");
		}
		
		return null;
	}
	
	/// <summary>
	/// 递归查找指定名称的节点
	/// </summary>
	public static ITestNode? FindNodeByName(ITestNode Root, str TargetName) {
		if (Root.Name == TargetName) {
			return Root;
		}
		
		if (Root is TestNodeBase nodeBase) {
			foreach (var child in nodeBase.Children) {
				var found = FindNodeByName(child, TargetName);
				if (found is not null) {
					return found;
				}
			}
		}
		
		return null;
	}
}
