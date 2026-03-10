using Tsinswreng.CsTest;

file class DirDoc {
	str Doc =
$$"""
#Sum[
轻量级、AOT友好的测试框架。通过手动注册 `Func<obj?, Task<obj?>>` 委托来定义和运行测试。支持树型结构组织测试用例。
]

#Descr[
== 核心类

- {{nameof(TestCase)}}: 表示单个测试用例，包含测试名称和测试函数
- {{nameof(TestFixture)}}: 测试固件（测试套件容器），用于注册和管理测试用例
- {{nameof(TestResult)}}: 单个测试的执行结果（状态、异常、耗时）
- {{nameof(TestReport)}}: 测试报告汇总（通过/失败/错误统计）

== 树型节点系统

- {{nameof(ITestNode)}}: 测试树节点接口，支持递归注册和运行
- {{nameof(TestNodeBase)}}: 测试树节点基类，提供子节点管理和递归注册
- {{nameof(TestGroupNode)}}: 容器节点，只用于组织子节点
- {{nameof(TestNodeRunner)}}: 节点运行器，支持运行指定节点或查找节点

== 特性

- 无反射依赖 - 完全手动注册，支持 AOT 编译
- 支持异步 - 原生支持 Task 和 async/await
- 并行执行 - 所有测试默认并行运行
- 树型结构 - 支持按模块组织测试，递归注册子节点
- 简洁 API - 流式注册，易于使用
- 异常捕获 - 自动捕获并报告异常
- 详细报告 - 生成格式化的测试报告
- 灵活运行 - 可运行全部测试或指定节点下的测试

== 基础使用示例

树型组织的测试自动并行执行所有用例，无需单独调用 TestRunner：

```csharp
// 定义测试节点
public class RepoTestNode : TestNodeBase {
    public override str Name => "Repo Tests";
    
    protected override async Task RegisterOwnTests(TestFixture Fixture, CT Ct) {
        var testRepo = GetRSvc<TestRepo>();
        Fixture.Register("Test 1", (Obj) => testRepo.TestMethod1(Obj, Ct));
        Fixture.Register("Test 2", (Obj) => testRepo.TestMethod2(Obj, Ct));
        await Task.CompletedTask;
    }
}

// 定义根节点组织树结构
public class RootTestNode : TestNodeBase {
    public override str Name => "All Tests";
    
    public RootTestNode() {
        var csSqlHelper = new TestGroupNode("CsSqlHelper");
        csSqlHelper.AddChild(new RepoTestNode());
        AddChild(csSqlHelper);
        
        AddChild(new WordTestNode());
        // 更多节点...
    }
}

// 运行全部测试
var root = new RootTestNode();
await TestNodeRunner.RunNode(root, default);

// 或运行指定节点的测试
var repoNode = TestNodeRunner.FindNodeByName(root, "Repo Tests");
if (repoNode is not null) {
    await TestNodeRunner.RunNode(repoNode, default);  // 仅运行 Repo Tests 及其子节点
}
```

== 返回值约定

- 返回 null - 测试通过
- 抛出异常 - 测试失败/错误
- 其他返回值 - 测试通过（返回值被保存到 TestResult.ReturnValue）
]
""";
}
