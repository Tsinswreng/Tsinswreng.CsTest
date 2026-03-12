using System.ComponentModel.Design;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using Tsinswreng.CsCore;

namespace Tsinswreng.CsTest;

[Doc(@$"DependencyInjection and Test Manager")]
public interface IDiEtTestMgr:ITester{
	[Doc(@$"Root TestNode of the (Sub)Tree")]
	public ITestNode TestNode{get;set;}
	[Doc(@$"Functions to register types into {nameof(IServiceCollection)}")]
	public IList<Func<IServiceCollection, nil>> DiFns{get;set;}
	[Doc(@$"Functions to retrieve {nameof(ITester)}
	from {nameof(IServiceProvider)} and register it into {nameof(TestNode)}
	")]
	public IList<Func<IServiceProvider, ITestNode, nil>> RegisterTestFns{get;set;}
}

public abstract class DiEtTestMgr:IDiEtTestMgr{
	public DiEtTestMgr(){
		this.RegisterTestsInto(this.TestNode);
	}
	public ITestNode TestNode{get;set;} = new TestNode();
	public IList<Func<IServiceCollection, nil>> DiFns{get;set;} = [];
	public IList<Func<IServiceProvider, ITestNode, nil>> RegisterTestFns{get;set;} = [];
	public abstract ITestNode RegisterTestsInto(ITestNode? Test);
}


public static class ExtnDiEtTestMgr{
	extension(IDiEtTestMgr z){
		[Doc($$"""
		Register an impl class of {{nameof(ITester)}}
		both into {{nameof(ServiceCollection)}} and {{nameof(TestNode)}}
		""")]
		public void RegisterTester<
			[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)]
			T
		>(str? NewNodeUniqName = null)
			where T:class, ITester
		{
			z.DiFns.Add((SvcC)=>{
				SvcC.AddSingleton<T>();
				return NIL;
			});
			z.RegisterTestFns.Add((SvcP, TestNode)=>{
				ITester t = SvcP.GetRequiredService<T>();
				t.RegisterTestsInto(TestNode.NewChild(NewNodeUniqName));
				return NIL;
			});
		}
		[Doc(@$"Register another {nameof(IDiEtTestMgr)} as self's subnode")]
		public void RegisterSubMgr(IDiEtTestMgr SubMgr){
			SubMgr.TestNode = z.TestNode.NewChild();
			z.DiFns.AddRange(SubMgr.DiFns);
			z.RegisterTestFns.AddRange(SubMgr.RegisterTestFns);
		}
		
		[Doc(@$"Init {nameof(IServiceCollection)} and {nameof(IServiceProvider)}
		you should provide the two args by yourself, then better call this at entrance
		")]
		public IServiceProvider InitSvc(
			IServiceCollection SvcColct
			,IServiceProvider SvcProvdr
		){
			foreach(var fn in z.DiFns){
				fn(SvcColct);
			}
			SvcProvdr = SvcColct.BuildServiceProvider();
			foreach(var fn in z.RegisterTestFns){
				fn(SvcProvdr, z.TestNode);
			}
			return SvcProvdr;
		}

		[Doc(@$"Init {nameof(IServiceCollection)} then build and return {nameof(IServiceProvider)}")]
		public IServiceProvider InitSvc(IServiceCollection SvcColct){
			IServiceProvider SvcProvdr = SvcColct.BuildServiceProvider();
			return z.InitSvc(SvcColct, SvcProvdr);
		}
	}
}
