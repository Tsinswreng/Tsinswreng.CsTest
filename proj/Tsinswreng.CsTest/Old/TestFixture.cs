namespace Tsinswreng.CsTest;

/// 测试固件（测试套件容器）
public class TestFixture {
	private readonly str _Name;
	private readonly List<TestCaseOld> _TestCases;

	public str Name => _Name;
	public IReadOnlyList<TestCaseOld> TestCases => _TestCases.AsReadOnly();

	public TestFixture(str Name) {
		_Name = Name ?? throw new ArgumentNullException(nameof(Name));
		_TestCases = new List<TestCaseOld>();
	}

	/// 注册一个测试用例（同步适配）
	public TestFixture Register(str TestName, Func<obj?, obj?> TestFunc) {
		if (TestFunc == null)
			throw new ArgumentNullException(nameof(TestFunc));

		var asyncFunc = new Func<obj?, Task<obj?>>(async Obj => {
			return await Task.FromResult(TestFunc(Obj));
		});

		_TestCases.Add(new TestCaseOld(TestName, asyncFunc));
		return this;
	}

	/// 注册一个异步测试用例
	public TestFixture Register(str TestName, Func<obj?, Task<obj?>> TestFunc) {
		if (TestFunc == null)
			throw new ArgumentNullException(nameof(TestFunc));

		_TestCases.Add(new TestCaseOld(TestName, TestFunc));
		return this;
	}

	/// 注册一个异步测试用例（无返回值）
	public TestFixture Register(str TestName, Func<obj?, Task> TestFunc) {
		if (TestFunc == null)
			throw new ArgumentNullException(nameof(TestFunc));

		var wrappedFunc = new Func<obj?, Task<obj?>>(async Obj => {
			await TestFunc(Obj);
			return null;
		});

		_TestCases.Add(new TestCaseOld(TestName, wrappedFunc));
		return this;
	}

	/// 清空所有测试用例
	public void Clear() {
		_TestCases.Clear();
	}
}
