using MyProj1.Test;
using Tsinswreng.CsTreeTest;

var mgr = MyProj1TestMgr.Inst;
ITestExecutor executor = new TreeTestExecutor();
await executor.RunEtPrint(mgr.TestNode);
