using System.Collections.Concurrent;
using Tsinswreng.CsCore;

namespace Tsinswreng.CsTest;

public sealed class OptTreeTestExecutor{
	[Doc("Default is Environment.ProcessorCount")]
	public int MaxDegreeOfParallelism { get; set; } = Environment.ProcessorCount;
}

public sealed class TestCaseRunResult{
	public required int Order { get; set; }
	public required str NodePath { get; set; }
	public required ITestNode Node { get; set; }
	public required ITestCase TestCase { get; set; }
	public required bool IsPassed { get; set; }
	public Exception? Exception { get; set; }
	public TimeSpan Elapsed { get; set; }
}

public sealed class TestRunSummary{
	public DateTimeOffset StartedAt { get; set; }
	public DateTimeOffset EndedAt { get; set; }
	public TimeSpan Elapsed => EndedAt - StartedAt;

	public int Total { get; set; }
	public int Passed { get; set; }
	public int Failed { get; set; }

	public IList<TestCaseRunResult> Results { get; set; } = [];
}

public sealed class TestRunFailedException : Exception{
	public TestRunSummary Summary { get; }

	public TestRunFailedException(TestRunSummary summary)
		: base($"Test run failed. Total={summary.Total}, Passed={summary.Passed}, Failed={summary.Failed}")
	{
		Summary = summary;
	}
}

public interface ITreeTestExecutor{
	public Task<TestRunSummary> RunAsync(
		ITestNode root,
		obj? arg = null,
		OptTreeTestExecutor? options = null,
		CancellationToken cancellationToken = default
	);
}

public sealed class TreeTestExecutor : ITreeTestExecutor{
	private sealed class WorkItem{
		public required int Order { get; set; }
		public required str NodePath { get; set; }
		public required ITestNode Node { get; set; }
		public required ITestCase TestCase { get; set; }
	}

	public async Task<TestRunSummary> RunAsync(
		ITestNode root,
		obj? arg = null,
		OptTreeTestExecutor? options = null,
		CancellationToken cancellationToken = default
	){
		ArgumentNullException.ThrowIfNull(root);
		options ??= new OptTreeTestExecutor();

		var startedAt = DateTimeOffset.Now;
		var workItems = CollectWorkItems(root);
		var results = new ConcurrentBag<TestCaseRunResult>();
		var parallelOptions = new ParallelOptions{
			CancellationToken = cancellationToken,
			MaxDegreeOfParallelism = options.MaxDegreeOfParallelism <= 0
				? Environment.ProcessorCount
				: options.MaxDegreeOfParallelism,
		};

		await Parallel.ForEachAsync(workItems, parallelOptions, async (item, _) => {
			var begin = DateTimeOffset.Now;
			try{
				await item.TestCase.FnTest(arg);
				results.Add(new TestCaseRunResult{
					Order = item.Order,
					NodePath = item.NodePath,
					Node = item.Node,
					TestCase = item.TestCase,
					IsPassed = true,
					Exception = null,
					Elapsed = DateTimeOffset.Now - begin,
				});
			}
			catch(Exception ex){
				results.Add(new TestCaseRunResult{
					Order = item.Order,
					NodePath = item.NodePath,
					Node = item.Node,
					TestCase = item.TestCase,
					IsPassed = false,
					Exception = ex,
					Elapsed = DateTimeOffset.Now - begin,
				});
			}
		});

		var ordered = results.OrderBy(x => x.Order).ToList();
		var passed = ordered.Count(x => x.IsPassed);
		var failed = ordered.Count - passed;

		return new TestRunSummary{
			StartedAt = startedAt,
			EndedAt = DateTimeOffset.Now,
			Total = ordered.Count,
			Passed = passed,
			Failed = failed,
			Results = ordered,
		};
	}

	private static IList<WorkItem> CollectWorkItems(ITestNode root){
		var order = 0;
		var output = new List<WorkItem>();
		Dfs(root, "0", output, ref order);
		return output;
	}

	private static void Dfs(
		ITestNode node,
		str path,
		IList<WorkItem> output,
		ref int order
	){
		if(node.Data is ITestCase testCase){
			output.Add(new WorkItem{
				Order = order++,
				NodePath = path,
				Node = node,
				TestCase = testCase,
			});
		}

		for(var i = 0; i < node.Children.Count; i++){
			Dfs(node.Children[i], $"{path}/{i}", output, ref order);
		}
	}
}

public static class ExtnTreeTestExecutor{
	extension(ITestNode z){
		[Doc("Run all test cases under current tree root (parallel by default), no default consume")]
		public Task<TestRunSummary> RunTests(
			obj? Arg = default,
			OptTreeTestExecutor? Opt = default,
			CT Ct = default
		){
			ITreeTestExecutor executor = new TreeTestExecutor();
			return executor.RunAsync(z, Arg, Opt, Ct);
		}

		[Doc("Run tests then do default consume: print summary and throw when failed")]
		public async Task<TestRunSummary> RunEtPrint(
			obj? Arg = default,
			OptTreeTestExecutor? Opt = default,
			CT Ct = default,
			bool ThrowOnFailed = true
		){
			var summary = await z.RunTests(Arg, Opt, Ct);
			WriteSummary(summary);
			if(ThrowOnFailed && summary.Failed > 0){
				throw new TestRunFailedException(summary);
			}
			return summary;
		}

		private static void WriteSummary(TestRunSummary summary){
			Console.WriteLine($"[CsTest] Total={summary.Total}, Passed={summary.Passed}, Failed={summary.Failed}, Elapsed={summary.Elapsed}");
			foreach(var result in summary.Results.OrderBy(x => x.Order)){
				var status = result.IsPassed ? "PASS" : "FAIL";
				Console.WriteLine($"[{status}] {result.NodePath} {result.TestCase.UniqName} ({result.Elapsed})");
				if(result.Exception is not null){
					Console.WriteLine(result.Exception.ToString());
				}
			}
		}
	}
}
