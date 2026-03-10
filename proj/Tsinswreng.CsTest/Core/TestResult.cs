namespace Tsinswreng.CsTest.Core;

/// <summary>
/// 测试结果状态枚举
/// </summary>
public enum TestResultStatus
{
    Passed = 0,
    Failed = 1,
    Error = 2,
    Timeout = 3
}

/// <summary>
/// 单个测试的执行结果
/// </summary>
public class TestResult
{
    public string TestName { get; set; }
    
    public TestResultStatus Status { get; set; }
    
    public object? ReturnValue { get; set; }
    
    public Exception? Exception { get; set; }
    
    public long ElapsedMilliseconds { get; set; }

    public TestResult(string testName)
    {
        TestName = testName ?? throw new ArgumentNullException(nameof(testName));
        Status = TestResultStatus.Passed;
        ElapsedMilliseconds = 0;
    }

    public override string ToString()
    {
        var status = Status switch
        {
            TestResultStatus.Passed => "✓ PASSED",
            TestResultStatus.Failed => "✗ FAILED",
            TestResultStatus.Error => "⚠ ERROR",
            TestResultStatus.Timeout => "⏱ TIMEOUT",
            _ => "? UNKNOWN"
        };

        var message = $"[{status}] {TestName} ({ElapsedMilliseconds}ms)";
        
        if (Exception is not null)
        {
            message += $"\n  Exception: {Exception.GetType().Name}";
            message += $"\n  Message: {Exception.Message}";
        }

        return message;
    }
}
