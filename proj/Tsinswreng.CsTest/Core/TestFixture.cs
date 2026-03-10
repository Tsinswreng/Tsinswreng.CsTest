namespace Tsinswreng.CsTest.Core;

/// <summary>
/// 测试固件（测试套件容器）
/// </summary>
public class TestFixture
{
    private readonly string _name;
    private readonly List<TestCase> _testCases;

    public string Name => _name;
    public IReadOnlyList<TestCase> TestCases => _testCases.AsReadOnly();

    public TestFixture(string name)
    {
        _name = name ?? throw new ArgumentNullException(nameof(name));
        _testCases = new List<TestCase>();
    }

    /// <summary>
    /// 注册一个测试用例（同步适配）
    /// </summary>
    public TestFixture Register(string testName, Func<object?, object?> testFunc)
    {
        if (testFunc == null)
            throw new ArgumentNullException(nameof(testFunc));

        // 将同步函数适配为异步
        var asyncFunc = new Func<object?, Task<object?>>(async obj =>
        {
            return await Task.FromResult(testFunc(obj));
        });

        _testCases.Add(new TestCase(testName, asyncFunc));
        return this;
    }

    /// <summary>
    /// 注册一个异步测试用例
    /// </summary>
    public TestFixture RegisterAsync(string testName, Func<object?, Task<object?>> testFunc)
    {
        if (testFunc == null)
            throw new ArgumentNullException(nameof(testFunc));

        _testCases.Add(new TestCase(testName, testFunc));
        return this;
    }

    /// <summary>
    /// 注册一个异步测试用例（无返回值）
    /// </summary>
    public TestFixture RegisterAsync(string testName, Func<object?, Task> testFunc)
    {
        if (testFunc == null)
            throw new ArgumentNullException(nameof(testFunc));

        var wrappedFunc = new Func<object?, Task<object?>>(async obj =>
        {
            await testFunc(obj);
            return null;
        });

        _testCases.Add(new TestCase(testName, wrappedFunc));
        return this;
    }

    /// <summary>
    /// 清空所有测试用例
    /// </summary>
    public void Clear()
    {
        _testCases.Clear();
    }
}
