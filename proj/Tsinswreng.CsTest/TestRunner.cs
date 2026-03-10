namespace Tsinswreng.CsTest;
/// 测试运行
public class TestRunner {
	private readonly int _timeoutMilliseconds;

	public TestRunner(int timeoutMilliseconds = 30000) {
		_timeoutMilliseconds = timeoutMilliseconds;
	}

	/// 异步运行测试固件中的所有测试（并行执行）
	public async Task<TestReport> RunAsync(TestFixture fixture) {
		if (fixture == null)
			throw new ArgumentNullException(nameof(fixture));

		var report = new TestReport(fixture.Name);

		// 并行执行所有测试
		var tasks = fixture.TestCases.Select(testCase => RunTestCaseAsync(testCase)).ToList();
		var results = await Task.WhenAll(tasks).ConfigureAwait(false);

		foreach (var result in results) {
			report.AddResult(result);
		}

		return report;
	}

	/// 执行单个测试用例
	private async Task<TestResult> RunTestCaseAsync(TestCase testCase) {
		var result = new TestResult(testCase.Name);
		var stopwatch = System.Diagnostics.Stopwatch.StartNew();

		try {
			using var cts = new CancellationTokenSource(_timeoutMilliseconds);
			var task = testCase.TestFunc(null);

			// 等待任务完成或超时
			result.ReturnValue = await task.ConfigureAwait(false);

			result.Status = TestResultStatus.Passed;
		}
		catch (OperationCanceledException) {
			result.Status = TestResultStatus.Timeout;
			result.Exception = new TimeoutException($"Test timed out after {_timeoutMilliseconds}ms");
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
