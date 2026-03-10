namespace Tsinswreng.CsTest;
/// 测试运行
public class TestRunner {
	private readonly i32 _TimeoutMilliseconds;

	public TestRunner(i32 TimeoutMilliseconds = 30000) {
		_TimeoutMilliseconds = TimeoutMilliseconds;
	}

	/// 异步运行测试固件中的所有测试（并行执行）
	public async Task<TestReport> Run(TestFixture Fixture, CT Ct) {
		if (Fixture == null)
			throw new ArgumentNullException(nameof(Fixture));

		var report = new TestReport(Fixture.Name);

		// 并行执行所有测试
		var tasks = Fixture.TestCases.Select(testCase => RunTestCaseAsync(testCase, Ct)).ToList();
		var results = await Task.WhenAll(tasks).ConfigureAwait(false);

		foreach (var result in results) {
			report.AddResult(result);
		}

		return report;
	}

	/// 执行单个测试用例
	private async Task<TestResult> RunTestCaseAsync(TestCase TestCase, CT Ct) {
		var result = new TestResult(TestCase.Name);
		var stopwatch = System.Diagnostics.Stopwatch.StartNew();

		try {
			using var cts = new CancellationTokenSource(_TimeoutMilliseconds);
			using var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(Ct, cts.Token);
			var task = TestCase.TestFunc(null);

			// 等待任务完成或超时
			result.ReturnValue = await task.ConfigureAwait(false);

			result.Status = TestResultStatus.Passed;
		}
		catch (OperationCanceledException) {
			result.Status = TestResultStatus.Timeout;
			result.Exception = new TimeoutException($"Test timed out after {_TimeoutMilliseconds}ms");
		}
		catch (Exception ex) {
			result.Status = TestResultStatus.Error;
			result.Exception = ex;
		}
		finally {
			stopwatch.Stop();
			result.ElapsedMilliseconds = stopwatch.ElapsedMilliseconds;
		}

		return result;
	}
}
