using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using Tsinswreng.CsCore;

namespace Tsinswreng.CsTest;

public interface IDiEtTestMgr:I_RegisterTests{
	public ITestNode TestNode{get;set;}
	public IList<Func<IServiceCollection, nil>> DiFns{get;set;}
	public IList<Func<IServiceProvider, ITestNode, nil>> RegisterTestFns{get;set;}
}

public abstract class DiEtTestMgr:IDiEtTestMgr{
	public ITestNode TestNode{get;set;} = new TestNode();
	public IList<Func<IServiceCollection, nil>> DiFns{get;set;} = [];
	public IList<Func<IServiceProvider, ITestNode, nil>> RegisterTestFns{get;set;} = [];
	public abstract ITestNode RegisterTests(ITestNode? Test);
}


public static class ExtnDiEtTestMgr{
	extension(IDiEtTestMgr z){
		public void RegisterTesterType<
			[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)]
			T
		>()
			where T:class, I_RegisterTests
		{
			z.DiFns.Add((SvcC)=>{
				SvcC.AddSingleton<T>();
				return NIL;
			});
			z.RegisterTestFns.Add((SvcP, TestNode)=>{
				I_RegisterTests t = SvcP.GetRequiredService<T>();
				t.RegisterTests(TestNode.NewChild());
				return NIL;
			});
		}
		public void RegisterSubMgr(IDiEtTestMgr SubMgr){
			SubMgr.TestNode = z.TestNode.NewChild();
			z.DiFns.AddRange(SubMgr.DiFns);
			z.RegisterTestFns.AddRange(SubMgr.RegisterTestFns);
		}
	}
}
