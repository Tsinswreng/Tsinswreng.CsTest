namespace Tsinswreng.CsTest.Examples;

/// <summary>
/// 测试框架使用示例
/// </summary>
public static class BasicExample {
	public static async Task RunBasicExampleAsync() {
		// 创建测试固件
		var fixture = new TestFixture("Basic Calculator Tests");

		// 注册同步测试
		fixture.Register("Add 1+1=2", obj => {
			var result = 1 + 1;
			if (result != 2)
				throw new AssertionException($"Expected 2, got {result}");
			return null;
		});

		fixture.Register("Add 3+4=7", obj => {
			var result = 3 + 4;
			if (result != 7)
				throw new AssertionException($"Expected 7, got {result}");
			return null;
		});

		// 注册异步测试
		fixture.RegisterAsync("Async Sleep Test", async obj => {
			await Task.Delay(100);
			return null;
		});

		// 注册可能失败的测试
		fixture.Register("Intentional Failure", obj => {
			throw new AssertionException("This test is expected to fail");
		});

		// 创建运行器（可设置超时时间，默认30秒）
		var runner = new TestRunner(timeoutMilliseconds: 30000);

		// 异步运行所有测试
		var report = await runner.RunAsync(fixture);

		// 输出报告
		Console.WriteLine(report.ToString());
	}
}

/// <summary>
/// 简单的断言异常
/// </summary>
public class AssertionException : Exception {
	public AssertionException(string message) : base(message) { }
}
