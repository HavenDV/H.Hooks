namespace H.Hooks.IntegrationTests;

[TestClass]
public class KeyboardTests
{
    [TestMethod]
    public async Task DefaultTest()
    {
        using var cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(30));
        var cancellationToken = cancellationTokenSource.Token;

        using var hook = new LowLevelKeyboardHook().WithEventLogging();

        hook.Start();

        await Task.Delay(TimeSpan.FromSeconds(5), cancellationToken);
    }

    [TestMethod]
    public async Task HandlingTest()
    {
        using var cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(30));
        var cancellationToken = cancellationTokenSource.Token;

        using var hook = new LowLevelKeyboardHook
        {
            Handling = true,
        }.WithEventLogging();
        hook.Up += (_, args) => args.IsHandled = true;
        hook.Down += (_, args) => args.IsHandled = true;

        hook.Start();

        await Task.Delay(TimeSpan.FromSeconds(5), cancellationToken);
    }

    [TestMethod]
    public async Task ExtendedModeTest()
    {
        using var cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(30));
        var cancellationToken = cancellationTokenSource.Token;

        using var hook = new LowLevelKeyboardHook
        {
            IsExtendedMode = true,
        }.WithEventLogging();

        hook.Start();

        await Task.Delay(TimeSpan.FromSeconds(5), cancellationToken);
    }

    [TestMethod]
    public async Task LeftRightGranularityTest()
    {
        using var cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(30));
        var cancellationToken = cancellationTokenSource.Token;

        using var hook = new LowLevelKeyboardHook
        {
            IsLeftRightGranularity = true,
        }.WithEventLogging();

        hook.Start();

        await Task.Delay(TimeSpan.FromSeconds(5), cancellationToken);
    }

    [TestMethod]
    public async Task CapsLockTest()
    {
        using var cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(30));
        var cancellationToken = cancellationTokenSource.Token;

        using var hook = new LowLevelKeyboardHook
        {
            IsCapsLock = true,
        }.WithEventLogging();

        hook.Start();

        await Task.Delay(TimeSpan.FromSeconds(5), cancellationToken);
    }
}
