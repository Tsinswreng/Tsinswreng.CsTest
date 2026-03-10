using Tsinswreng.CsTest;

file class DirDoc {
	str Doc =
$$"""
#Sum[
轻量级、AOT友好的测试框架。通过手动注册 `Func<obj?, Task<obj?>>` 委托来定义和运行测试。
]

#Descr[
== 核心类

- {{nameof(TestCase)}}: 表示单个测试用例，包含测试名称和测试函数
- {{nameof(TestFixture)}}: 测试固件（测试套件容器），用于注册和管理测试用例
- {{nameof(TestRunner)}}: 测试运行器，并行执行所有测试并生成报告
- {{nameof(TestResult)}}: 单个测试的执行结果（状态、异常、耗时）
- {{nameof(TestReport)}}: 测试报告汇总（通过/失败/错误统计）

== 特性

- 无反射依赖 - 完全手动注册，支持 AOT 编译
- 支持异步 - 原生支持 Task 和 async/await
- 并行执行 - 所有测试默认并行运行
- 简洁 API - 流式注册，易于使用
- 异常捕获 - 自动捕获并报告异常
- 详细报告 - 生成格式化的测试报告

== 使用示例

```
var fixture = new TestFixture("Calculator Tests");

fixture.Register("1+1=2", Obj => {
    var result = 1 + 1;
    if (result != 2) throw new Exception("Failed");
    return null!;
});

fixture.Register("Async Test", async Obj => {
    await Task.Delay(100);
    return null!;
});

var runner = new TestRunner();
var report = await runner.Run(fixture, default);
Console.WriteLine(report.ToString());
```

== 返回值约定

- 返回 null - 测试通过
- 抛出异常 - 测试失败/错误
- 其他返回值 - 测试通过（返回值被保存到 TestResult.ReturnValue）
]
""";
}
