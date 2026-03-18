using Microsoft.Extensions.DependencyInjection;
using MyProj1.Test;
using MyProj1.Test.MyDomains.Calculator;
using Tsinswreng.CsTreeTest;




public class Program{
	public static IServiceCollection SvcColct = new ServiceCollection();
	public static IServiceProvider SvcProvdr = null!;
	public static async Task Main(string[] args){
		SvcColct
			//.SetupXxx()
			.AddSingleton<ICalculator, Calculator>();
		;
		var mgr = MyProj1TestMgr.Inst;
		SvcProvdr = mgr.InitSvc(SvcColct, sc => sc.BuildServiceProvider());
		ITestExecutor executor = new TreeTestExecutor();
		await executor.RunEtPrint(mgr.TestNode);
		
	}
}
