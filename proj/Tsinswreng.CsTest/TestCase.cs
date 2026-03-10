namespace Tsinswreng.CsTest;

/// 表示单个测试用例
public class TestCase {
	public str Name { get; set; }

	public Func<obj?, Task<obj?>> TestFunc { get; set; }

	public TestCase(str Name, Func<obj?, Task<obj?>> TestFunc) {
		this.Name = Name ?? throw new ArgumentNullException(nameof(Name));
		this.TestFunc = TestFunc ?? throw new ArgumentNullException(nameof(TestFunc));
	}
}
