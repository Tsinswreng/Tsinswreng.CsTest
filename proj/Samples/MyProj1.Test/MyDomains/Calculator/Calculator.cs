namespace MyProj1.Test.MyDomains.Calculator;

/// <summary>
/// 示例计算器类 - 被测试的对象
/// </summary>
public class Calculator {
	
	/// <summary>
	/// 加法
	/// </summary>
	public int Add(int a, int b) {
		return a + b;
	}

	/// <summary>
	/// 减法
	/// </summary>
	public int Subtract(int a, int b) {
		return a - b;
	}

	/// <summary>
	/// 乘法
	/// </summary>
	public int Multiply(int a, int b) {
		return a * b;
	}

	/// <summary>
	/// 除法
	/// </summary>
	/// <exception cref="ArgumentException">除数不能为0</exception>
	public int Divide(int a, int b) {
		if (b == 0) {
			throw new ArgumentException("除数不能为零");
		}
		return a / b;
	}
}
