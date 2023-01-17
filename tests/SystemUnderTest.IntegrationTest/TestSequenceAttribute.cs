namespace SystemUnderTest.IntegrationTest;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public class TestSequenceAttribute : Attribute
{
    public TestSequenceAttribute(int priority)
    {
        Priority = priority;
    }

    public int Priority { get; private set; }
}
