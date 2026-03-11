using Tsinswreng.CsCore;

namespace Tsinswreng.CsTest;


public delegate obj FnGetRSvcObj();

public delegate T FnGetRSvc<T>();

[Doc($$"""
Dependency Injection Manager

""")]
public interface IDiMgr{
	//public FnGetRSvcObj FnGetRSvcObj{get;set;}
	public FnGetRSvc<T> FnGetRSvc<T>();
	public void FnGetRSvc<T>(
		FnGetRSvc<T> FnGetRSvc
	);
}

public class DiMgr : IDiMgr{
	public obj FnObj{get;set;} = null!;
	public FnGetRSvc<T> FnGetRSvc<T>(){
		if(FnObj is null){
			throw new NullReferenceException("FnObj is null. You must initialize it first manually.");
		}
		return (FnGetRSvc<T>)FnObj;
	}
	public void FnGetRSvc<T>(
		FnGetRSvc<T> FnGetRSvc
	){
		this.FnObj = FnGetRSvc;
	}
}

public static class ExtnDiMgr{
	extension(IDiMgr z){
		public T GetRSvc<T>() where T : class{
			return z.FnGetRSvc<T>()();
		}
	}
}
