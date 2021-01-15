using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace H.Hooks.IntegrationTests
{
    [TestClass]
    public class SimpleTests
    {
        [TestMethod]
        public async Task LowLevelKeyboardHookTest()
        {
            using var cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(30));
            var cancellationToken = cancellationTokenSource.Token;

            using var hook = new LowLevelKeyboardHook();
            hook.KeyUp += (_, args) => Console.WriteLine($"{nameof(hook.KeyUp)}: {args}");
            hook.KeyDown += (_, args) => Console.WriteLine($"{nameof(hook.KeyDown)}: {args}");

            hook.Start();

            await Task.Delay(TimeSpan.FromSeconds(5), cancellationToken);
        }
    }
}
