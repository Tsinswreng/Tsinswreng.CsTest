namespace MyProj1.Test.MyDomains.StringTool;

/// <summary>
/// 示例字符串工具类 - 被测试的对象
/// </summary>
public class StringToolService {
	
	/// <summary>
	/// 反转字符串
	/// </summary>
	public string ReverseString(string input) {
		if (string.IsNullOrEmpty(input)) {
			return input;
		}
		char[] chars = input.ToCharArray();
		System.Array.Reverse(chars);
		return new string(chars);
	}

	/// <summary>
	/// 判断是否为回文字符串
	/// </summary>
	public bool IsPalindrome(string input) {
		if (string.IsNullOrEmpty(input)) {
			return true;
		}
		string cleaned = System.Text.RegularExpressions.Regex.Replace(
			input.ToLower(), 
			@"[^a-z0-9]", 
			""
		);
		return cleaned == ReverseString(cleaned);
	}

	/// <summary>
	/// 统计字符串中特定字符的出现次数
	/// </summary>
	public int CountCharacter(string input, char target) {
		if (string.IsNullOrEmpty(input)) {
			return 0;
		}
		return input.ToLower().Count(c => c == char.ToLower(target));
	}

	/// <summary>
	/// 将字符串转换为大写
	/// </summary>
	public string ToUpperCase(string input) {
		return string.IsNullOrEmpty(input) ? input : input.ToUpper();
	}
}
