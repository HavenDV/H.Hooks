namespace H.Hooks.UnitTests;

[TestClass]
public class Tests
{
    [TestMethod]
    public void KeysTest()
    {
        new Keys(Key.Escape, Key.Control).Are(Key.Control, Key.Escape).Should().BeTrue();
        (new Keys(Key.Escape, Key.Control) == new Keys(Key.Control, Key.Escape)).Should().BeTrue();
    }
}
