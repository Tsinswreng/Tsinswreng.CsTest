namespace Tsinswreng.CsTest;

/// <summary>
/// 测试树节点接口
/// 支持递归地注册和运行测试
/// </summary>
public interface ITestNode {
	str Name { get; }
	Task RegisterTests(TestFixture Fixture, CT Ct);
}

/// <summary>
/// 测试树节点基类
/// </summary>
public abstract class TestNodeBase : ITestNode {
	public abstract str Name { get; }
	
	/// <summary>
	/// 子节点列表
	/// </summary>
	public List<ITestNode> Children { get; } = new();
	
	/// <summary>
	/// 添加子节点
	/// </summary>
	protected void AddChild(ITestNode Node) {
		Children.Add(Node);
	}
	
	/// <summary>
	/// 注册本节点和所有子节点的测试
	/// </summary>
	public virtual async Task RegisterTests(TestFixture Fixture, CT Ct) {
		await RegisterOwnTests(Fixture, Ct);
		foreach (var child in Children) {
			await child.RegisterTests(Fixture, Ct);
		}
	}
	
	/// <summary>
	/// 注册当前节点自己的测试（由子类实现）
	/// </summary>
	protected virtual async Task RegisterOwnTests(TestFixture Fixture, CT Ct) {
		// 默认无测试，子类可重写
		await Task.CompletedTask;
	}
}

/// <summary>
/// 容器节点，只用于组织子节点，不包含测试
/// </summary>
public class TestGroupNode : TestNodeBase {
	private readonly str _name;
	
	public TestGroupNode(str Name) {
		_name = Name;
	}
	
	public override str Name => _name;
}
