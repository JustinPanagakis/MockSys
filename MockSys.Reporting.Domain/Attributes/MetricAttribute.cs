namespace MockSys.Reporting.Domain.Attributes;

public sealed class MetricAttribute : Attribute
{
    /// <summary>
    /// The key used to register and look up this metric runner.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Construct a MetricAttribute with the specified key name.
    /// </summary>
    /// <param name="name">The unique key for this metric runner.</param>
    public MetricAttribute(string name)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
    }
}
