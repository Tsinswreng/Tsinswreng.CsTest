namespace Tsinswreng.CsTest;
/// 测试报告汇
public class TestReport {
	private readonly List<TestResult> _results;
	private readonly string _fixtureName;

	public string FixtureName => _fixtureName;
	public IReadOnlyList<TestResult> Results => _results.AsReadOnly();

	public int TotalCount => _results.Count;
	public int PassedCount => _results.Count(r => r.Status == TestResultStatus.Passed);
	public int FailedCount => _results.Count(r => r.Status == TestResultStatus.Failed);
	public int ErrorCount => _results.Count(r => r.Status == TestResultStatus.Error);
	public int TimeoutCount => _results.Count(r => r.Status == TestResultStatus.Timeout);

	public long TotalElapsedMilliseconds => _results.Sum(r => r.ElapsedMilliseconds);

	public bool AllPassed => _results.All(r => r.Status == TestResultStatus.Passed);

	public TestReport(string fixtureName) {
		_fixtureName = fixtureName ?? throw new ArgumentNullException(nameof(fixtureName));
		_results = new List<TestResult>();
	}

	/// 添加测试结果
	internal void AddResult(TestResult result) {
		_results.Add(result ?? throw new ArgumentNullException(nameof(result)));
	}

	/// 生成文本报告
	public override string ToString() {
		var sb = new System.Text.StringBuilder();

		sb.AppendLine();
		sb.AppendLine("═════════════════════════════════════════════════════════════");
		sb.AppendLine($"  Test Report: {FixtureName}");
		sb.AppendLine("═════════════════════════════════════════════════════════════");
		sb.AppendLine();

		foreach (var result in _results) {
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
