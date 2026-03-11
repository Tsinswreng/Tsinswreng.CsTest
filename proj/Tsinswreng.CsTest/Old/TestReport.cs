namespace Tsinswreng.CsTest;
/// 测试报告汇总
public class TestReport {
	private readonly List<TestResult> _Results;
	private readonly str _FixtureName;

	public str FixtureName => _FixtureName;
	public IReadOnlyList<TestResult> Results => _Results.AsReadOnly();

	public i32 TotalCount => _Results.Count;
	public i32 PassedCount => _Results.Count(r => r.Status == TestResultStatus.Passed);
	public i32 FailedCount => _Results.Count(r => r.Status == TestResultStatus.Failed);
	public i32 ErrorCount => _Results.Count(r => r.Status == TestResultStatus.Error);
	public i32 TimeoutCount => _Results.Count(r => r.Status == TestResultStatus.Timeout);

	public long TotalElapsedMilliseconds => _Results.Sum(r => r.ElapsedMilliseconds);

	public bool AllPassed => _Results.All(r => r.Status == TestResultStatus.Passed);

	public TestReport(str FixtureName) {
		_FixtureName = FixtureName ?? throw new ArgumentNullException(nameof(FixtureName));
		_Results = new List<TestResult>();
	}

	/// 添加测试结果
	internal void AddResult(TestResult Result) {
		_Results.Add(Result ?? throw new ArgumentNullException(nameof(Result)));
	}

	/// 生成文本报告
	public override str ToString() {
		var sb = new System.Text.StringBuilder();

		sb.AppendLine();
		sb.AppendLine("═════════════════════════════════════════════════════════════");
		sb.AppendLine($"  Test Report: {FixtureName}");
		sb.AppendLine("═════════════════════════════════════════════════════════════");
		sb.AppendLine();

		foreach (var result in _Results) {
			sb.AppendLine(result.ToString());
		}

		sb.AppendLine();
		sb.AppendLine("─────────────────────────────────────────────────────────────");
		sb.AppendLine($"  Summary:");
		sb.AppendLine($"    Total:    {TotalCount}");
		sb.AppendLine($"    Passed:   {PassedCount}");
		sb.AppendLine($"    Failed:   {FailedCount}");
		sb.AppendLine($"    Error:    {ErrorCount}");
		sb.AppendLine($"    Timeout:  {TimeoutCount}");
		sb.AppendLine($"    Duration: {TotalElapsedMilliseconds}ms");
		sb.AppendLine("─────────────────────────────────────────────────────────────");
		sb.AppendLine(AllPassed ? "  Result: ✓ ALL PASSED" : "  Result: ✗ SOME FAILED");
		sb.AppendLine("═════════════════════════════════════════════════════════════");
		sb.AppendLine();

		return sb.ToString();
	}
}
