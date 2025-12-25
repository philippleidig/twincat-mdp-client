using Microsoft.VisualStudio.TestTools.UnitTesting;

// Configure test execution settings
[assembly: Parallelize(Workers = 0, Scope = ExecutionScope.MethodLevel)]
