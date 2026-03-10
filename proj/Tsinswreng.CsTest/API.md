# Tsinswreng.CsTest - 无注解测试框架

一个轻量级、AOT友好的测试框架，通过手动注册 `Func<object?, Task<object?>>` 委托来定义和运行测试。

## 特性

- ✅ **无反射依赖** - 完全手动注册，支持 AOT 编译
- ✅ **支持异步** - 原生支持 `Task` 和 `async/await`
- ✅ **简洁 API** - 流式注册，易于使用
- ✅ **异常捕获** - 自动捕获并报告异常
- ✅ **详细报告** - 生成格式化的测试报告

## 核心类

### TestFixture
测试套件容器，用于注册和管理测试用例。

```csharp
var fixture = new TestFixture("My Tests");

// 注册同步测试
fixture.Register("Test Name", obj => 
{
    // 测试代码
    if (condition) throw new Exception("Failed");
    return null; // null 表示通过
});

// 注册异步测试
fixture.RegisterAsync("Async Test", async obj =>
{
    await Task.Delay(100);
    return null;
});
```

**支持链式调用：**
```csharp
fixture
    .Register("Test1", obj => null)
    .Register("Test2", obj => null)
    .RegisterAsync("Test3", async obj => null);
```

### TestRunner
执行所有测试并生成报告。

```csharp
var runner = new TestRunner(timeoutMilliseconds: 30000);
var report = await runner.RunAsync(fixture);
Console.WriteLine(report.ToString());
```

### TestResult & TestReport
- **TestResult**: 单个测试的执行结果（状态、异常、耗时）
- **TestReport**: 所有测试的汇总报告（通过/失败/错误统计）

## 使用示例

```csharp
using Tsinswreng.CsTest.Core;

// 创建测试
var fixture = new TestFixture("Calculator Tests");

fixture.Register("1+1=2", obj =>
{
    var result = 1 + 1;
    if (result != 2) throw new Exception("Failed");
    return null;
});

fixture.RegisterAsync("Async Operation", async obj =>
{
    await Task.Delay(100);
    return null;
});

// 运行测试
var runner = new TestRunner();
var report = await runner.RunAsync(fixture);

// 查看结果
Console.WriteLine(report.ToString());
Console.WriteLine($"All Passed: {report.AllPassed}");
```

## 返回值约定

- **返回 `null`** → 测试通过
- **抛出异常** → 测试失败/错误
- **其他返回值** → 测试通过（返回值被保存到 `TestResult.ReturnValue`）

## AOT 兼容性

该框架完全兼容 AOT 编译，因为：
- 不使用反射
- 不使用 `Activator.CreateInstance` 或动态创建
- 使用强类型委托而非 Func\<dynamic\>
- 所有类型都是编译时确定的

## 报告格式

```
═════════════════════════════════════════════════════════════
  Test Report: Calculator Tests
═════════════════════════════════════════════════════════════

[✓ PASSED] 1+1=2 (5ms)
[⚠ ERROR] Async Operation (105ms)
  Exception: TimeoutException
  Message: Test timed out after 30000ms

─────────────────────────────────────────────────────────────
  Summary:
    Total:    2
    Passed:   1
    Failed:   0
    Error:    1
    Timeout:  0
    Duration: 110ms
─────────────────────────────────────────────────────────────
  Result: ✗ SOME FAILED
═════════════════════════════════════════════════════════════
```

## 配置

### 超时设置

```csharp
// 自定义超时时间（毫秒）
var runner = new TestRunner(timeoutMilliseconds: 60000);
```

默认超时时间为 30 秒。
