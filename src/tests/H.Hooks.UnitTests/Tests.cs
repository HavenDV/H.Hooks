namespace H.Hooks.UnitTests;

[TestClass]
public class Tests
{
    [TestMethod]
    public async Task DelayTest()
    {
        using var source = new CancellationTokenSource(TimeSpan.FromSeconds(5));

        await Task.Delay(TimeSpan.FromSeconds(1), source.Token);
    }
}
