namespace Tsinswreng.CsTest;

/// <summary>
/// 测试运行器
/// </summary>
public class TestRunner
{
    private readonly int _timeoutMilliseconds;

    public TestRunner(int timeoutMilliseconds = 30000)
    {
        _timeoutMilliseconds = timeoutMilliseconds;
    }

    /// <summary>
    /// 异步运行测试固件中的所有测试
    /// </summary>
    public async Task<TestReport> RunAsync(TestFixture fixture)
    {
        if (fixture == null)
            throw new ArgumentNullException(nameof(fixture));

        var report = new TestReport(fixture.Name);

        foreach (var testCase in fixture.TestCases)
        {
            var result = await RunTestCaseAsync(testCase);
            report.AddResult(result);
        }

        return report;
    }

    /// <summary>
    /// 执行单个测试用例
    /// </summary>
    private async Task<TestResult> RunTestCaseAsync(TestCase testCase)
    {
        var result = new TestResult(testCase.Name);
        var stopwatch = System.Diagnostics.Stopwatch.StartNew();

        try
        {
            using var cts = new CancellationTokenSource(_timeoutMilliseconds);
            var task = testCase.TestFunc(null);
            
            // 等待任务完成或超时
            result.ReturnValue = await task.ConfigureAwait(false);
            
            result.Status = TestResultStatus.Passed;
        }
        catch (OperationCanceledException)
        {
            result.Status = TestResultStatus.Timeout;
            result.Exception = new TimeoutException($"Test timed out after {_timeoutMilliseconds}ms");
        }
        catch (Exception ex)
        {
            result.Status = TestResultStatus.Error;
            result.Exception = ex;
        }
        finally
        {
            stopwatch.Stop();
            result.ElapsedMilliseconds = stopwatch.ElapsedMilliseconds;
        }

        return result;
    }
}
