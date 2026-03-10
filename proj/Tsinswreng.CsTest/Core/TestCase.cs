namespace Tsinswreng.CsTest.Core;

/// <summary>
/// 表示单个测试用例
/// </summary>
public class TestCase
{
    public string Name { get; set; }
    
    public Func<object?, Task<object?>> TestFunc { get; set; }

    public TestCase(string name, Func<object?, Task<object?>> testFunc)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        TestFunc = testFunc ?? throw new ArgumentNullException(nameof(testFunc));
    }
}
