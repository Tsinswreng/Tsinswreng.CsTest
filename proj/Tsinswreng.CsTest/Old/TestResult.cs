namespace Tsinswreng.CsTest;

/// 测试结果状态枚举
public enum TestResultStatus {
	Passed = 0,
	Failed = 1,
	Error = 2,
	Timeout = 3
}

/// 单个测试的执行结果
public class TestResult {
	public str TestName { get; set; }

	public TestResultStatus Status { get; set; }

	public obj? ReturnValue { get; set; }

	public Exception? Exception { get; set; }

	public long ElapsedMilliseconds { get; set; }

	public TestResult(str TestName) {
		this.TestName = TestName ?? throw new ArgumentNullException(nameof(TestName));
		Status = TestResultStatus.Passed;
		ElapsedMilliseconds = 0;
	}

	public override str ToString() {
		var statusStr = Status switch {
			TestResultStatus.Passed => "✓ PASSED",
			TestResultStatus.Failed => "✗ FAILED",
			TestResultStatus.Error => "⚠ ERROR",
			TestResultStatus.Timeout => "⏱ TIMEOUT",
			_ => "? UNKNOWN"
		};

		var message = $"[{statusStr}] {TestName} ({ElapsedMilliseconds}ms)";

		if (Exception is not null) {
			message += $"\n  Exception: {Exception.GetType().Name}";
			message += $"\n  Message: {Exception.Message}";
		}

		return message;
	}
}
