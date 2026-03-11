namespace Tsinswreng.CsTest;
/// 测试树节点接口
/// 支持递归地注册和运行测试
public interface ITestNodeOld {
	str Name { get; }
	Task RegisterTests(TestFixture Fixture, CT Ct);
}
/// 测试树节点基类
public abstract class TestNodeBase : ITestNodeOld {
	public abstract str Name { get; }
	
	/// 子节点列表
	public List<ITestNodeOld> Children { get; } = new();
	
	/// 添加子节点
	public void AddChild(ITestNodeOld Node) {
		Children.Add(Node);
	}
	
	/// 注册本节点和所有子节点的测试
	public virtual async Task RegisterTests(TestFixture Fixture, CT Ct) {
		await RegisterOwnTests(Fixture, Ct);
		foreach (var child in Children) {
			await child.RegisterTests(Fixture, Ct);
		}
	}
	
	/// 注册当前节点自己的测试（由子类实现）
	protected virtual async Task RegisterOwnTests(TestFixture Fixture, CT Ct) {
		// 默认无测试，子类可重写
		await Task.CompletedTask;
	}
}
/// 容器节点，只用于组织子节点，不包含测试
public class TestGroupNode : TestNodeBase {
	private readonly str _name;
	
	public TestGroupNode(str Name) {
		_name = Name;
	}
	
	public override str Name => _name;
}
