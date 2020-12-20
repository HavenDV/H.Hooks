using System;
using H.Hooks.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace H.Hooks.UnitTests
{
    [TestClass]
    public class Kernel32MethodsTests
    {
        [TestMethod]
        public void GetCurrentProcessModuleHandle()
        {
            var ptr = Kernel32Methods.GetCurrentProcessModuleHandle();
            
            Assert.AreNotEqual(IntPtr.Zero, ptr);
        }
    }
}
