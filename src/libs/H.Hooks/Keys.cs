namespace H.Hooks;

/// <summary>
/// 
/// </summary>
public class Keys
{
    #region Static methods

    /// <summary>
    /// 
    /// </summary>
    /// <param name="text"></param>
    /// <exception cref="ArgumentNullException"></exception>
    /// <returns></returns>
    public static Keys Parse(string text)
    {
        text = text ?? throw new ArgumentNullException(nameof(text));

        return new Keys(text.Split('+')
            .Select(static value => value.Trim())
            .Select(KeyExtensions.Parse)
            .ToArray());
    }

    #endregion

    #region Properties

    /// <summary>
    /// 
    /// </summary>
    public IReadOnlyCollection<Key> Values { get; }

    /// <summary>
    /// 
    /// </summary>
    public bool IsMouseLeft => Values.Contains(Key.MouseLeft);

    /// <summary>
    /// 
    /// </summary>
    public bool IsMouseRight => Values.Contains(Key.MouseRight);

    /// <summary>
    /// 
    /// </summary>
    public bool IsMouseMiddle => Values.Contains(Key.MouseMiddle);

    /// <summary>
    /// 
    /// </summary>
    public bool IsRightCtrl => Values.Contains(Key.RControl);

    /// <summary>
    /// 
    /// </summary>
    public bool IsRightAlt => Values.Contains(Key.RAlt);

    /// <summary>
    /// 
    /// </summary>
    public bool IsRightShift => Values.Contains(Key.RShift);

    /// <summary>
    /// 
    /// </summary>
    public bool IsLeftCtrl => Values.Contains(Key.LControl);

    /// <summary>
    /// 
    /// </summary>
    public bool IsLeftAlt => Values.Contains(Key.LAlt);

    /// <summary>
    /// 
    /// </summary>
    public bool IsLeftShift => Values.Contains(Key.LShift);
    
    /// <summary>
    /// 
    /// </summary>
    public bool IsAlt => IsRightAlt || IsLeftAlt || Values.Contains(Key.Alt);

    /// <summary>
    /// 
    /// </summary>
    public bool IsCtrl => IsLeftCtrl || IsRightCtrl || Values.Contains(Key.Control);

    /// <summary>
    /// 
    /// </summary>
    public bool IsShift => IsRightShift || IsLeftShift || Values.Contains(Key.Shift);

    /// <summary>
    /// 
    /// </summary>
    public bool IsEmpty => !Values.Any();

    #endregion

    #region Constructors

    /// <summary>
    /// 
    /// </summary>
    /// <param name="values"></param>
    public Keys(params Key[] values)
    {
        Values = values;
    }

    #endregion

    #region Public methods

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public bool Are(params Key[] values)
    {
        values = values ?? throw new ArgumentNullException(nameof(values));

        return
            Values.Count == values.Length &&
            Values.All(value => values.Contains(value));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        return string.Join("+", Values
            .OrderBy(key => key)
            .Select(value => $"{value.ToFixedString()}"));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public override bool Equals(object? obj)
    {
        if (obj is not Keys other)
        {
            return false;
        }

        return Equals(other);
    }

    private bool Equals(Keys other)
    {
        return 
            Values.Count == other.Values.Count &&
            Values.All(value => other.Values.Contains(value));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public override int GetHashCode()
    {
        unchecked
        {
            return Values
                .OrderBy(key => key)
                .Aggregate(
                    27,
                    static (current, value) => (current * 397) ^ value.GetHashCode());
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <returns></returns>
    public static bool operator ==(Keys left, Keys right)
    {
        return Equals(left, right);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <returns></returns>
    public static bool operator !=(Keys left, Keys right)
    {
        return !Equals(left, right);
    }

    #endregion
}
