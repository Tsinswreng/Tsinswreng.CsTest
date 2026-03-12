using System.ComponentModel.Design;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using Tsinswreng.CsCore;

namespace Tsinswreng.CsTest;

[Doc(@$"DependencyInjection and Test Manager.
Own a {nameof(TestNode)}
Do not own {nameof(IServiceCollection)} and {nameof(IServiceProvider)}.
each test csproj should have one {nameof(IDiEtTestMgr)}.
{nameof(IDiEtTestMgr)} itself can be registered by other {nameof(IDiEtTestMgr)}
")]
public interface IDiEtTestMgr:ITester{
	[Doc(@$"the value list should only contain 0 or 1 element,
	designed as IList is for future extension
	")]
	public IDictionary<str, IList<ITestNode>> UniqName_TestNode{get;set;}
	[Doc(@$"the value list should only contain 0 or 1 element,
	designed as IList is for future extension
	")]
	public IDictionary<Type, IList<ITestNode>> TesterType_TestNode{get;set;}
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
	public IDictionary<str, IList<ITestNode>> UniqName_TestNode{get;set;} = new Dictionary<str, IList<ITestNode>>();
	public IDictionary<Type, IList<ITestNode>> TesterType_TestNode{get;set;} = new Dictionary<Type, IList<ITestNode>>();
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
				if(z.TesterType_TestNode.ContainsKey(typeof(T))){
					throw new InvalidOperationException(
						$"Duplicated tester type: {typeof(T)}"
					);
				}
				if(NewNodeUniqName != null && z.UniqName_TestNode.ContainsKey(NewNodeUniqName)){
					throw new InvalidOperationException($"Duplicated test node uniq name: {NewNodeUniqName}");
				}
				ITester t = SvcP.GetRequiredService<T>();
				ITestNode node = TestNode.NewChild<T>(NewNodeUniqName);
				if(z.UniqName_TestNode.ContainsKey(node.UniqName)){
					throw new InvalidOperationException($"Duplicated test node uniq name: {node.UniqName}");
				}
				z.UniqName_TestNode.Add(node.UniqName, [node]);
				z.TesterType_TestNode.Add(typeof(T), [node]);
				t.RegisterTestsInto(node);
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
		
		public ITestNode GetNodeByName(str UniqName){
			if(!z.UniqName_TestNode.TryGetValue(UniqName, out var nodes) || nodes.Count < 1){
				throw new KeyNotFoundException($"Test node not found by uniq name: {UniqName}");
			}
			return nodes[0];
		}
		
		public ITestNode GetNodeByTesterType(Type TesterType){
			if(!z.TesterType_TestNode.TryGetValue(TesterType, out var nodes) || nodes.Count < 1){
				throw new KeyNotFoundException($"Test node not found by tester type: {TesterType}");
			}
			return nodes[0];
		}
	}
}
